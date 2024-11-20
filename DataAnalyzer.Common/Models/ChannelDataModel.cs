using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Common.Models
{
    public class ChannelDataModel
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public bool[] BooleanLevels { get; set; }

        public DateTime Timestamp { get; set; }
       
    }
}
