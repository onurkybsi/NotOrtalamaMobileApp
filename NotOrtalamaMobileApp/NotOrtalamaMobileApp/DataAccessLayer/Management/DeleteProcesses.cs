using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public partial class DbManagement
    {
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
    }
}
