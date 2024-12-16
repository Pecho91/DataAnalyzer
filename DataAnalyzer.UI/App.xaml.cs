using DataAnalyzer.Data.FT232Data;
using DataAnalyzer.Services.FT232ProcessorServices;
using DataAnalyzer.Services.FT232ReaderServices;
using DataAnalyzer.UI.ViewModels;
using DataAnalyzer.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Windows;

namespace DataAnalyzer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private IChannelDataReaderService readerService;
        private ISimulatedDataReaderService simulatedDataReader;
        private IChannelDataProcessorService processorService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Configure dependency injection
                var services = new ServiceCollection();
                ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();

                // Read configuration
                bool useSimulation = bool.Parse(ConfigurationManager.AppSettings["UseSimulation"] ?? "false");
                Debug.WriteLine($"Configuration: UseSimulation = {useSimulation}");

                //if (useSimulation)
                //{
                //    string filePath = FPGADataSimulator.SimulationFilePathProvider.GetSimulatedFilePath();
                //    Debug.WriteLine($"Generated file path: {filePath}");

                //    Debug.WriteLine("Simulation mode enabled. Checking for file...");
                //    const int maxWaitTime = 10000;
                //    const int pollInterval = 500;

                //    await WaitForFileAsync(filePath, maxWaitTime, pollInterval);
                //}

                // Resolve services and start the main window
                //var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                var mainWindow = new MainWindow(readerService, simulatedDataReader, processorService);
                Debug.WriteLine("MainWindow is being shown");
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during startup: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Shutdown();
            }
        }

        [SupportedOSPlatform("windows")]
        private void ConfigureServices(IServiceCollection services)
        {
            // Add data layer services
            services.AddScoped<ISimulatedFT232DataReader, SimulatedFT232DataReader>();
            //services.AddScoped<IFT232DataReader, FT232DataReader>();

            // Add services layer
            services.AddScoped<ISimulatedDataReaderService, SimulatedDataReaderService>();
           // services.AddScoped<IChannelDataProcessorService, ChannelDataProcessorService>();
            services.AddScoped<IChannelDataReaderService, ChannelDataReaderService>();

            bool useSimulation = bool.Parse(ConfigurationManager.AppSettings["UseSimulation"] ?? "false");

            if (useSimulation)
            {
                services.AddSingleton<ISimulatedFT232DataReader, SimulatedFT232DataReader>();
            }
            //else
            //{
            //    services.AddSingleton<IFT232DataReader>(provider =>
            //    {
            //        IntPtr pointer = GetFT232Handle(); 
            //        return new FT232DataReader(pointer);
            //    });
            //}

            // Add MainWindow
            services.AddTransient<MainWindow>();
        }

        private IntPtr GetFT232Handle()
        {
            IntPtr handle = IntPtr.Zero;

            if (handle == IntPtr.Zero)
            {
                Debug.WriteLine("Failed to obtain FT232 handle.");
                throw new InvalidOperationException("Unable to open FT232 device.");
            }

            return handle;
        }

        private async Task WaitForFileAsync(string filePath, int maxWaitTime, int pollInterval)
        {
            int elapsed = 0;
            while (!File.Exists(filePath) && elapsed < maxWaitTime)
            {
                Debug.WriteLine($"Waiting for file... Elapsed: {elapsed}ms");
                await Task.Delay(pollInterval); 
                elapsed += pollInterval;
            }

            if (!File.Exists(filePath))
            {
                Debug.WriteLine("FPGA simulator data file not found. Exiting...");
                Shutdown();
            }
            else
            {
                Debug.WriteLine("File found!");
            }
        }
    }
}
