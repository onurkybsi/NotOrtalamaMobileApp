using NotOrtalamaMobileApp.DataAccessLayer.Process;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer.Logger
{
    public class UILogger : ILogger
    {
        private static Page CurrentPage { get; set; }

        public UILogger(Application app)
        {
            app.PageAppearing += SetCurrentPage;
        }

        private static void SetCurrentPage(object sender, Page e) => CurrentPage = e;

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

        private async Task InsertLog(IProcess process) => await CurrentPage.DisplayAlert("Yeni kayıt", string.Format("Entity id {0} inserted", process.Entity.Id), "OK");

        private async Task DeleteLog(IProcess process) => await CurrentPage.DisplayAlert("Kayıt silme", string.Format("Entity id {0} deleted", process.EntityId), "OK");
    }
}
