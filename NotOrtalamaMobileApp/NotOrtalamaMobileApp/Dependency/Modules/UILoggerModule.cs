using Autofac;
using NotOrtalamaMobileApp.DataAccessLayer.Logger;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.Dependency.Modules
{
    public class UILoggerModule : Module
    {
        private Application _app;
        public UILoggerModule(Application app)
        {
            _app = app;
        }
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(c => new UILogger(_app)).As<ILogger>();
        }
    }
}
