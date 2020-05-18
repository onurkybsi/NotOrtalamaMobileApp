using NotOrtalamaMobileApp.DataAccessLayer.Process;
using System;

namespace NotOrtalamaMobileApp.DataAccessLayer.Logger
{
    public interface ILogger
    {
        Action<IProcess> Log(IProcess process);
    }
}
