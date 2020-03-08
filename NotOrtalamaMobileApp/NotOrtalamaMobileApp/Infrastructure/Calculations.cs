using NotOrtalamaMobileApp.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotOrtalamaMobileApp.Infrastructure
{
    public static class Calculations
    {
        public static double YANOCalculate(List<Ders> derseler)
        {
            double grades = 0.0;
            int totalKredi = 0;

            foreach(Ders ders in derseler)
            {
                grades += ders.DersEtki;
                totalKredi += ders.Kredi;
            }

            return grades / totalKredi;
        }
    }
}
