using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalyzer.Common.Models
{
    public class MultiChannelDataModel
    {
        public List<ChannelDataModel> Channels { get; set; } = new List<ChannelDataModel>();
        public DateTime TimeStamp { get; set; }
    }
}
