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
    }
}