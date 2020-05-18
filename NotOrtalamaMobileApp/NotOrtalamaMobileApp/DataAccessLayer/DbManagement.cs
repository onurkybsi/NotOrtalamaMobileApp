using NotOrtalamaMobileApp.DataAccessLayer.Logger;
using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer
{
    public class DbManagement : IDbManagement
    {
        private static DbManagement _dbManagement;
        private SQLiteAsyncConnection database;

        private static Page CurrentPage { get; set; }

        public delegate void DelegateOfManipulation(Processes processes);
        public event DelegateOfManipulation EventOfManipulation;

        private DbManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
            EventOfManipulation += ExecuteAfterManipulation;
        }
        public static DbManagement CreateAsSingleton(Application app)
        {
            app.PageAppearing += SetCurrentPage;
            return _dbManagement ?? (_dbManagement = new DbManagement());
        }

        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();
        async public Task<List<T>> ProcessSpecifiedEntities<T>(string tableName, List<KeyValuePair<string, object>> filter, Processes process) where T : IEntity, new()
        {

            string filterExpressions = process == Processes.Get
                ? "SELECT * FROM " + tableName + " WHERE"
                : "DELETE FROM " + tableName + " WHERE";

            object[] args = new object[filter.Count];
            int i = 0;

            foreach (var filterExpression in filter)
            {
                if (filterExpression.Value.GetType() != typeof(int) && filterExpression.Value.GetType() != typeof(string))
                    throw new ArgumentException("Type " + filterExpression.Value.GetType() + " is not handled.");

                if (!filterExpressions.Contains(filterExpression.Key))
                {
                    filterExpressions += " (";

                    foreach (var otherFilterExpression in filter)
                    {

                        if (filterExpression.Key == otherFilterExpression.Key && filterExpression.Value != otherFilterExpression.Value)
                        {
                            filterExpressions += string.Format(" {0} = ? OR", otherFilterExpression.Key);

                            args[i] = otherFilterExpression.Value;
                            i++;
                        }
                    }

                    filterExpressions = filterExpressions.Insert(filterExpressions.Length, string.Format(" {0} = ?) AND", filterExpression.Key));
                    args[i] = filterExpression.Value;
                    i++;
                }
            }

            filterExpressions = filterExpressions.Remove(filterExpressions.Length - 4, 4);

            //if (process == Processes.Delete)
            //    EventOfManipulation();

            return await database.QueryAsync<T>(filterExpressions, args);
        }
        async public Task<IEnumerable<T>> GetAllEntities<T>() where T : IEntity, new() => await database.Table<T>().ToListAsync();
        async public Task<IEntity> GetEntity<T>(Expression<Func<T, bool>> predicate) where T : IEntity, new()
        {

            try
            {
                return await database.GetAsync<T>(predicate);
            }
            catch { return null; }
        }
        async public Task InsertEntity<T>(IEntity entity) where T : IEntity, new()
        {
            await database.InsertAsync(entity);
            EventOfManipulation(Processes.Insert);
        }
        async public Task DeleteEntity<T>(int Id, string tableName) where T : IEntity, new()
        {
            await database.ExecuteScalarAsync<int>("DELETE FROM " + tableName + " WHERE _id = ?", Id);
            EventOfManipulation(Processes.Delete);
        }
        private async void ExecuteAfterManipulation(Processes processes)
        {
            //if(processes == Processes.Insert)
            //{
            //    await CurrentPage.DisplayAlert("Automatic information display test", "Insert", "OK");
            //}
            //else if(processes == Processes.Delete)
            //{
            //    await CurrentPage.DisplayAlert("Automatic information display test", "Delete", "OK");
            //}

            ILogger logger = new UILogger(CurrentPage);
            IProcess process = new InsertProcess
            {
                Entity = null,
                EntityId = 0,
                TableName = "DersTable"
            };

            logger.Log(process);
        }
        private static void SetCurrentPage(object sender, Page e) => CurrentPage = e;
    }
}
