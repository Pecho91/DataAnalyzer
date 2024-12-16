namespace FT232SimulatorConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("FT232 Simulator Console App");
            Console.WriteLine("Choose simulation mode:");
            Console.WriteLine("1 = File-Based");
            Console.WriteLine("2 = Event-Driven (bytes)");
            Console.WriteLine("3 = Event-Driven (binary - bits)");
            Console.WriteLine("4 = Event-Driven (binary - hexadecimal)");
            Console.WriteLine("5 = Event-Driven (in channels - bits)");

            var choice = Console.ReadLine();

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Termination requested...");
                cts.Cancel();
                eventArgs.Cancel = true;
            };

            if (choice == "1")
            {
                string filePath = GetFilePath();
                uint bufferSize = 1024; 

                var fileSimulator = new FileSimulator(filePath, bufferSize);

                await fileSimulator.StartSimulationAsync(cts.Token);

                Console.WriteLine($"Reading data back from: {filePath}");
                var fileReader = new FileReader(filePath);
                byte[] dataFromFile = await fileReader.ReadDataAsync();


                Console.WriteLine("Processing data from file (in bits):");
                ProcessDataAsBits(dataFromFile);
            }
            else if (choice == "2")
            {
                uint bufferSize = 1024; 

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    Console.WriteLine($"Event received: {data.Length} bytes of data.");
                };

                await eventSimulator.StartSimulationAsync(cts.Token);
            }

            else if (choice == "3")
            {
                uint bufferSize = 1024; 

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    Console.WriteLine("Event received: Data (in bits):");

                    ProcessDataAsBits(data);
                };

                await eventSimulator.StartSimulationAsync(cts.Token);
            }
            else if (choice == "4")
            {
                uint bufferSize = 1024; 

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    string binaryData = BitConverter.ToString(data).Replace("-", string.Empty); // Converts byte array to binary string
                    foreach (char bit in binaryData)
                    {
                        Console.Write(bit); 
                    }
                    Console.WriteLine(); 
                };

                await eventSimulator.StartSimulationAsync(cts.Token);
            }

            else if (choice == "5")
            {
                uint bufferSize = 1024; 

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    Console.WriteLine("Event received: Data (in channels - bits):");

                    // Process each byte as channels (each bit is a channel)
                    ProcessDataAsChannels(data);
                };

                await eventSimulator.StartSimulationAsync(cts.Token);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        private static string GetFilePath()
        {
            Console.WriteLine("Enter output file path (leave empty to use default):");
            string filePath = Console.ReadLine();

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, "simulation_output.txt");
                Console.WriteLine($"No path provided. Using default file path: {filePath}");
            }

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose(); // Creates the file if it doesn't exist
                Console.WriteLine($"File created at: {filePath}");
            }
            return filePath;
        }

        private static void ProcessDataAsBits(byte[] data)
        {
            // Iterate over each byte
            foreach (byte byteValue in data)
            {
                // Print the bits of each byte
                for (int bitIndex = 7; bitIndex >= 0; bitIndex--)
                {
                    // Use bitwise AND to check the bit value
                    bool bit = (byteValue & (1 << bitIndex)) != 0;
                    Console.Write(bit ? "1" : "0");
                }
                Console.Write(" "); // Space between bytes for readability
            }

            Console.WriteLine(); // New line after printing all bits
        }

        private static void ProcessDataAsChannels(byte[] data )
        {
            //byte[] data = new byte[] { 0b10101010, 0b11001100 }; //channel 1: index 0 - array, ch2: index 1, ch3: index 2, etc

            // Iterate over each bit position (channel)
            for (int channelIndex = 0; channelIndex < 8; channelIndex++)
            {
                Console.WriteLine($"Channel {channelIndex + 1}:");

                // Iterate over each byte in the array and extract the bit at the given channel index (bit position)
                foreach (byte byteValue in data)
                {
                    // Check if the bit at the current position is set (1) or not (0)
                    bool bit = (byteValue & (1 << (7 - channelIndex))) != 0;
                    Console.Write(bit ? "1" : "0");
                }

                // Move to the next line after printing each channel's bits
                Console.WriteLine();
            }
        }
    }
}
