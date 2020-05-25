using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SemesterRepo : ContentPage
    {

        async private static Task<IEnumerable<object>> Semesters()
        {
            var semesters = (await App.dbManagement.GetAllEntities<Ders>()).Select(x => x.DonemId).Distinct().OrderBy(x => x);
            List<object> semestersObjects = new List<object>();

            foreach (var semester in semesters)
            {
                semestersObjects.Add(new
                {
                    SemesterId = semester,
                    SemesterName = $"{semester}. Donem"
                });
            }

            return semestersObjects;
        }

        public SemesterRepo()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            listView.ItemsSource = await Semesters();
        }

        // Delete semester
        private async void deleteSemester(object sender, System.EventArgs e)
        {
            var semester = ((MenuItem)sender).CommandParameter;
            int clientid = (int)(semester.GetType()).GetProperty("SemesterId").GetValue(semester);

            await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", clientid)
            }, new DeleteProcess());

            listView.ItemsSource = await Semesters();

            await DisplayAlert("Dönem Sil", "Silindi !", "OK");
        }
    }
}