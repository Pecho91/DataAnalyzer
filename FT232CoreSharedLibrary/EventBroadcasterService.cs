using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FT232CoreSharedLibrary
{
    public class EventBroadcasterService
    {
        public event EventHandler<byte[]> DataReceived;

        public void OnDataReceived(byte[] data)
        {
            DataReceived?.Invoke(this, data);
        }
    }
}
