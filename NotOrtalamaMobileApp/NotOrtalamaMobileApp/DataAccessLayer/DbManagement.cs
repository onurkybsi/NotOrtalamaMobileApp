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
        private SQLiteAsyncConnection database;

        private static DbManagement _dbManagement;

        private DbManagement() {    database = DependencyService.Get<ISQLiteDb>().GetConnection();  }

        public static DbManagement CreateAsSingleton() => _dbManagement ?? (_dbManagement = new DbManagement());

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();

        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();

        async public Task<List<T>> DeleteSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("DELETE FROM " + tableName + " WHERE DonemId = ?", donemId);

        async public Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new() => await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", Id);

        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();

        async public Task<IEntity> GetEntity<T>(int Id) where T : IEntity, new()
        {
            try
            {
                return await database.GetAsync<T>(Id);
            }
            catch { return null; }
        }

        async public Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {
            try
            {
                return await database.GetAsync<T>(predicate);
            }
            catch { return null; }
        }

        async public Task<List<T>> GetSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("SELECT * FROM " + tableName + " WHERE DonemId = ?", donemId);
        
        async public Task<List<T>> GetSpecifiedEntities<T>(string courseName, string tableName) where T : IEntity, new() => await database.QueryAsync<T>("SELECT * FROM " + tableName + " WHERE DersAdi = ?", courseName);

        async public Task InsertEntity<T>(IEntity entity) where T : IEntity, new() => await database.InsertAsync(entity);
    }
}
