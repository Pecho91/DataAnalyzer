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
            // Check if the channelData or BooleanLevels is null or empty
            if (channelData?.BooleanLevels == null || channelData.BooleanLevels.Length == 0)
                return; // No data to plot, exit the method

            // Clear previous canvas children
            _canvas.Children.Clear();

            // Get canvas width and compute segment width based on the length of BooleanLevels
            double width = _canvas.ActualWidth * _scaleX;
            double segmentWidth = width / channelData.BooleanLevels.Length;
            double centerY = _canvas.ActualHeight;

            // Initialize previous state and position for the first point
            bool previousState = channelData.BooleanLevels[0];
            double previousX = 0;
            double previousY = previousState ? centerY - 20 * _scaleY : centerY + 20 * _scaleY;

            // Loop through the BooleanLevels and plot each segment
            for (int i = 1; i < channelData.BooleanLevels.Length; i++)
            {
                bool currentState = channelData.BooleanLevels[i];
                double currentX = i * segmentWidth;
                double currentY = currentState ? centerY - 20 * _scaleY : centerY + 20 * _scaleY;

                // Add a line between previous and current state
                _canvas.Children.Add(new Line
                {
                    X1 = previousX,
                    Y1 = previousY,
                    X2 = currentX,
                    Y2 = currentY,
                    Stroke = _lineBrush,
                    StrokeThickness = 1
                });

                // Update the previous position for the next iteration
                previousX = currentX;
                previousY = currentY;
            }
        }
    }
}
