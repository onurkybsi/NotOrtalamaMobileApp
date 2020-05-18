using NotOrtalamaMobileApp.Tables;
using System;

namespace NotOrtalamaMobileApp.DataAccessLayer.Process
{
    public interface IProcess
    {
        string TableName { get; set; }
        int EntityId { get; set; }
        IEntity Entity { get; set; }
        Type ProcessType { get; }
    }
}
