﻿using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Infrastructure;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            lastCalculatedAGNO.Text = Calculations.AGNOCalculate(await App.dbManagement.GetAllEntities<Ders>() as List<Ders>) == -1
                ? "-"
                : Calculations.AGNOCalculate(await App.dbManagement.GetAllEntities<Ders>() as List<Ders>).ToString("##.#");
        }

        private async void toYano_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new YanoCalculationPage());
        }

        private async void yanoRepo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SemesterRepo());
        }

        private async void toAgno_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AgnoCalculationPage());
        }
    }
}
