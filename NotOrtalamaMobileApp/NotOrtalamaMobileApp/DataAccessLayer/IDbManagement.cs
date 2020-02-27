using NotOrtalamaMobileApp.Tables;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public interface IDbManagement
    {
        Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new();
        Task<CreateTableResult> CreateTable<T>() where T : IEntity, new();
        Task<IEntity> GetEntity<T>(int Id) where T : IEntity, new();
        Task InsertEntity<T>(IEntity entity) where T : IEntity, new();
        Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new();
        Task DeleteAllEntities<T>() where T : IEntity, new();
        Task DeleteTransientSemesters();
        Task DbSil<T>() where T : IEntity, new();
    }
}
