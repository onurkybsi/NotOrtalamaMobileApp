using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Infrastructure;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                lessonToBeDeleted.ItemsSource = await GetCoursesOfSelectedSemester(semesters.SelectedIndex);
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

                    var callBack = App.logger.Log(new DeleteProcess
                    {
                        EntityId = toBeDeleted,
                        TableName = "DersTable"
                    });

                    await App.dbManagement.DeleteEntity<Ders>(toBeDeleted, "DersTable", callBack);

                    lessonToBeDeleted.ItemsSource = await GetCoursesOfSelectedSemester(semesters.SelectedIndex);
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

            var itemsSource = await GetCoursesOfSelectedSemester((sender as Picker).SelectedIndex);

            lessonToBeDeleted.ItemsSource = itemsSource.Count > 0 ? itemsSource : null;
        }
        
        private async void calculateYano_Clicked(object sender, EventArgs e)
        {

            YanoResult.Text = Calculations.YANOCalculate(await GetCoursesOfSelectedSemester(semesters.SelectedIndex)).ToString("#.##");
        }

        private static async Task<List<Ders>> GetCoursesOfSelectedSemester(int donemId)
        {
            return await App.dbManagement.ProcessSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("DonemId", donemId)
            }, DataAccessLayer.Processes.Get);
        }
    }
}