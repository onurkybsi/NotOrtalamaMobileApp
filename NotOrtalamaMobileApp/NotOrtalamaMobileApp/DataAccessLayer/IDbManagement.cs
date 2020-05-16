﻿using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        ///     The name of process.Get or delete.
        /// </param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Processes process) where T : IEntity, new();
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity) where T : IEntity, new();
        Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new();
        void SetCurrentPage(Page page);
        Task DbSil<T>() where T : IEntity, new();
    }
}
