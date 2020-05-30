using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public interface IDbManagement
    {
        Task<CreateTableResult> CreateTable<T>() where T : IEntity, new();
        Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new();
        /// <summary>
        ///     Performs deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// </param>
        /// <param name="callBack">
        ///     Process to invoke after the getting process is over
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> GetSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<Task> callBack) where T : IEntity, new();
        /// <summary>
        ///     Performs deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// <returns>Task<List<T>></returns>
        Task<List<T>> GetSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter) where T : IEntity, new();
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity, string tableName, Func<Task> callBack) where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity, string tableName) where T : IEntity, new();
        Task UpdateEntity<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<T, bool> updateExpressions) where T : IEntity, new();
        Task DeleteEntity<T>(int id, string tableName, Func<Task> callBack) where T : IEntity, new();
        Task DeleteEntity<T>(int id, string tableName) where T : IEntity, new();
        /// <summary>
        ///     Performs deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// </param>
        /// <param name="callBack">
        ///     Process to invoke after the deleting process is over
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> DeleteSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Func<Task> callBack) where T : IEntity, new();
        /// <summary>
        ///     Performs deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> DeleteSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter) where T : IEntity, new();
        Task DbSil<T>() where T : IEntity, new();
    }
}
