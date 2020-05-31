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
        async public Task UpdateEntity<T>(string tableName, List<KeyValuePair<string, object>> filter, List<KeyValuePair<string, object>> newValues, Func<Task> callBack) where T : IEntity, new()
        {

            var queryParameters = BuildSQLCommandToBeExecute(tableName, filter, new UpdateProcess(), newValues);

            string filterExpressions = (string)queryParameters[0];
            object[] args = (object[])queryParameters[1];

            await database.QueryAsync<T>(filterExpressions, args);
            if (callBack != null)
                await EventOfManipulation(callBack);
        }

        async public Task UpdateEntity<T>(string tableName, List<KeyValuePair<string, object>> filter, List<KeyValuePair<string, object>> newValues) where T : IEntity, new()
            => await UpdateEntity<T>(tableName, filter, newValues, null);
    }
}
