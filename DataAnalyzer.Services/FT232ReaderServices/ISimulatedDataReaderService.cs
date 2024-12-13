using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ReaderServices
{
    public interface ISimulatedDataReaderService
    {
        Task<byte[]> ReadSimulatedDataAsync(uint bufferSize);
    }
}
