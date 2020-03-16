using System.Collections.Generic;
using Xamarin.Forms;
using Xunit;

namespace NotOrtalamaMobileTests
{
    public class YanoCalclationPageTests
    {
        [Fact]
        public void ConstructorTest()
        {
            // Arrange

            Picker picker = new Picker();
            List<string> items = new List<string> { "Hiçbiri", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

            // Act

            picker.ItemsSource = new List<string> { "Hiçbiri", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };

            // Assert

            Assert.Equal(picker.ItemsSource, items);
        }

        [Fact]
        public void OnAppearingTest()
        {
            // Arrange

        }
    }
}
