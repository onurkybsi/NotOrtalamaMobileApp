
using SQLite;

namespace NotOrtalamaMobileApp.Tables
{
    [SQLite.Table("DersTable")]
    public class Ders : IEntity
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        
        [Unique]
        public string DersAdi { get; set; }
        public int Kredi { get; set; }
        public string HarfNotu { get; set; }
        public int DonemId { get; set; }
        
        [Ignore]
        public double DersEtki
        {
            get
            {
                switch (HarfNotu)
                {
                    case "F0":
                    case "FF":
                        return 0.0;
                    case "FD":
                        return ((0.5) * Kredi);
                    case "DD":
                        return ((1.0) * Kredi);
                    case "DC":
                        return ((1.5) * Kredi);
                    case "CC":
                        return ((2.0) * Kredi);
                    case "CB":
                        return ((2.5) * Kredi);
                    case "BB":
                        return ((3.0) * Kredi);
                    case "BA":
                        return ((3.5) * Kredi);
                    case "AA":
                        return ((4.0) * Kredi);
                    default:
                        return 0.0;
                }
            }
        }
    }
}
