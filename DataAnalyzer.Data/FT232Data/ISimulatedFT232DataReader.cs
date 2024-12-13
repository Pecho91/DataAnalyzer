using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Data.FT232Data
{
    public interface ISimulatedFT232DataReader
    {
        Task<byte[]> ReadSimulatedDataAsync(uint bufferSize);
    }
}
