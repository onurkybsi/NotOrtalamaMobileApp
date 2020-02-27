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

            semesters.ItemsSource = new List<string>
            {
                "Hiçbiri","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16"
            };
        }

        protected async override void OnAppearing()
        {
            lessonToBeDeleted.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
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

                    lessonToBeDeleted.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
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
                int donemId = (semesters.SelectedItem.ToString() == "Hiçbiri") ? 0 : Convert.ToInt32(semesters.SelectedItem);

                await Navigation.PushAsync(new AddLesson(donemId));
            }
            else
            {
                await DisplayAlert("Hata", "Lütfen hesaplanacak dönemi seçin !", "OK");
            }
        }

        // Insert Donem to repository.
        private async void insertDonemToRepo_Clicked(object sender, EventArgs e)
        {
            if (lessonToBeDeleted.Items.Count <= 0)
                await DisplayAlert("Hata", "Kaydedilecek dönem için dersler girin !", "OK");
            else
            {
                await App.dbManagement.InsertEntity<Donem>(new Donem
                {
                    Id = Convert.ToInt32(semesters.SelectedItem)
                });
            }
        }

        // Enable or disable "insertDonemToRepo" button.
        private void semesters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as Picker).SelectedIndex == 0 || (sender as Picker).SelectedIndex == -1) insertDonemToRepo.IsEnabled = false;
            else
                insertDonemToRepo.IsEnabled = true;
        }
    }
}