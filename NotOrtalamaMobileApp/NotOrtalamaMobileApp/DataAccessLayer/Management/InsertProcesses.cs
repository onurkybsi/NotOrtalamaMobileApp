using NotOrtalamaMobileApp.Tables;
using System;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public partial class DbManagement
    {
        async public Task InsertEntity<T>(IEntity entity, string tableName) where T : IEntity, new() => await database.InsertAsync(entity);
        async public Task InsertEntity<T>(IEntity entity, string tableName, Func<Task> callBack) where T : IEntity, new()
        {

            await database.InsertAsync(entity);
            await EventOfManipulation(callBack);
        }
    }
}
