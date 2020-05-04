using NotOrtalamaMobileApp.Infrastructure;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            double agno = Calculations.AGNOCalculate(await App.dbManagement.GetAllEntities<Ders>() as List<Ders>);

            lastCalculatedAGNO.Text = (agno == -1) ? "-" : agno.ToString("##.#");
        }

        private async void toYano_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new YanoCalculationPage());
        }

        private async void toAgno_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgnoCalculationPage());
        }

        private async void yanoRepo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SemesterRepo());
        }   
    }
}
