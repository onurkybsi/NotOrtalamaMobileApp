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
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task<List<T>> GetSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new();
        Task<List<T>> GetSpecifiedEntities<T>(string courseName, string tableName) where T : IEntity, new();
        Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity) where T : IEntity, new();
        Task DbSil<T>() where T : IEntity, new();
        Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new();
        Task<List<T>> DeleteSpecifiedEntities<T>(int donemId, string tableName) where T : IEntity, new();
    }
}
