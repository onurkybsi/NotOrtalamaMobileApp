using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotOrtalamaMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YanoCalculationPage : ContentPage
    {
        public YanoCalculationPage()
        {
            InitializeComponent();
        }

        private async void addCourseToSemester_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddLesson());
        }
    }
}