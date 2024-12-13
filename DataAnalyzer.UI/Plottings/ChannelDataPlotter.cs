using DataAnalyzer.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DataAnalyzer.UI.Plottings
{
    public class ChannelDataPlotter
    {
        private readonly Canvas _canvas;
        private readonly Brush _lineBrush;
        private double _scaleX = 4;
        private double _scaleY = 4;

        public ChannelDataPlotter(Canvas canvas, Brush lineBrush)
        {
            _canvas = canvas;
            _lineBrush = lineBrush;
        }

        public void SetScale(double scaleX, double scaleY)
        {
            _scaleX = scaleX;
            _scaleY = scaleY;
        }

        public void PlotChannelData(ChannelDataModel channelData)
        {

            if (channelData?.BooleanLevels == null || channelData.BooleanLevels.Length == 0)
            {
                return;
            }

            _canvas.Children.Clear();

            
            double width = _canvas.ActualWidth * _scaleX;
            double segmentWidth = width / channelData.BooleanLevels.Length;

            double topMargin = 20;
            double bottomMargin = 20;
            double usableHeight = _canvas.ActualHeight - topMargin - bottomMargin;

            double centerY = topMargin + usableHeight / 2;

            bool previousState = channelData.BooleanLevels[0];
            double previousX = 0;
            double previousY = previousState ? centerY - usableHeight / 2 : centerY + usableHeight / 2;

            for (int i = 1; i < channelData.BooleanLevels.Length; i++)
            {
                bool currentState = channelData.BooleanLevels[i];
                double currentX = i * segmentWidth;
                double currentY = currentState ? centerY - usableHeight / 2 : centerY + usableHeight / 2;

                _canvas.Children.Add(new Line
                {
                    X1 = previousX,
                    Y1 = previousY,
                    X2 = currentX,
                    Y2 = currentY,
                    Stroke = _lineBrush,
                    StrokeThickness = 1
                });

                previousX = currentX;
                previousY = currentY;
            }
        }
    }
}
