using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YanoCalculationPage : ContentPage
    {
        private DersManagement dersManagement;

        public YanoCalculationPage()
        {
            InitializeComponent();

            dersManagement = new DersManagement();
        }

        protected async override void OnAppearing()
        {
            await dersManagement.CreateTable();

            silinecekDersPicker.ItemsSource = await dersManagement.GetAllEntities() as List<Ders>;
        }

        private async void addCourseToSemester_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddLesson());
        }

        private async void deleteLesson_Clicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Ders Sil", "Emin misiniz ?", "Evet", "Iptal"))
            {
                var silinecekDers = silinecekDersPicker.SelectedItem as Ders;

                await dersManagement.DeleteEntity(silinecekDers.Id);
            }
            else { }
        }
    }
}