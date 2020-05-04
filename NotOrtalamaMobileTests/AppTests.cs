using Moq;
using NotOrtalamaMobileApp;
using NotOrtalamaMobileApp.DataAccessLayer;
using NotOrtalamaMobileApp.Tables;
using SQLite;
using System;
using Xamarin.Forms;
using Xunit;

namespace NotOrtalamaMobileTests
{
    public class AppTests
    {
        private IDbManagement dbManagement;

        public AppTests()
        {
            dbManagement = DbManagement.CreateAsSingleton();
        }

        [Fact]
        public void Is_TableCreated()
        {
            
        }
    }
}
