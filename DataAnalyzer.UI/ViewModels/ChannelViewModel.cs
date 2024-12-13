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
using System.Windows.Controls;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace DataAnalyzer.UI.ViewModels
{
    public class ChannelViewModel : ViewModelBase
    {
        // private readonly ZoomManager _zoomManager;

        private readonly IChannelDataReaderService _readerService;
        private readonly IChannelDataProcessorService _processorService;
        private readonly ISimulatedDataReaderService _simulatedReaderService;

        private ChannelDataModel _channelData;
        public ChannelDataModel ChannelData
        {
            get => _channelData;
            set
            {
                _channelData = value;
                OnPropertyChanged(nameof(ChannelData));
                OnPropertyChanged(nameof(ChannelDataAsBinary));               
            }
        }

        public string ChannelDataAsBinary
        {
            get
            {
                if (ChannelData?.BooleanLevels == null) return string.Empty;
                return string.Concat(ChannelData.BooleanLevels.Select(b => b ? "1" : "0"));
            }
        }

        public int ChannelId { get; set; }
        public string ChannelName { get; set; }

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

        [SupportedOSPlatform("windows")]
        public ChannelViewModel(IChannelDataReaderService readerService, ISimulatedDataReaderService simulatedReaderService,
                                IChannelDataProcessorService processorService, int channelId, string channelName)
        {
            _readerService = readerService;
            _processorService = processorService;
            _simulatedReaderService = simulatedReaderService;

            _channelData = new ChannelDataModel
            {
                ChannelId = channelId,
                ChannelName = channelName,
                BooleanLevels = Array.Empty<bool>(),
                Timestamp = DateTime.Now,
            };

            //UpdateDataCommand = new RelayCommand(async () => await UpdateChannelDataAsync());
            UpdateDataCommand = new RelayCommand(async () => await UpdateSimulatedChannelDataAsync());

            StartAutoUpdate();
          
        }

        private async Task UpdateSimulatedChannelDataAsync()
        {
            try
            {
                Debug.WriteLine("Reading simulated data...");

                var rawData = await _simulatedReaderService.ReadSimulatedDataAsync(1024);
                Debug.WriteLine($"Raw data length: {rawData.Length}");

                var processedData = await _processorService.ProcessedDataAsync(rawData, _channelData.ChannelId, _channelData.ChannelName);

                Debug.WriteLine($"Processed data: {processedData.BooleanLevels.Length} items");

                ChannelData = processedData;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating channel data: {ex.Message}");
            }
        }

        private async Task UpdateChannelDataAsync()
        {
            try
            {

                var rawData = await _readerService.ReadDataAsync(1024); // Example buffer size, adjust as needed

                var processedData = await _processorService.ProcessedDataAsync(rawData, _channelData.ChannelId, _channelData.ChannelName);

                ChannelData = processedData;
            }
            catch (Exception ex)
            {
                // Handle exceptions (like connection issues or invalid data)
                Debug.WriteLine($"Error updating channel data: {ex.Message}");
            }
        }

        private async Task StartAutoUpdate()
        {

            //await UpdateChannelDataAsync();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1000) };

            timer.Tick += async (s, e) =>
            {
                await UpdateSimulatedChannelDataAsync();
                timer.Stop();
            };

            timer.Start();
        }
    }
}