using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DersManagement : IDbManagement
    {
        private SQLiteAsyncConnection database;

        public DersManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        async public Task CreateTable() => await database.CreateTableAsync<Ders>();

        async public Task<IEnumerable<IEntity>> GetAllEntities() => await database.Table<Ders>().ToListAsync();

        async public Task<IEntity> GetEntity(int Id) => await database.GetAsync<Ders>(Id);

        async public Task InsertEntity(IEntity ders) => await database.InsertAsync(ders);

        async public Task UpdateEntity(IEntity ders) => await database.UpdateAsync(ders);

        async public Task DeleteEntity(int Id) => await database.DeleteAsync(Id);

        async public Task DeleteAllEntities() => await database.ExecuteAsync("DELETE FROM DersTable");
    }
}
