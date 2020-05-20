using Autofac;
using NotOrtalamaMobileApp.DataAccessLayer.Logger;

namespace NotOrtalamaMobileApp.Dependency
{
    public static class DIContainer
    {
        private static IContainer _container;
        public static ILogger LoggerService { get { return _container.Resolve<ILogger>(); } }
        public static void Initialize()
        {
            if (_container == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<UILogger>().As<ILogger>().SingleInstance();
                _container = builder.Build();
            }
        }
    }
}
