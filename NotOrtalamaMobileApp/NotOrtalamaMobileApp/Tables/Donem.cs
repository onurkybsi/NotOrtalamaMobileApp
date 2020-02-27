using SQLite;

namespace NotOrtalamaMobileApp.Tables
{
    [Table("DonemTable")]
    public class Donem : IEntity
    {
        [PrimaryKey, Column("_id")]
        public int Id { get; set; }

        [Unique]
        public string DonemAdi
        {
            get => Id.ToString() + ". Dönem";
        }
    }
}
