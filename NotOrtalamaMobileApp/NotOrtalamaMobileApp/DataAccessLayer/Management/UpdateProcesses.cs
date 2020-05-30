using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public partial class DbManagement : IDbManagement
    {
        async public Task UpdateEntity<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<T, bool> updateExpressions) where T : IEntity, new()
        {
            var updatedEntity = await GetSpecifiedEntities<T>(tableName, filter);

            var result = updateExpressions.Invoke(new T());

            var queryParameters = BuildSQLCommandToBeExecute(tableName, filter, new UpdateProcess());

            string filterExpressions = (string)queryParameters[0];
            object[] args = (object[])queryParameters[1];

            await database.QueryAsync<T>(filterExpressions, args);          
        }
    }
}
