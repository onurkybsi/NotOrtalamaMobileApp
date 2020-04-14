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

        private DbManagement() {    database = DependencyService.Get<ISQLiteDb>().GetConnection();  }

        public static DbManagement CreateAsSingleton() => _dbManagement ?? (_dbManagement = new DbManagement());

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();

        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();

        async public Task<List<T>> GetSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("SELECT * FROM " + tableName + " WHERE DonemId = ?", donemId);

        async public Task<List<T>> GetSpecifiedEntities<T>(string courseName, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("SELECT * FROM " + tableName + " WHERE DersAdi = ?", courseName);

        ///<summary>
        ///<para>The values of the dictionary that this method takes as a parameter must be string or integer.</para>
        ///</summary>
        async public Task<List<T>> GetSpecifiedEntities<T>(string tableName, Dictionary<string, object> filter) where T : IEntity, new()
        {
            string filterExpressions = "SELECT * FROM " + tableName + " WHERE";
            object[] args = new object[filter.Count];

            foreach (var filterExpression in filter)
            {
                if(filterExpression.Value.GetType() != typeof(int) && filterExpression.Value.GetType() != typeof(string))
                    throw new ArgumentException("Type " + filterExpression.Value.GetType() + " is not handled.");

                filterExpressions += string.Format(" {0} = ? AND", filterExpression.Key); 
            }

            filterExpressions = filterExpressions.Remove(filterExpressions.Length - 3, 3);
            filter.Values.CopyTo(args, 0);

            return await database.QueryAsync<T>(filterExpressions, args);
        }
            
        async public Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {
            try
            {
                return await database.GetAsync<T>(predicate);
            }
            catch { return null; }
        }

        async public Task InsertEntity<T>(IEntity entity) where T : IEntity, new() => await database.InsertAsync(entity);

        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();

        async public Task<List<T>> DeleteSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("DELETE FROM " + tableName + " WHERE DonemId = ?", donemId);

        async public Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new() => await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", Id);
    }
}
