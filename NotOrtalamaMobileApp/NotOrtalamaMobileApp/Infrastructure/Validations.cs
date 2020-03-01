
using NotOrtalamaMobileApp.Tables;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.Infrastructure
{
    public static class Validations
    {
        public async static Task<bool> CheckUIDersInputThenInsert(string dersAdi, int donemId, string kredi, Picker letterGrade)
        {

            try
            {
                await App.dbManagement.InsertEntity<Ders>(new Ders
                {
                    DersAdi = string.IsNullOrWhiteSpace(dersAdi) ? throw new Exception() : dersAdi,
                    DonemId = donemId,
                    Kredi = Convert.ToInt32(kredi) > 0 ? Convert.ToInt32(kredi) : throw new Exception(),
                    HarfNotu = letterGrade.SelectedItem.ToString().Length > 2 ? throw new Exception() : letterGrade.SelectedItem.ToString()
                });
            }
            catch { return false; }

            return true;
        }

        public async static Task<bool> CheckUIDersInputThenUpdate(Ders ders, string kredi, Picker letterGrade)
        {

            try
            {

                foreach (Ders sameCourses in await App.dbManagement.GetSpecifiedEntities<Ders>(ders.DersAdi, "DersTable"))
                {
                    await App.dbManagement.DeleteEntity<Ders>(sameCourses.Id, "DersTable");

                    await App.dbManagement.InsertEntity<Ders>(new Ders
                    {
                        DersAdi = string.IsNullOrWhiteSpace(sameCourses.DersAdi) ? throw new Exception() : sameCourses.DersAdi,
                        DonemId = sameCourses.DonemId,
                        Kredi = Convert.ToInt32(kredi) > 0 ? Convert.ToInt32(kredi) : throw new Exception(),
                        HarfNotu = sameCourses.Id != ders.Id ? sameCourses.HarfNotu : letterGrade.SelectedItem.ToString().Length > 2 ? throw new Exception() : letterGrade.SelectedItem.ToString()
                    });
                }
            }
            catch { return false; }

            return true;
        }

    }
}
