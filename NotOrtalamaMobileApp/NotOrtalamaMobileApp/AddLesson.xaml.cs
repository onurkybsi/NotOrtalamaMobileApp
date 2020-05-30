using NotOrtalamaMobileApp.DataAccessLayer.Process;
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

        protected async override void OnAppearing()
        {
            List<KeyValuePair<string, object>> filters = new List<KeyValuePair<string, object>>();

            for (int i = _donemId; i > 0; i--) 
            {
                filters.Add(new KeyValuePair <string, object>("DonemId", i));
            }

            courseToBeUpdated.ItemsSource = await App.dbManagement.GetSpecifiedEntities<Ders>("DersTable", filters);
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
                    courseCreditLabel.IsVisible = true;
                    courseCredit.IsVisible = true;

                    courseCredit.Text = (courseToBeUpdated.SelectedItem as Ders).Kredi.ToString();
                }

                letterGrade.SelectedItem = (courseToBeUpdated.SelectedItem as Ders).HarfNotu;
            }
        }

        // Add or update course
        private async void addOrUpdateCourse_Clicked(object sender, EventArgs e)
        {
            // Update processes
            if (courseToBeUpdated.SelectedIndex != -1)
            {
                var updatedCourse = (courseToBeUpdated.SelectedItem as Ders);

                // Ayni dersi 2. kez almis
                if (_donemId != updatedCourse.DonemId)
                {
                    if (await Validations.CheckUIDersInputForInsert(updatedCourse.DecisiveName, _donemId, updatedCourse.Kredi.ToString(), letterGrade))
                    {
                        Ders ders = new Ders
                        {
                            DecisiveName = updatedCourse.DecisiveName.Trim(),
                            DonemId = _donemId,
                            Kredi = updatedCourse.Kredi,
                            HarfNotu = letterGrade.SelectedItem.ToString()
                        };

                        await App.dbManagement.InsertEntity<Ders>(ders, "DersTable");

                        await DisplayAlert("Tekrar ders kaydı", "Ders eklendi !", "OK");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Bu ders zaten alınmış !", "OK");
                    }
                }
                // Guncelleme yapiyor
                else
                {
                    if (Validations.CheckUIDersInputForUpdate(courseCredit.Text, letterGrade))
                    {
                        foreach (Ders sameCourses in await App.dbManagement.GetSpecifiedEntities<Ders>("DersTable", new List<KeyValuePair<string, object>>
                        {
                           new KeyValuePair<string, object>("DersAdi", updatedCourse.DecisiveName)
                        }))
                        {
                            //await App.dbManagement.DeleteEntity<Ders>(sameCourses.Id, "DersTable");

                            //Ders ders = new Ders
                            //{
                            //    DecisiveName = sameCourses.DecisiveName,
                            //    DonemId = sameCourses.DonemId,
                            //    Kredi = Convert.ToInt32(courseCredit.Text),
                            //    HarfNotu = sameCourses.Id != updatedCourse.Id ? sameCourses.HarfNotu : letterGrade.SelectedItem.ToString()
                            //};

                            string harfNotu = (sameCourses.Id != updatedCourse.Id) ? sameCourses.HarfNotu : letterGrade.SelectedItem.ToString();

                            //await App.dbManagement.InsertEntity<Ders>(ders, "DersTable");

                            await App.dbManagement.UpdateEntity<Ders>("DersTable", new List<KeyValuePair<string, object>>
                            {
                                new KeyValuePair<string, object>("_id", sameCourses.Id)
                            }, x => x.Kredi == Convert.ToInt32(courseCredit.Text) && x.HarfNotu == harfNotu);
                        }
                        
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

                if (await Validations.CheckUIDersInputForInsert(courseNameToBeAdded.Text.Trim(), _donemId, courseCredit.Text, letterGrade))
                {
                    Ders toBeInserted = new Ders
                    {
                        DecisiveName = courseNameToBeAdded.Text.Trim(),
                        DonemId = _donemId,
                        Kredi = Convert.ToInt32(courseCredit.Text),
                        HarfNotu = letterGrade.SelectedItem.ToString()
                    };

                    var callBack = App.logger.Log(new InsertProcess
                    {
                        Entity = toBeInserted,
                        EntityId = toBeInserted.Id,
                        TableName = "DersTable"
                    });

                    await App.dbManagement.InsertEntity<Ders>(toBeInserted, "DersTable", callBack);

                    await Navigation.PopAsync();
                }
                else
                    await DisplayAlert("Hata", "Ders adını,ders kredisini ve harf notunu doğru girin yada aynı dersi kaydetmeye çalışmayın !", "OK");
            }
        }
    }
}