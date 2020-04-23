using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DbManagement : IDbManagement
    {
        private static DbManagement _dbManagement;

        private SQLiteAsyncConnection database;

        private DbManagement() { database = DependencyService.Get<ISQLiteDb>().GetConnection(); }
        public static DbManagement CreateAsSingleton() => _dbManagement ?? (_dbManagement = new DbManagement());

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();
        async public Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Processes processes) where T : IEntity, new()
        {
            string filterExpressions = processes == Processes.Get
                ? "SELECT * FROM " + tableName + " WHERE"
                : "DELETE FROM " + tableName + " WHERE";

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

            return await database.QueryAsync<T>(filterExpressions, args);
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
        async public Task InsertEntity<T>(IEntity entity) where T : IEntity, new() => await database.InsertAsync(entity);
        async public Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new() => await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", Id);
        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();
    }
}
