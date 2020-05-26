using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public partial class DbManagement
    {
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
    }
}
