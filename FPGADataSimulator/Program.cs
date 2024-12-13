namespace FPGADataSimulator
{
    public class Program
    {
        private static bool _isRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("FPGA Data Simulator started...");
            Console.WriteLine("Press Ctrl+C to stop the simulator.");

            string filePath = SimulationFilePathProvider.GetSimulatedFilePath();

            Thread simulationThread = new Thread(() => SimulateFPGADataWithFSM(filePath));
            simulationThread.Start();
            
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Stopping simulator...");
                _isRunning = false;
                eventArgs.Cancel = true;
            };

            simulationThread.Join();
            Console.WriteLine("Simulator stopped.");
        }

        private static void SimulateFPGADataWithFSM(string filePath)
        {
            Random random = new Random();
            FPGAState currentState = FPGAState.Idle;
            int stateDuration = 0;

            try
            {
                while (_isRunning)
                {
                    if (stateDuration <= 0)
                    {
                        currentState = GetNextState(random, currentState);
                        stateDuration = random.Next(10, 50); // Stay in state for 10–50 cycles
                    }

                    byte[] buffer = GenerateDataForState(currentState, random);

                    AddNoise(buffer, random);

                    File.WriteAllBytes(filePath, buffer);

                    Thread.Sleep(50);

                    stateDuration--;
                }
            }
            finally
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine("Simulated data file deleted.");
                }
            }
        }

        private static FPGAState GetNextState(Random random, FPGAState currentState)
        {
            // Define state transitions (e.g., 70% stay in the same state, 15% change to Active, etc.)
            double rand = random.NextDouble();
            return currentState switch
            {
                FPGAState.Idle => rand < 0.7 ? FPGAState.Idle : (rand < 0.85 ? FPGAState.Active : FPGAState.Error),
                FPGAState.Active => rand < 0.7 ? FPGAState.Active : (rand < 0.85 ? FPGAState.Idle : FPGAState.Error),
                FPGAState.Error => rand < 0.5 ? FPGAState.Error : FPGAState.Idle,
                _ => FPGAState.Idle
            };
        }

        private static byte[] GenerateDataForState(FPGAState state, Random random)
        {
            byte[] buffer = new byte[1024];

            switch (state)
            {
                case FPGAState.Idle:
                    // Mostly zeros, occasional ones
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (byte)(random.NextDouble() < 0.1 ? 0b00000001 : 0b00000000);
                    }
                    break;

                case FPGAState.Active:
                    // Structured patterns
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (i % 2 == 0) ? (byte)0b10101010 : (byte)0b11110000;
                    }
                    break;

                case FPGAState.Error:
                    // Random bursts of data
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = (byte)random.Next(0, 7);
                    }
                    break;
            }

            return buffer;
        }

        private static void AddNoise(byte[] buffer, Random random)
        {
            // Introduce random bit flips with a 5% probability
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int bit = 0; bit < 8; bit++)
                {
                    if (random.NextDouble() < 0.05)
                    {
                        Console.WriteLine($"Flipping bit {bit} in byte {i}.");
                        buffer[i] ^= (byte)(1 << bit);
                    }
                }
            }
        }
    }
}
