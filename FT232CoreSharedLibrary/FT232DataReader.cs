using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FT232CoreSharedLibrary
{
    public class FT232DataReader
    {
        [DllImport("FTD2XX.dll")]
        private static extern uint FT_Read(IntPtr ftHandle, byte[] buffer, uint bufferSize, ref uint bytesRead);

        private readonly IntPtr _ftHandle;

        public FT232DataReader(IntPtr ftHandle)
        {
            _ftHandle = ftHandle;
        }

        public async Task<byte[]> ReadDataAsync(uint bufferSize)
        {
            return await Task.Run(() =>
            {
                byte[] buffer = new byte[bufferSize];
                uint bytesRead = 0;

                if (FT_Read(_ftHandle, buffer, bufferSize, ref bytesRead) != 0 || bytesRead != bufferSize)
                {
                    throw new InvalidOperationException("Failed to read the expected amount of data from FT232");
                }

                return buffer;
            });
        }
    }
}
