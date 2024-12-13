using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FT232CoreSharedLibrary
{
    public static class FT232Initializer
    {
        [DllImport("FTD2XX.dll")]
        private static extern uint FT_Open(uint deviceIndex, ref IntPtr ftHandle);

        [DllImport("FTD2XX.dll")]
        private static extern uint FT_SetBaudRate(IntPtr ftHandle, uint baudRate);

        [DllImport("FTD2XX.dll")]
        private static extern uint FT_SetBitMode(IntPtr ftHandle, byte mask, byte mode);

        [DllImport("FTD2XX.dll")]
        private static extern uint FT_Close(IntPtr ftHandle);

        public static IntPtr InitializeDevice(uint deviceIndex, uint baudRate, byte bitMode)
        {
            IntPtr ftHandle = IntPtr.Zero;

            if (FT_Open(deviceIndex, ref ftHandle) != 0)
                throw new InvalidOperationException("Failed to open FT232 device");

            if (FT_SetBaudRate(ftHandle, baudRate) != 0)
                throw new InvalidOperationException("Failed to set baud rate");

            if (FT_SetBitMode(ftHandle, bitMode, 0x00) != 0)
                throw new InvalidOperationException("Failed to set bit mode");

            return ftHandle;
        }

        public static void CloseDevice(IntPtr ftHandle)
        {
            FT_Close(ftHandle);
        }
    }
}
