using DataAnalyzer.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataAnalyzer.UI.Transformations
{ 
        public class ZoomManager
        {
            private double _scaleX = 1.0;
            private double _scaleY = 1.0;

            public double ScaleX => _scaleX;
            public double ScaleY => _scaleY;

            public void ZoomIn()
            {
                _scaleX *= 1.1;
                _scaleY *= 1.1;
                //ApplyZoom();
            }

            public void ZoomOut()
            {
                _scaleX *= 0.9;
                _scaleY *= 0.9;
                //ApplyZoom();
            }

            //private void ApplyZoom()
            //{
            //    throw new NotImplementedException();
            //}

            public void Pan(double offsetX, double offsetY)
            {
                // Handle panning (translation) logic here.
            }

            internal double GetCurrentZoomLevel()
            {
                return _scaleX;
            }
        }
}
