using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YanoCalculationPage : ContentPage
    {
        public YanoCalculationPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            lessonToBeDeleted.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
        }

        private async void addCourseToSemester_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddLesson());
        }

        private async void deleteLesson_Clicked(object sender, EventArgs e)
        {
            if(!Object.Equals(lessonToBeDeleted.SelectedItem, null) && await DisplayAlert("Ders Sil","Emin misiniz ?","Evet","Hayır"))
            {
                int toBeDeleted = (lessonToBeDeleted.SelectedItem as Ders).Id;

                await App.dbManagement.DeleteEntity<Ders>(toBeDeleted, "DersTable");

                lessonToBeDeleted.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
            }
            else { }
        }
    }
}