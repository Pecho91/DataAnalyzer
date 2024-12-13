using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232CoreSharedLibrary
{
    public class FileWriterService
    {
        private readonly string _filePath;

        public FileWriterService(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveData(byte[] data)
        {
            using var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write, FileShare.None);
            stream.Write(data, 0, data.Length);
            Debug.WriteLine($"Saved {data.Length} bytes to {_filePath}");
        }
    }
}
