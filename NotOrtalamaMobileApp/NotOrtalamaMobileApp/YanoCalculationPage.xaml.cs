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
            //insertDonemToRepo.IsEnabled = semesters.SelectedIndex > 0;

            lessonToBeDeleted.ItemsSource = await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", semesters.SelectedIndex)
            }, DataAccessLayer.Processes.Get);
        }

        // Delete course.
        private async void deleteLesson_Clicked(object sender, EventArgs e)
        {
            if (!Object.Equals(lessonToBeDeleted.SelectedItem, null))
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
                int donemId = (semesters.SelectedItem.ToString() == "Hiçbiri") ? 0 : semesters.SelectedIndex;

                await Navigation.PushAsync(new AddLesson(donemId));
            }
            else
            {
                await DisplayAlert("Hata", "Lütfen hesaplanacak dönemi seçin !", "OK");
            }
        }

        // Enable or disable "insertDonemToRepo" button.
        private async void semesters_SelectedIndexChanged(object sender, EventArgs e)
        {
            //insertDonemToRepo.IsEnabled = (sender as Picker).SelectedIndex > 0;

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
    }
}