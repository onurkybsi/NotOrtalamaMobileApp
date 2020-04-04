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
        public static DbManagement dbManagement;

        public App()
        {
            InitializeComponent();

            dbManagement = DbManagement.CreateAsSingleton();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.LightGray,
                BarTextColor = Color.Black
            };
        }

        protected async override void OnStart()
        {
            //await App.dbManagement.DbSil<Ders>();
            //await App.dbManagement.DbSil<Donem>();

            await dbManagement.CreateTable<Ders>();
            await dbManagement.CreateTable<Donem>();

            foreach (int donemId in (await App.dbManagement.GetAllEntities<Ders>()).Select(x => x.DonemId).Distinct())
            {
                if (await App.dbManagement.GetEntity<Donem>(donemId) == null)
                    await App.dbManagement.DeleteSpecifiedEntities<Ders>(donemId, "DersTable");
            }
        }
    }
}
