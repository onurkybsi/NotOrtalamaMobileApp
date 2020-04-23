using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp
{
    public partial class App : Application
    {
        public static IDbManagement dbManagement;

        public App()
        {
            dbManagement = DbManagement.CreateAsSingleton();

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
            await dbManagement.CreateTable<Donem>();
        }

        protected async override void OnSleep()
        {
            foreach (int donemId in (await dbManagement.GetAllEntities<Ders>()).Select(x => x.DonemId).Distinct())
            {
                if (await dbManagement.GetEntity<Donem>(x => x.Id == donemId) == null)
                {
                    await dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("DonemId", donemId)
                    }, Processes.Delete);
                }
            }
        }
    }
}
