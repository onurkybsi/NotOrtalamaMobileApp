using NotOrtalamaMobileApp.DataAccessLayer.Process;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer.Logger
{
    public class UILogger : ILogger
    {
        private Page _currentPage;

        public UILogger(Page currentPage)
        {
            _currentPage = currentPage;
        }

        public Func<Task> Log(IProcess process)
        {
            Func<Task> actionToBeInvoked = null;

            if (process.ProcessType == typeof(InsertProcess))
            {
                actionToBeInvoked = async () =>
                {
                    await InsertLog(process);
                };
            }
            else if (process.ProcessType == typeof(DeleteProcess))
            {
                actionToBeInvoked = async () =>
                {
                    await DeleteLog(process);
                };
            }

            return actionToBeInvoked;
        }

        private async Task InsertLog(IProcess process) => await _currentPage.DisplayAlert("Insert Log", string.Format("Entity id {0} inserted", process.Entity.Id), "OK");

        private async Task DeleteLog(IProcess process) => await _currentPage.DisplayAlert("Delete Log", string.Format("Entity id {0} deleted", process.Entity.Id), "OK");
    }
}
