using Autofac;
using NotOrtalamaMobileApp.DataAccessLayer.Logger;
using Xamarin.Forms;

namespace NotOrtalamaMobileApp.Dependency
{
    public static class DIContainer
    {
        private static IContainer _container;
        public static ILogger LoggerService{ get => _container.Resolve<ILogger>(); }
        public static void Initialize(Application app)
        {
            if (_container == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<UILogger>()
                    .As<ILogger>().SingleInstance()
                    .WithParameter(new TypedParameter(typeof(Application), app));
                _container = builder.Build();
            }
        }
    }
}
