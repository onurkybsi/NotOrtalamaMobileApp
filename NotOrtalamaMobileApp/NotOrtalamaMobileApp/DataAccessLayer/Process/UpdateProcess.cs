﻿using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotOrtalamaMobileApp.DataAccessLayer.Process
{
    public class UpdateProcess : IProcessThatEntitiesCanBeSpecified
    {
        public string TableName { get; set; }
        public int EntityId { get; set; }
        public IEntity Entity { get; set; }
        public Type ProcessType => typeof(UpdateProcess);
    }
}
