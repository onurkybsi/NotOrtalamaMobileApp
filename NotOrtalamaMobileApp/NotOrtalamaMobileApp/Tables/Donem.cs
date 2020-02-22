using NotOrtalamaMobileApp.DataAccessLayer;
using SQLite;

namespace NotOrtalamaMobileApp.Tables
{
    [Table("DersTable")]
    public class Donem : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; }
        [Unique]
        public string DonemAdi
        {
            get => Id.ToString() + ". Dönem";
        }
    }
}
