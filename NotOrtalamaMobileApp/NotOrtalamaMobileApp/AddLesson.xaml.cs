using NotOrtalamaMobileApp.Infrastructure;
using NotOrtalamaMobileApp.Tables;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLesson : ContentPage
    {
        private int _donemId;

        public AddLesson(int donemId)
        {
            InitializeComponent();

            letterGrade.ItemsSource = new List<string>
            {
                "AA", "BA", "BB", "CB", "CC", "DC", "DD", "FD", "FF", "F0"
            };

            _donemId = donemId;
        }

        // On Appering
        protected async override void OnAppearing()
        {
            courseToBeUpdated.ItemsSource = await App.dbManagement.GetAllEntities<Ders>() as List<Ders>;
        }

        // Select course to be update
        private void courseToBeUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as Picker).SelectedIndex != -1)
            {
                courseNameToBeAdded.IsVisible = false;
                courseNameToBeAddedLabel.IsVisible = false;

                // Tekrar alinan ders var kredi girisi engelle
                if (((sender as Picker).SelectedItem as Ders).DonemId != _donemId)
                {
                    courseCreditLabel.IsVisible = false;
                    courseCredit.IsVisible = false;
                }
                // Düzenlenen ders kredi girisini yaz
                else
                {
                    courseCredit.Text = (courseToBeUpdated.SelectedItem as Ders).Kredi.ToString();
                }

                letterGrade.SelectedItem = (courseToBeUpdated.SelectedItem as Ders).HarfNotu;
            }
        }

        // Add course
        private async void addOrUpdateCourse_Clicked(object sender, EventArgs e)
        {
            // Update processes
            if (courseToBeUpdated.SelectedIndex != -1)
            {
                var updatedCourse = (courseToBeUpdated.SelectedItem as Ders);

                // Ayni dersi 2. kez almis
                if (_donemId != updatedCourse.DonemId)
                {
                    if(await Validations.CheckUIDersInputThenInsert(updatedCourse.DersAdi, _donemId, updatedCourse.Kredi.ToString(), letterGrade))
                    {
                        await DisplayAlert("Tekrar ders kaydı", "Ders eklendi !", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                        await DisplayAlert("Hata", "Harf notunu girin !", "OK");
                }
                // Guncelleme yapiyor
                else
                {
                    if(await Validations.CheckUIDersInputThenUpdate(updatedCourse, courseCredit.Text, letterGrade))
                    {
                        await DisplayAlert("Güncelleme", "Ders güncellendi !", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                        await DisplayAlert("Hata", "Harf notunu ve ders kredisini girin !", "OK");
                }
            }
            // Insert process
            else
            {
                // AYNI ISIMLI DERSI INSERT ETMESINE IZIN VERME !!
                if ((await App.dbManagement.GetEntity<Ders>(x => x.DersAdi = courseNameToBeAdded.Text) == null) && await Validations.CheckUIDersInputThenInsert(courseNameToBeAdded.Text, _donemId, courseCredit.Text, letterGrade))
                {
                    await DisplayAlert("Yeni ders kaydı", "Ders eklendi !", "OK");
                    await Navigation.PopAsync();
                }
                else
                    await DisplayAlert("Hata", "Ders adını,ders kredisini ve harf notunu doğru girin !", "OK");
            }
        }
    }
}