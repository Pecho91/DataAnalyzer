using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232SimulatorConsoleApp
{
    public class EventSimulator
    {
        private readonly uint _bufferSize;

        public EventSimulator(uint bufferSize)
        {
            _bufferSize = bufferSize;
        }

        public event EventHandler<byte[]> DataGenerated;

        public async Task StartSimulationAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Simulating event-driven data output...");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    //random data
                    byte[] data = FT232Simulator.GenerateRandomData(_bufferSize);

                    //not random data
                    //byte[] data = new byte[] { 0b10101010, 0b11001100 }; 
                    DataGenerated?.Invoke(this, data);

                    Console.WriteLine($"Generated {data.Length} bytes of random data.");
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
