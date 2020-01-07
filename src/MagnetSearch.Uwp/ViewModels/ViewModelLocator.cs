using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using MagnetSearch.Configuration;
using MagnetSearch.Services;
using MagnetSearch.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MagnetSearch.Uwp.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            var autofacServiceLocator = new AutofacServiceLocator(ConfigureAutofacContainer());
            ServiceLocator.SetLocatorProvider(() => autofacServiceLocator);
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        private static IContainer ConfigureAutofacContainer()
        {
            var containerBuilder = new ContainerBuilder();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddHttpClient();
            containerBuilder.Populate(serviceCollection);

            containerBuilder.RegisterType<AppSettings>().As<IAppSettings>();

            containerBuilder.RegisterType<BtsowService>().As<IMagnetService>();
            containerBuilder.RegisterType<SobtService>().As<IMagnetService>();
            containerBuilder.RegisterType<SukebeiNyaaService>().As<IMagnetService>();
            containerBuilder.RegisterType<TorrentKittyService>().As<IMagnetService>();

            containerBuilder.RegisterType<AggregateService>().As<IAggregateService>();

            containerBuilder.RegisterType<MainViewModel>();

            return containerBuilder.Build();
        }
    }
}
