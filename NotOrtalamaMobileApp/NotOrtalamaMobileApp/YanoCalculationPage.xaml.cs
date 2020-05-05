using NotOrtalamaMobileApp.Infrastructure;
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

            semesters.ItemsSource = new List<string> { "Hiçbiri", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
        }

        // OnAppering
        protected async override void OnAppearing()
        {
            if (semesters.SelectedIndex > -1)
            {
                lessonToBeDeleted.ItemsSource = await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("DonemId", semesters.SelectedIndex)
                }, DataAccessLayer.Processes.Get);
            }
        }

        // Delete course.
        private async void deleteLesson_Clicked(object sender, EventArgs e)
        {
            if (lessonToBeDeleted.SelectedItem != null)
            {
                if (await DisplayAlert("Ders Sil", "Emin misiniz ?", "Evet", "Hayır"))
                {
                    int toBeDeleted = (lessonToBeDeleted.SelectedItem as Ders).Id;

                    await App.dbManagement.DeleteEntity<Ders>(toBeDeleted, "DersTable");

                    lessonToBeDeleted.ItemsSource = await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
                    {
                        new KeyValuePair<string, object>("DonemId", semesters.SelectedIndex)
                    }, DataAccessLayer.Processes.Get);
                }
            }
            else
            {
                await DisplayAlert("Hata", "Lütfen silinecek dersi seçin !", "OK");
            }
        }

        // To AddLesson page.
        private async void addCourseToSemester_Clicked(object sender, EventArgs e)
        {
            if (semesters.SelectedItem != null)
            {
                int donemId = semesters.SelectedIndex > 0
                    ? semesters.SelectedIndex
                    : 0;

                await Navigation.PushAsync(new AddLesson(donemId));
            }
            else
            {
                await DisplayAlert("Hata", "Lütfen hesaplanacak dönemi seçin !", "OK");
            }
        }

        private async void semesters_SelectedIndexChanged(object sender, EventArgs e)
        {

            var itemsSource = await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", (sender as Picker).SelectedIndex)
            }, DataAccessLayer.Processes.Get);

            lessonToBeDeleted.ItemsSource = itemsSource.Count > 0 ? itemsSource : null;
        }

        private async void calculateYano_Clicked(object sender, EventArgs e)
        {

            YanoResult.Text = Calculations.YANOCalculate(await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", semesters.SelectedIndex)
            }, DataAccessLayer.Processes.Get)).ToString("#.##");
        }

        // ProcessSpecifiedEntities islemi kod tekrarini gidermek lazim.
        private static List<Ders> GetCoursesOfSelectedSemester(int donemId)
        {
            return null;
        }

        // YANOCalculate metodunu button'la tetiklemek yerine her eklenen ve silinen derste otomatik yenileyelim.Metodun string dönüşünüde burda yazalım.
        private static string YanoCalculate(int donemId)
        {
            return string.Empty;
        }
    }
}