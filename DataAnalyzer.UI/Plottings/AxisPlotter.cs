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
    public class AxisPlotter
    {
        private readonly Canvas _canvas;
        private readonly Brush _lineBrush;
        private double _scaleX = 1;
        private double _scaleY = 1;

        public AxisPlotter(Canvas canvas, Brush lineBrush)
        {
            _canvas = canvas;
            _lineBrush = lineBrush;
        }

        public void SetScale(double scaleX, double scaleY)
        {
            _scaleX = scaleX;
            _scaleY = scaleY;
        }

        public void DrawAxes()
        {
            double width = _canvas.ActualWidth;
            double height = _canvas.ActualHeight;

            // Clear the canvas if needed
            _canvas.Children.Clear();

            // Check if the canvas size is valid (non-zero)
            if (width <= 0 || height <= 0)
            {
                return; // Exit if canvas has no size, avoid drawing outside of bounds
            }

            // Draw X-axis
            _canvas.Children.Add(new Line
            {
                X1 = 0,
                Y1 = height / 2,
                X2 = width,
                Y2 = height / 2,
                Stroke = _lineBrush,
                StrokeThickness = 2
            });

            // Draw Y-axis
            _canvas.Children.Add(new Line
            {
                X1 = width / 2,
                Y1 = 0,
                X2 = width / 2,
                Y2 = height,
                Stroke = _lineBrush,
                StrokeThickness = 2
            });
        }
    }
}
