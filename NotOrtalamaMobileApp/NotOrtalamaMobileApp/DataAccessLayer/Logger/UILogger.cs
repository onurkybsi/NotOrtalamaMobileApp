using NotOrtalamaMobileApp.DataAccessLayer.Process;
using System;
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

        public Action<IProcess> Log(IProcess process)
        {

            if (process.ProcessType == typeof(InsertProcess))
            {
                return (newProcess) => _currentPage.DisplayAlert("Insert Log", "Entity inserted", "OK");
            }
            else
                return null;
        }
    }
}
