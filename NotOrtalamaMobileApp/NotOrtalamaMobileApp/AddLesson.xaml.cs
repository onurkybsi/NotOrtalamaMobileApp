using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLesson : ContentPage
    {

        public AddLesson()
        {
            InitializeComponent();

            letterGrade.ItemsSource = new List<string>
            {
                "AA", "BA", "BB", "CB", "CC", "DC", "DD", "FD", "FF", "F0"
            };
        }

        protected async override void OnAppearing()
        {
            courseToBeAdded.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
        }

        private async void addCourse_Clicked(object sender, EventArgs e)
        {
            if (Object.Equals(courseToBeAdded.SelectedItem, null))
            {
                await App.dbManagement.InsertEntity<Ders>(new Ders
                {
                    DersAdi = courseName.Text,
                    Kredi = Convert.ToInt32(courseCredit.Text),
                    HarfNotu = letterGrade.SelectedItem.ToString(),
                    DonemId = 0
                });

                await DisplayAlert("Ders Ekle", "Eklendi !", "OK");

                await Navigation.PopAsync();
            }
            else
            {

            }
        }
    }
}