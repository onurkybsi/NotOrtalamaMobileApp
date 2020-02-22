using System.IO;
using NotOrtalamaMobileApp.Droid;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]
namespace NotOrtalamaMobileApp.Droid
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            string documentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            var path = Path.Combine(documentPath, "NotOrtalamaAppDb.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}