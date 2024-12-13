using DataAnalyzer.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ProcessorServices
{
    public class ChannelDataProcessorService //: IChannelDataProcessorService
    {
        //public async Task<ChannelDataModel> ProcessedDataAsync(byte[] rawData, int channelId, string channelName)
        //{
        //    return await Task.Run(() =>
        //    {
        //        bool[] booleanLevels = new bool[rawData.Length * 8];

        //        for (int i = 0; i < rawData.Length; i++)
        //        {
        //            for (int bit = 0; bit < 8; bit++)
        //            {
        //                booleanLevels[i * 8 + bit] = (rawData[i] & (1 << bit)) != 0;
        //            }
        //        }

        //        Debug.WriteLine($"Processed {booleanLevels.Length} boolean levels");

        //        return new ChannelDataModel
        //        {
        //            ChannelId = channelId,  // Set appropriate channel ID dynamically
        //            ChannelName = channelName,  // Set appropriate channel name dynamically
        //            BooleanLevels = booleanLevels,
        //            Timestamp = DateTime.Now
        //        };
        //    });
        //}

        public async Task<List<ChannelDataModel>> ProcessedDataAsync(byte[] rawData)
        {
            return await Task.Run(() =>
            {
                // List to hold processed channel data for each byte.
                List<ChannelDataModel> processedData = new List<ChannelDataModel>();

                for (int byteIndex = 0; byteIndex < rawData.Length; byteIndex++)
                {
                    // Process each byte in the raw data
                    for (int bitIndex = 0; bitIndex < 8; bitIndex++)
                    {
                        bool bitValue = (rawData[byteIndex] & (1 << bitIndex)) != 0;

                        processedData.Add(new ChannelDataModel
                        {
                            ChannelId = bitIndex + 1,  // Channel IDs 1 to 8
                            ChannelName = $"Channel {bitIndex + 1}",  // Dynamic Channel Names
                            BooleanLevels = new bool[] { bitValue },
                            Timestamp = DateTime.Now
                        });
                    }
                }

                // Debug: Log the processed channel data length
                Debug.WriteLine($"Processed {processedData.Count} channel data models");

                return processedData;  // Return a list of processed channel data models
            });
        }
    }
}