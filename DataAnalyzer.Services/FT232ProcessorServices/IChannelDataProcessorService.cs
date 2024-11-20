using DataAnalyzer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ProcessorServices
{
    public interface IChannelDataProcessorService
    {
        Task<ChannelDataModel> ProcessedDataAsync(byte[] rawData, int channelId, string channelName);
    }
}
