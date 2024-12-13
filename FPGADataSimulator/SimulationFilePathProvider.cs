using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.AppBroadcasting;

namespace FPGADataSimulator
{
    public static class SimulationFilePathProvider
    {
        public static string GetSimulatedFilePath()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimulatedData");
            Directory.CreateDirectory(directory);

            string filePath = Path.Combine(directory, "simulated_fpga_data.bin");

            // Ensure the file exists (create it if missing)
            if (!File.Exists(filePath))
            {
                Debug.WriteLine("Simulated file does not exist. Creating dummy file.");
                File.WriteAllText(filePath, "Simulated FPGA data content.");
            }

            Debug.WriteLine($"Simulated file path generated: {filePath}");
            return filePath;

        }
    }
}
