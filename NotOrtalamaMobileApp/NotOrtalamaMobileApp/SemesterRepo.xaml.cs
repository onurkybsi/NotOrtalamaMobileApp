using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Tables;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SemesterRepo : ContentPage
    {

        public SemesterRepo()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            listView.ItemsSource = await App.dbManagement.GetAllEntities<Donem>() as List<Donem>;
        }

        // Delete semester
        private async void deleteSemester(object sender, System.EventArgs e)
        {
            var semester = ((MenuItem)sender).CommandParameter;

            await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", (semester as Donem).Id)

            }, Processes.Delete);

            await App.dbManagement.DeleteEntity<Donem>((semester as Donem).Id, "DonemTable");

            listView.ItemsSource = await App.dbManagement.GetAllEntities<Donem>() as List<Donem>;

            await DisplayAlert("Dönem Sil", "Silindi !", "OK");
        }
    }
}