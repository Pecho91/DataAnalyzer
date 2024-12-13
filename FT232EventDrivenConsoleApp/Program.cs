using FT232CoreSharedLibrary;

namespace FT232EventDrivenConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting FT232 Event-Driven Console Application...");

            uint bufferSize = args.Length > 0 && uint.TryParse(args[0], out var size) ? size : 1024;

            IntPtr ftHandle;
            try
            {
                ftHandle = FT232Initializer.InitializeDevice(0, 9600, 0x00);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to initialize FT232 device: {ex.Message}");
                return;
            }

            var reader = new FT232DataReader(ftHandle);
            var eventBroadcasterService = new EventBroadcasterService();

            eventBroadcasterService.DataReceived += (sender, data) =>
            {
                Console.WriteLine($"Event raised: Received {data.Length} bytes of data.");
            };

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Termination requested...");
                cts.Cancel();
                eventArgs.Cancel = true;
            };

            try
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    var data = await reader.ReadDataAsync(bufferSize);
                    eventBroadcasterService.OnDataReceived(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during data reading: {ex.Message}");
            }
            finally
            {
                FT232Initializer.CloseDevice(ftHandle);
            }
        }
    }
}
