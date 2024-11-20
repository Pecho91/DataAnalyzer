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
    public class GridPlotter
    {
        private readonly Canvas _canvas;
        private readonly Brush _lineBrush;
        private double _scaleX = 1;
        private double _scaleY = 1;

        public GridPlotter(Canvas canvas, Brush lineBrush)
        {
            _canvas = canvas;
            _lineBrush = lineBrush;
        }

        public void SetScale(double scaleX, double scaleY)
        {
            _scaleX = scaleX;
            _scaleY = scaleY;
        }

        public void DrawGrid()
        {
            _canvas.Children.Clear();
            double gridSpacingX = 20 * _scaleX;
            double gridSpacingY = 20 * _scaleY;

            if (_canvas.ActualWidth == 0 || _canvas.ActualHeight == 0)
            {
                return;
            }


            // Draw vertical grid lines
            for (double x = 0; x < _canvas.ActualWidth; x += gridSpacingX)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = _canvas.ActualHeight,
                    Stroke = _lineBrush,
                    StrokeThickness = 0.2
                });
            }

            // Draw horizontal grid lines
            for (double y = 0; y < _canvas.ActualHeight; y += gridSpacingY)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = _canvas.ActualWidth,
                    Y2 = y,
                    Stroke = _lineBrush,
                    StrokeThickness = 0.2
                });
            }
        }
    }
}
