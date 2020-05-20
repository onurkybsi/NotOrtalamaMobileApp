using NotOrtalamaMobileApp.DataAccessLayer.Process;
using System;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.DataAccessLayer.Logger
{
    public interface ILogger
    {
        Func<Task> Log(IProcess processToBeInvoked);
    }
}
