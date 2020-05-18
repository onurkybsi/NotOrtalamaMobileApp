using NotOrtalamaMobileApp.Tables;
using System;

namespace NotOrtalamaMobileApp.DataAccessLayer.Process
{
    public class InsertProcess : IProcess
    {
        public string TableName { get; set; }
        public int EntityId { get; set; }
        public IEntity Entity { get; set; }
        public Type ProcessType => typeof(InsertProcess);
    }
}
