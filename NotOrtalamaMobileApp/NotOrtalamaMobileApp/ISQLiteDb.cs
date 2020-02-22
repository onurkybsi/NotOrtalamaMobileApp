using SQLite;

namespace NotOrtalamaMobileApp
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
