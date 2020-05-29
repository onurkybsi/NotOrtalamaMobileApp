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

        public static double AGNOCalculate(List<Ders> dersler)
        {
            double grades = 0.0;
            int totalKredi = 0;
            bool IsThatLastOne = true;

            foreach (Ders ders1 in dersler)
            {
                foreach (Ders ders2 in dersler)
                {
                    IsThatLastOne = true;

                    if (ders1.DecisiveName == ders2.DecisiveName && ders1.DonemId < ders2.DonemId)
                    {
                        IsThatLastOne = false;
                        break;
                    }
                }

                if (IsThatLastOne)
                {
                    grades += ders1.DersEtki;
                    totalKredi += ders1.Kredi;
                }
            }

            return totalKredi == 0 ? -1 : (grades / totalKredi);
        }
    }
}
