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
        private DonemManagement donemManagement;

        public SemesterRepo()
        {
            InitializeComponent();

            donemManagement = new DonemManagement();
        }

        protected async override void OnAppearing()
        {
            await donemManagement.CreateTable();

            var donem1 = new Donem();

            await donemManagement.InsertEntity(donem1);

            var dersler = new List<Ders>
            {
                new Ders
                {
                    DersAdi = "Programlama",
                    HarfNotu = "AA",
                    Kredi = 4,
                    DonemId = donem1.Id
                }
            };

            listView.ItemsSource = await donemManagement.GetAllEntities();
        }
    }
}