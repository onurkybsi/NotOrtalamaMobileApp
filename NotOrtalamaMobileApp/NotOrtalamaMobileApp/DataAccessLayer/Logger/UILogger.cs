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

        public Func<Task> Log(string message) => async () => { await CurrentPage.DisplayAlert("Kayıt silme", message, "OK"); };

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

        private async Task InsertLog(IProcess process)
        {
            string message = process.Entity != null
                ? string.Format("{0} eklendi!", process.Entity.DecisiveName)
                : string.Format("Id {0} eklendi!", process.EntityId);

            await CurrentPage.DisplayAlert("Yeni Kayıt", message, "OK");
        }

        private async Task DeleteLog(IProcess process) => await CurrentPage.DisplayAlert("Kayıt silme", string.Format("Id {0} silindi!", process.EntityId), "OK");
    }
}
