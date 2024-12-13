using DataAnalyzer.Common.Models;
using DataAnalyzer.Data.FT232Data;
using DataAnalyzer.Services;
using DataAnalyzer.Services.FT232ProcessorServices;
using DataAnalyzer.Services.FT232ReaderServices;
using DataAnalyzer.UI.Controls;
using DataAnalyzer.UI.ViewModels;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DataAnalyzer.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IChannelDataReaderService readerService,
                          ISimulatedDataReaderService simulatedDataReader,
                          IChannelDataProcessorService processorService)
        {
            InitializeComponent();

            DataContext = new MainViewModel(readerService, processorService, simulatedDataReader);
        }
    }
}    
