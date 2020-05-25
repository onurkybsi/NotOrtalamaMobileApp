using NotOrtalamaMobileApp.Tables;
using System;

namespace NotOrtalamaMobileApp.DataAccessLayer.Process
{
    public class GetProcess : IProcessThatEntitiesCanBeSpecified
    {
        public string TableName { get; set; }
        public int EntityId { get; set; }
        public IEntity Entity { get; set; }
        public Type ProcessType => typeof(GetProcess);
    }
}
