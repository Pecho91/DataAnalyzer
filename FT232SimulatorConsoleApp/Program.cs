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
                uint bufferSize = 1024; // Example buffer size

                var fileSimulator = new FileSimulator(filePath, bufferSize);

                // Stage 1: Write simulated data to the file
                await fileSimulator.StartSimulationAsync(cts.Token);

                // Stage 2: Now read data back from the file and process it (for example, event-driven)
                Console.WriteLine($"Reading data back from: {filePath}");
                var fileReader = new FileReader(filePath);
                byte[] dataFromFile = await fileReader.ReadDataAsync();

                // Process the data (for example, print bits)
                Console.WriteLine("Processing data from file (in bits):");
                ProcessDataAsBits(dataFromFile);
            }
            else if (choice == "2")
            {
                uint bufferSize = 1024; // Example buffer size

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    Console.WriteLine($"Event received: {data.Length} bytes of data.");
                };

                await eventSimulator.StartSimulationAsync(cts.Token);
            }

            else if (choice == "3")
            {
                uint bufferSize = 1024; // Example buffer size

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
                uint bufferSize = 1024; // Example buffer size

                var eventSimulator = new EventSimulator(bufferSize);
                eventSimulator.DataGenerated += (sender, data) =>
                {
                    // Print the received data bit by bit
                    string binaryData = BitConverter.ToString(data).Replace("-", string.Empty); // Converts byte array to binary string
                    foreach (char bit in binaryData)
                    {
                        Console.Write(bit); // Print each bit
                    }
                    Console.WriteLine(); // Move to the next line after printing all bits
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
                // Default file path (could be a specific directory or the application's working directory)
                filePath = Path.Combine(Environment.CurrentDirectory, "simulation_output.txt");
                Console.WriteLine($"No path provided. Using default file path: {filePath}");
            }

            // Check if the file path is valid (create the file if necessary)
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
    }
}
