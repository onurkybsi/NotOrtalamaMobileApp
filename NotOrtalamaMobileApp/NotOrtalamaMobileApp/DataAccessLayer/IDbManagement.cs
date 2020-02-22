using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public interface IDbManagement
    {
        Task<IEnumerable<IEntity>> GetAllEntities();
        Task CreateTable();
        Task<IEntity> GetEntity(int Id);
        Task InsertEntity(IEntity entity);
        Task UpdateEntity(IEntity entity);
        Task DeleteEntity(int Id);
        Task DeleteAllEntities();
    }
}
