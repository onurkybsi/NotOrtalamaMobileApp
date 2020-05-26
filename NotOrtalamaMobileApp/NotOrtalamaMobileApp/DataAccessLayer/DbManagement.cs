using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DbManagement : IDbManagement
    {
        private static DbManagement _dbManagement;
        private SQLiteAsyncConnection database;

        public delegate Task DelegateOfManipulation(Func<Task> callBack);
        public event DelegateOfManipulation EventOfManipulation;

        private DbManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
            EventOfManipulation += ExecuteAfterProcess;
        }
        public static DbManagement CreateAsSingleton() => _dbManagement ?? (_dbManagement = new DbManagement());

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();
        async public Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {

            try
            {
                return await database.GetAsync<T>(predicate);
            }
            catch { return null; }
        }
        async public Task<List<T>> GetSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<Task> callBack) where T : IEntity, new()
        {
            var queryParameters = BuildFilterExpression(tableName, filter, new GetProcess());

            string filterExpressions = (string)queryParameters[0];
            object[] args = (object[])queryParameters[1];

            var processedSpecifiedEntities = await database.QueryAsync<T>(filterExpressions, args);

            if (callBack != null)
                await EventOfManipulation(callBack);

            return processedSpecifiedEntities;
        }
        async public Task<List<T>> GetSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter) where T : IEntity, new()
        {
            return await GetSpecifiedEntities<T>(tableName, filter, null);
        }
        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();
        async public Task InsertEntity<T>(IEntity entity, string tableName) where T : IEntity, new() => await database.InsertAsync(entity);
        async public Task InsertEntity<T>(IEntity entity, string tableName, Func<Task> callBack) where T : IEntity, new()
        {

            await database.InsertAsync(entity);
            await EventOfManipulation(callBack);
        }
        async public Task DeleteEntity<T>(int id, string tableName) where T : IEntity, new() => await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", id);
        async public Task DeleteEntity<T>(int id, string tableName, Func<Task> callBack) where T : IEntity, new()
        {

            await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", id);
            await EventOfManipulation(callBack);
        }
        async public Task<List<T>> DeleteSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<Task> callBack) where T : IEntity, new()
        {
            var queryParameters = BuildFilterExpression(tableName, filter, new DeleteProcess());

            string filterExpressions = (string)queryParameters[0];
            object[] args = (object[])queryParameters[1];

            var processedSpecifiedEntities = await database.QueryAsync<T>(filterExpressions, args);

            if (callBack != null)
                await EventOfManipulation(callBack);

            return processedSpecifiedEntities;
        }
        async public Task<List<T>> DeleteSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter) where T : IEntity, new()
        {
            return await DeleteSpecifiedEntities<T>(tableName, filter, null);
        }
        async private Task ExecuteAfterProcess(Func<Task> callBack) => await callBack.Invoke();
        private static object[] BuildFilterExpression(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process)
        {
            var result = new object[2];

            string detectProcess = DetectProcess(process);

            string filterExpressions = string.Format("{0} FROM {1} WHERE", detectProcess, tableName);

            object[] args = new object[filter.Count];
            int i = 0;

            foreach (var filterExpression in filter)
            {
                if (filterExpression.Value.GetType() != typeof(int) && filterExpression.Value.GetType() != typeof(string))
                    throw new ArgumentException("Type " + filterExpression.Value.GetType() + " is not handled.");

                if (!filterExpressions.Contains(filterExpression.Key))
                {
                    filterExpressions += " (";

                    foreach (var otherFilterExpression in filter)
                    {

                        if (filterExpression.Key == otherFilterExpression.Key && filterExpression.Value != otherFilterExpression.Value)
                        {
                            filterExpressions += string.Format(" {0} = ? OR", otherFilterExpression.Key);

                            args[i] = otherFilterExpression.Value;
                            i++;
                        }
                    }

                    filterExpressions = filterExpressions.Insert(filterExpressions.Length, string.Format(" {0} = ?) AND", filterExpression.Key));
                    args[i] = filterExpression.Value;
                    i++;
                }
            }

            filterExpressions = filterExpressions.Remove(filterExpressions.Length - 4, 4);

            result[0] = filterExpressions;
            result[1] = args;

            return result;
        }
        private static string DetectProcess(IProcessThatEntitiesCanBeSpecified process)
        {
            string result = "SELECT *";

            if (process.ProcessType == typeof(DeleteProcess))
            {
                result = "DELETE";
            }

            return result;
        }
    }
}
