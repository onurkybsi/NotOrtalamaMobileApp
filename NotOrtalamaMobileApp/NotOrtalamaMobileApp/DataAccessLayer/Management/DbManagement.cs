using NotOrtalamaMobileApp.DataAccessLayer.Process;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private static object[] BuildSQLCommandToBeExecute(string tableName, List<KeyValuePair<string, object>> filter, IProcessThatEntitiesCanBeSpecified process, List<KeyValuePair<string, object>> newValues = null)
        {
            // Detect inconsistency on update
            #region
            if (process.ProcessType == typeof(UpdateProcess) && newValues == null)
            {
                throw new InvalidOperationException(string.Format("If process type is {0}, updateExpression cannot be null", typeof(UpdateProcess).ToString()));
            }
            if (process.ProcessType != typeof(UpdateProcess) && newValues != null)
            {
                throw new InvalidOperationException(string.Format("If updateExpressions is not null, process type is can only be {0}", typeof(UpdateProcess).ToString()));
            }
            #endregion 

            var result = new object[2];

            string processCommand = BuildProcessCommand(process, tableName);

            if (process.ProcessType == typeof(UpdateProcess))
            {
                foreach (var newValue in newValues)
                {
                    processCommand += string.Format("{0} = '{1}', ", newValue.Key, newValue.Value);
                }

                processCommand = processCommand.Remove(processCommand.Length - 2, 2);
            }

            string filterExpressions = string.Format("{0} WHERE", processCommand);

            object[] args = new object[filter.Count];
            int i = 0;

            // WHERE conditions adding;
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
        private static string BuildProcessCommand(IProcessThatEntitiesCanBeSpecified process, string tableName)
        {
            string result = string.Format("SELECT * FROM {0}", tableName);

            if (process.ProcessType == typeof(DeleteProcess))
                result = string.Format("DELETE FROM {0}", tableName);
            else if (process.ProcessType == typeof(UpdateProcess))
                result = string.Format("UPDATE {0} SET ", tableName);

            return result;
        }
        async public Task DbSil<T>() where T : IEntity, new() => await database.DropTableAsync<T>();
    }
}
