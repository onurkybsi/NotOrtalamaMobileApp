
using NotOrtalamaMobileApp.Tables;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.Infrastructure
{
    public static class Validations
    {
        public static bool CheckUIDersInputForUpdate(string kredi, Picker letterGrade)
        {

            if (letterGrade.SelectedItem.ToString().Length > 2)
                return false;
            try
            {
                int newKredi = Convert.ToInt32(kredi) > 0 ? Convert.ToInt32(kredi) : throw new Exception();
            }
            catch { return false; }

            return true;
        }

        public async static Task<bool> CheckUIDersInputForInsert(string dersAdi, int donemId, string kredi, Picker letterGrade)
        {

            if (string.IsNullOrWhiteSpace(dersAdi))
                return false;
            if (letterGrade.SelectedItem == null || letterGrade.SelectedItem.ToString().Length > 2)
                return false;
            try
            {
                int newKredi = Convert.ToInt32(kredi) > 0 ? Convert.ToInt32(kredi) : throw new Exception();
            }
            catch { return false; }

            if (await App.dbManagement.GetEntity<Ders>(x => x.DecisiveName == dersAdi && x.DonemId == donemId) != null)
                return false;

            return true;
        }
    }
}
