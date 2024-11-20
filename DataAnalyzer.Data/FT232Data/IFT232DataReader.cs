using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Data.FT232Data
{
    public interface IFT232DataReader
    {
        byte[] ReadDataFromFT232(uint bufferSize);
        byte[] GenerateMockDataFromFT232(uint bufferSize);
    }
}
