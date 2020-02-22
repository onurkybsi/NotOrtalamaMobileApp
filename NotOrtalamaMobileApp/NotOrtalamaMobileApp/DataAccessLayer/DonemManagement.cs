using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DonemManagement : IDbManagement
    {
        private SQLiteAsyncConnection database;

        public DonemManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        async public Task CreateTable() => await database.CreateTableAsync<Donem>();
        
        async public Task<IEnumerable<IEntity>> GetAllEntities() => await database.Table<Donem>().ToListAsync();

        async public Task<IEntity> GetEntity(int Id) => await database.GetAsync<Donem>(Id);

        async public Task InsertEntity(IEntity donem) => await database.InsertAsync(donem);

        async public Task UpdateEntity(IEntity donem) => await database.UpdateAsync(donem);

        async public Task DeleteEntity(int Id) => await database.DeleteAsync(Id);

        async public Task DeleteAllEntities() => await database.ExecuteAsync("DELETE FROM DonemTable");

    }
}
