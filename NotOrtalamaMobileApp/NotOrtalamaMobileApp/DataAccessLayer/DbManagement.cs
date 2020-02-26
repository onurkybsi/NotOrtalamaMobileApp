using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DbManagement : IDbManagement
    {
        private SQLiteAsyncConnection database;

        public DbManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();

        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();

        async public Task DeleteAllEntities<T>() where T : IEntity, new() => await database.DeleteAllAsync<T>();

        async public Task DeleteEntity<T>(int Id) where T : IEntity, new() => await database.ExecuteAsync("DELETE FROM DersTable WHERE _id = ?", Id);

        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();

        async public Task<IEntity> GetEntity<T>(int Id) where T : IEntity, new() => await database.GetAsync<T>(Id);

        async public Task InsertEntity<T>(IEntity entity) where T : IEntity, new() => await database.InsertAsync(entity);
    }
}
