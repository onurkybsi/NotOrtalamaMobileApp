using Autofac;
using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.DataAccessLayer.Logger;
using NotOrtalamaMobileApp.Dependency;
using NotOrtalamaMobileApp.Tables;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp
{
    public partial class App : Application
    {
        public static IDbManagement dbManagement;
        public static ILogger logger;

        public App()
        {
            dbManagement = DbManagement.CreateAsSingleton();

            DIContainer.Initialize(this);
            logger = DIContainer.LoggerService;

            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.LightGray,
                BarTextColor = Color.Black
            };
        }

        protected async override void OnStart()
        {
            await dbManagement.CreateTable<Ders>();
        }

        protected async override void OnSleep()
        {
            await dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", 0)
            }, Processes.Delete);
        }
    }
}
