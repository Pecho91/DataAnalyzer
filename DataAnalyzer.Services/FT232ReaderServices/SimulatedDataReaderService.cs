using DataAnalyzer.Data.FT232Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Services.FT232ReaderServices
{
    public class SimulatedDataReaderService : ISimulatedDataReaderService
    {
        private readonly ISimulatedFT232DataReader _simulationDataReader;

        public SimulatedDataReaderService(ISimulatedFT232DataReader simulationDataReader)
        {
            _simulationDataReader = simulationDataReader;
        }
        public Task<byte[]> ReadSimulatedDataAsync(uint bufferSize)
        {
            return Task.Run(() => _simulationDataReader.ReadSimulatedDataAsync(bufferSize));
        }

    } 
}
