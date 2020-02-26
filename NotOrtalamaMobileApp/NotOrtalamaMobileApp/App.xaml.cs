using NotOrtalamaMobileApp.DataAccessLayer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
