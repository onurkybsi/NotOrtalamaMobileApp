using SQLite;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public interface IEntity
    {
        [PrimaryKey,AutoIncrement]
        int Id { get; }
    }
}
