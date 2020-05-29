using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.DataAccessLayer.Management
{
    public partial class DbManagement : IDbManagement
    {
        private static DbManagement _dbManagement;
        private SQLiteAsyncConnection database;

        public delegate Task DelegateOfManipulation(Func<Task> callBack);
        public event DelegateOfManipulation EventOfManipulation;

        private DbManagement()
        {
            database = DependencyService.Get<ISQLiteDb>().GetConnection();
            EventOfManipulation += ExecuteAfterProcess;
        }
        public static DbManagement CreateAsSingleton() => _dbManagement ?? (_dbManagement = new DbManagement());
        async public Task<CreateTableResult> CreateTable<T>() where T : IEntity, new() => await database.CreateTableAsync<T>();
        async private Task ExecuteAfterProcess(Func<Task> callBack) => await callBack.Invoke();
        private static object[] BuildFilterExpression(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process)
        {
            var result = new object[2];

            string detectProcess = DetectProcess(process);

            string filterExpressions = string.Format("{0} FROM {1} WHERE", detectProcess, tableName);

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

            result[0] = filterExpressions;
            result[1] = args;

            return result;
        }
        private static string DetectProcess(IProcessThatEntitiesCanBeSpecified process)
        {
            string result = "SELECT *";

            if (process.ProcessType == typeof(DeleteProcess))
            {
                result = "DELETE";
            }

            return result;
        }

        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();
    }
}
