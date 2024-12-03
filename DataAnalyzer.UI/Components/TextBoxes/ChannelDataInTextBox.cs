using DataAnalyzer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataAnalyzer.UI.Components.TextBoxes
{
    public class ChannelDataInTextBox
    {
        private ChannelDataModel ChannelData;
        public ChannelDataInTextBox() 
        {
            ChannelData = new ChannelDataModel();
        }

        private void DisplayChannelDataInTextBox(ChannelDataModel channelData, TextBox textbox)
        {
            if (channelData?.BooleanLevels == null || textbox == null)
            {
                return;
            }

            string binaryString = string.Concat(ChannelData.BooleanLevels.Select(b => b ? 1 : 0));

            textbox.Text = binaryString;

        }
    }
}
