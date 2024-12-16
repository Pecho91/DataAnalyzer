
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAnalyzer.Data.FT232Data
{
    public class SimulatedFT232DataReader : ISimulatedFT232DataReader
    {
        private readonly string _filePath;

        public SimulatedFT232DataReader()
        {
            
        }

        public async Task<byte[]> ReadSimulatedDataAsync(uint bufferSize)
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"Simulated data file not found at {_filePath}. Ensure the FPGA simulator is running.");
            }

            byte[] buffer = new byte[bufferSize];

            // Async read from file
            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int bytesRead = 0;
                while (bytesRead < bufferSize)
                {
                    int read = await fs.ReadAsync(buffer, bytesRead, (int)(bufferSize - bytesRead));
                    if (read == 0) // End of file or insufficient data
                    {
                        throw new InvalidOperationException($"Failed to read the expected amount of data from the file.");
                    }
                    bytesRead += read;
                }
            }

            return buffer;
        }
    }
}
