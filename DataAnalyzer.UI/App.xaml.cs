using DataAnalyzer.Data.FT232Data;
using DataAnalyzer.Services.FT232ProcessorServices;
using DataAnalyzer.Services.FT232ReaderServices;
using DataAnalyzer.UI.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace DataAnalyzer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Obtain a handle to the FT232 device (you need to implement this)
            IntPtr ftHandle = GetFT232Handle();

            // Create the FT232 data reader instance with the valid handle
            var ft232DataReader = new FT232DataReader(ftHandle);
            var channelDataReaderService = new ChannelDataReaderService(ft232DataReader);
            var channelDataProcessorService = new ChannelDataProcessorService();

            // Create and show the MainWindow with the data reader service
            var mainWindow = new MainWindow(channelDataReaderService, channelDataProcessorService);
            mainWindow.Show();
        }

        private IntPtr GetFT232Handle()
        {
            // Logic to get the handle to the FT232 device.
            // This is typically done using FTDI D2XX functions to open a device.
            // Here's a placeholder implementation:

            // Example: 
            IntPtr handle = IntPtr.Zero; // Replace with actual logic to get the handle

            // You would normally use FT_Open or similar function here to obtain the handle.
            // Ensure to check for errors and validate the handle.

            return handle;
        }
    }
}
