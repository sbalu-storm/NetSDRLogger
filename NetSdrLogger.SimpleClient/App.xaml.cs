using Microsoft.Extensions.DependencyInjection;
using NetSdrLogger.Model.SignalSource;
using NetSdrLogger.SimpleClient.ViewModel;
using NetSdrLoggerConsole.Models;
using System.Windows;

namespace NetSdrLogger.SimpleClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            bool developmentConfiguration = true;
            if (developmentConfiguration)
            {
                serviceCollection
                    .AddSingleton<ISignalSource, RandomSignalSource>()
                    .AddSingleton<ICollectionService, CollectionService>()
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<MainWindow>();

            }
            else
            {
                serviceCollection
                    .AddSingleton<ISignalSource, TCPSignalSource>((ISignalSource) => new TCPSignalSource("127.0.0.1", 5000))
                    .AddSingleton<ICollectionService, CollectionService>()
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<MainWindow>();
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();

            mainWindow.Show();
            base.OnStartup(e);
        }


    }
}
