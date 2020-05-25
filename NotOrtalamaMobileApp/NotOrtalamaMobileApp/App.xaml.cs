using Autofac;
using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.DataAccessLayer.Logger;
using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Dependency;
using NotOrtalamaMobileApp.Dependency.Modules;
using NotOrtalamaMobileApp.Tables;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp
{
    public partial class App : Application
    {
        public static IDbManagement dbManagement;
        public static IContainer diContainer;
        public static ILogger logger;

        public App()
        {

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new UILoggerModule(this));
            diContainer = containerBuilder.Build();

            dbManagement = DbManagement.CreateAsSingleton();
            logger = diContainer.Resolve<ILogger>();

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
            }, new DeleteProcess());
        }
    }
}
