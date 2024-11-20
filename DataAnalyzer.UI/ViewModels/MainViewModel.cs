using DataAnalyzer.Common.Models;
using DataAnalyzer.Services;
using DataAnalyzer.Services.FT232ProcessorServices;
using DataAnalyzer.Services.FT232ReaderServices;
using DataAnalyzer.UI.RelayCommands;
using DataAnalyzer.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataAnalyzer.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IChannelDataReaderService _dataReaderService;
        private readonly IChannelDataProcessorService _dataProcessorService;
        public ObservableCollection<ChannelViewModel> Channels { get; }

        public MainViewModel(IChannelDataReaderService dataReaderService, IChannelDataProcessorService dataProcessorService)
        {
            _dataReaderService = dataReaderService;
            _dataProcessorService = dataProcessorService;

            Channels = new ObservableCollection<ChannelViewModel>();
            AddChannels(8);
        }

        private void AddChannels(int numberOfChannels)
        {

            for (int i = 0; i < numberOfChannels; i++)
            {
                // Add a new ChannelViewModel to the Channels collection
                var channelName = $"Channel {i + 1}";
                var channelViewModel = new ChannelViewModel(_dataReaderService, _dataProcessorService, i, channelName);
                Channels.Add(channelViewModel);
            }

        }

    }
}

