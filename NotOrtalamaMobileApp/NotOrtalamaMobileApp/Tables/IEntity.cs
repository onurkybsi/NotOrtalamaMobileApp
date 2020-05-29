
namespace NotOrtalamaMobileApp.Tables
{
    public interface IEntity
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        int Id { get; }
        string DecisiveName { get; set; }
    }
}
