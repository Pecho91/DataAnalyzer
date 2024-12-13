using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232SimulatorConsoleApp
{
    public class FileSimulator
    {
        private readonly string _outputFilePath;
        private readonly uint _bufferSize;

        public FileSimulator(string outputFilePath, uint bufferSize)
        {
            _outputFilePath = outputFilePath;
            _bufferSize = bufferSize;
        }

        public async Task StartSimulationAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Simulating file-based data output to {_outputFilePath}...");

            try
            {
                using var fileStream = new FileStream(_outputFilePath, FileMode.Create, FileAccess.Write);

                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] data = FT232Simulator.GenerateRandomData(_bufferSize);
                    await fileStream.WriteAsync(data, 0, data.Length, cancellationToken);
                    await fileStream.FlushAsync(cancellationToken);

                    Console.WriteLine($"Written {data.Length} bytes to file.");
                    await Task.Delay(1000, cancellationToken); // Simulate a 1-second interval
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Simulation cancelled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during simulation: {ex.Message}");
            }
        }
    }
}
