using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232SimulatorConsoleApp
{
    public static class FT232Simulator
    {
        private static readonly Random RandomGenerator = new Random();

        /// <summary>
        /// Generates random data to simulate FT232 output.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer to generate.</param>
        /// <returns>A byte array filled with random data.</returns>
        public static byte[] GenerateRandomData(uint bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            RandomGenerator.NextBytes(buffer); // Fill with random bytes
            return buffer;
        }
    }
}
