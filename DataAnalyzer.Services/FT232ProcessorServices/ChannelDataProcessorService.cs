using DataAnalyzer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ProcessorServices
{
    public class ChannelDataProcessorService : IChannelDataProcessorService
    {
        public async Task<ChannelDataModel> ProcessedDataAsync(byte[] rawData, int channelId, string channelName)
        {
            return await Task.Run(() =>
            {
                bool[] booleanLevels = new bool[rawData.Length * 8];

                // Process the raw byte data into boolean levels
                for (int i = 0; i < rawData.Length; i++)
                {
                    for (int bit = 0; bit < 8; bit++)
                    {
                        booleanLevels[i * 8 + bit] = (rawData[i] & (1 << bit)) != 0;
                    }
                }

                // Return processed data model with dynamic ChannelId and ChannelName
                return new ChannelDataModel
                {
                    ChannelId = channelId,  // Set appropriate channel ID dynamically
                    ChannelName = channelName,  // Set appropriate channel name dynamically
                    BooleanLevels = booleanLevels,
                    Timestamp = DateTime.Now
                };
            });
        } 
    }
}