using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using DataAnalyzer.Common.Models;
using DataAnalyzer.UI.RelayCommands;
using System.ComponentModel;
using System.Windows.Input;
using DataAnalyzer.UI.Transformations;
using DataAnalyzer.UI.Plottings;
using DataAnalyzer.Services.FT232ProcessorServices;
using DataAnalyzer.Services.FT232ReaderServices;
using System.Windows.Threading;

namespace DataAnalyzer.UI.ViewModels
{
    public class ChannelViewModel : ViewModelBase
    {
        // private readonly ZoomManager _zoomManager;

        private readonly IChannelDataReaderService _readerService;
        private readonly IChannelDataProcessorService _processorService;

        private ChannelDataModel _channelData;
        public ChannelDataModel ChannelData
        {
            get => _channelData;
            set
            {
                _channelData = value;
                OnPropertyChanged(nameof(ChannelData)); // Notify the UI when data changes
            }
        }

        public int ChannelId { get; }
        public string ChannelName { get; }

        public ICommand UpdateDataCommand { get; }

        //public ICommand ZoomInCommand { get; }
        //public ICommand ZoomOutCommand { get; }

        // Optional: You may want to track the zoom level or scale
        //private double _zoomLevel;
        //public double ZoomLevel
        //{
        //    get => _zoomLevel;
        //    set => SetProperty(ref _zoomLevel, value);
        //}

        public ChannelViewModel(IChannelDataReaderService readerService, IChannelDataProcessorService processorService, int channelId, string channelName)
        {
            _readerService = readerService;
            _processorService = processorService;

            _channelData = new ChannelDataModel
            {
                ChannelId = channelId,
                ChannelName = channelName,
                BooleanLevels = new bool[0],
                Timestamp = DateTime.Now,
            };

            UpdateDataCommand = new RelayCommand(async () => await UpdateChannelDataAsync());

            StartAutoUpdate();

            //_channelData = new ObservableCollection<ChannelDataModel>();
            //_zoomManager = new ZoomManager();  

            //ZoomInCommand = new RelayCommand(ZoomIn);
            //ZoomOutCommand = new RelayCommand(ZoomOut);
            //UpdateChannelData(_channelData);
        }

        private async Task UpdateChannelDataAsync()
        {
            try
            {
                // Fetch raw data from the FT232 reader service
                var rawData = await _readerService.ReadGeneratedMockDataAsync(1024); // Example buffer size, adjust as needed

                // Process the raw data into boolean levels for this channel
                var processedData = await _processorService.ProcessedDataAsync(rawData, _channelData.ChannelId, _channelData.ChannelName);

                // Update the ChannelData model with the processed data
                ChannelData = processedData;
            }
            catch (Exception ex)
            {
                // Handle exceptions (like connection issues or invalid data)
                Console.WriteLine($"Error updating channel data: {ex.Message}");
            }

           

            //public void ZoomIn()
            //{
            //    _zoomManager.ZoomIn();
            //    UpdateZoomLevel();
            //}

            //public void ZoomOut()
            //{
            //    _zoomManager.ZoomOut();
            //    UpdateZoomLevel();
            //}

            //public void UpdateZoomLevel()
            //{
            //    // Here, you can manage how the zoom affects the ZoomLevel property
            //    ZoomLevel = _zoomManager.GetCurrentZoomLevel(); // Assume ZoomManager exposes a method to get the current zoom level
            //}
        }
        private void StartAutoUpdate()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += async (s, e) => await UpdateChannelDataAsync();
            timer.Start();
        }
    }
}