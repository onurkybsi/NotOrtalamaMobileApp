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
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task<CreateTableResult> CreateTable<T>() where T : IEntity, new();
        Task<IEntity> GetEntity<T>(int Id) where T : IEntity, new();
        Task<IEntity> GetEntity<T>(Action<T> predicate) where T : IEntity, new();
        Task<List<T>> GetSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new();
        Task<List<T>> GetSpecifiedEntities<T>(string courseName, string tableName) where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity) where T : IEntity, new();
        Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new();
        Task DeleteAllEntities<T>() where T : IEntity, new();
        Task<List<T>> DeleteSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new();
        Task DeleteTransientSemesters();
        Task DbSil<T>() where T : IEntity, new();
    }
}
