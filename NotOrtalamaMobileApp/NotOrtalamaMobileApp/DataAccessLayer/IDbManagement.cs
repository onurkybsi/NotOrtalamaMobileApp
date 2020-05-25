using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public interface IDbManagement
    {
        Task<CreateTableResult> CreateTable<T>() where T : IEntity, new();
        Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new();
        /// <summary>
        ///     Performs getting or deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// </param>
        /// <param name="processes">
        ///    The instance of process that describe database process.Can be get or delete.
        /// </param>
        /// <param name="callBack">
        ///     Process to invoke after the database process is over
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process, Func<Task> callBack) where T : IEntity, new();
        /// <summary>
        ///     Performs getting or deleting entities that the given conditions.
        /// </summary>
        /// <param name="tableName">
        ///     The name of the table containing the entities to be processed.
        /// </param>
        /// <param name="filter">
        ///     Property name and value for the required conditions.
        /// </param>
        /// <param name="processes">
        ///    The instance of process that describe database process.Can be get or delete.
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process) where T : IEntity, new();
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity, string tableName) where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity, string tableName, Func<Task> callBack) where T : IEntity, new();
        Task DeleteEntity<T>(int id, string tableName) where T : IEntity, new();
        Task DeleteEntity<T>(int id, string tableName, Func<Task> callBack) where T : IEntity, new();
    }
}
