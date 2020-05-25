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
        public static DbManagement CreateAsSingleton()
        {
            return _dbManagement ?? (_dbManagement = new DbManagement());
        }

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();
        async public Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process, Func<Task> callBack) where T : IEntity, new()
        {

            var processType = process.ProcessType == typeof(DeleteProcess);

            string filterExpressions = processType
                ? "DELETE FROM " + tableName + " WHERE"
                : "SELECT * FROM " + tableName + " WHERE";

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

            var processedSpecifiedEntities = await database.QueryAsync<T>(filterExpressions, args);

            if (callBack != null && processType)
                await EventOfManipulation(callBack);

            return processedSpecifiedEntities;
        }
        async public Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process) where T : IEntity, new()
        {
            return await ProcessSpecifiedEntities<T>(tableName, filter, process, null);
        }
        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();
        async public Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {

            try
            {
                return await database.GetAsync<T>(predicate);
            }
            catch { return null; }
        }
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
        async private Task ExecuteAfterProcess(Func<Task> callBack) => await callBack.Invoke();
    }
}
