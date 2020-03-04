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

            dbManagement = new DbManagement();



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
        }

        protected async override void OnSleep()
        {
            foreach (int donemId in (await dbManagement.GetAllEntities<Ders>()).Select(x => x.DonemId).Distinct())
            {
                try
                { 
                    IEntity donem = await dbManagement.GetEntity<Donem>(donemId); 
                }
                catch   { await dbManagement.DeleteSpecifiedEntities<Ders>(donemId, "DersTable"); }
            }
        }

        protected override void OnResume()
        {
        }
    }
}
