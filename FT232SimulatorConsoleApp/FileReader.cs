using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232SimulatorConsoleApp
{
    public class FileReader
    {
        private readonly string _filePath;

        public FileReader(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<byte[]> ReadDataAsync()
        {
            try
            {
                byte[] data = await File.ReadAllBytesAsync(_filePath);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return Array.Empty<byte>();  // Return an empty byte array in case of an error
            }
        }
    }
}