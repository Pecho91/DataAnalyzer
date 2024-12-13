using DataAnalyzer.Data.FT232Data;
using DataAnalyzer.Services.FT232ReaderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ReaderServices
{
    public class ChannelDataReaderService : IChannelDataReaderService
    {
        private readonly IFT232DataReader _ft232DataReader;

        public ChannelDataReaderService(IFT232DataReader fT232DataReader)
        {
            _ft232DataReader = fT232DataReader;
        }

        public Task<byte[]> ReadDataAsync(uint bufferSize)
        {
            return Task.Run(() => _ft232DataReader.ReadDataAsync(bufferSize));
        }      

       
    }
}
