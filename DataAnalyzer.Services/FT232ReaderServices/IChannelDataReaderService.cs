using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ReaderServices
{
    public interface IChannelDataReaderService
    {
        Task<byte[]> ReadDataAsync(uint bufferSize);
        Task<byte[]> ReadGeneratedMockDataAsync(uint bufferSize);
    }
}
