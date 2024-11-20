using DataAnalyzer.Common.Models;
using DataAnalyzer.UI.Plottings;
using DataAnalyzer.UI.Transformations;
using DataAnalyzer.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataAnalyzer.UI.Controls
{
    /// <summary>
    /// Interaction logic for ChannelControl.xaml
    /// </summary>
    public partial class ChannelControl : UserControl
    {
        private readonly ZoomManager _zoomManager;
        private readonly GridPlotter _gridPlotter;
        private readonly ChannelDataPlotter _channelDataPlotter;

        public ChannelControl()
        {
            InitializeComponent();

            _zoomManager = new ZoomManager();
            _gridPlotter = new GridPlotter(GridCanvas, Brushes.Blue);
            _channelDataPlotter = new ChannelDataPlotter(WaveformCanvas, Brushes.Black);

            GridCanvas.SizeChanged += (s, e) => _gridPlotter.DrawGridWithTimeMarkers(TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

            this.DataContextChanged += ChannelControl_DataContextChanged;
        
        }

        private void ChannelControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ChannelViewModel channelViewModel)
            {
                channelViewModel.PropertyChanged += (s, ev) =>
                {
                    if (ev.PropertyName == nameof(ChannelViewModel.ChannelData))
                    {
                        _channelDataPlotter.PlotChannelData(channelViewModel.ChannelData);
                    }
                };

                if (channelViewModel.ChannelData != null)
                {
                    _channelDataPlotter.PlotChannelData(channelViewModel.ChannelData);
                }             
            }
        }

        // Zoom event handler when mouse wheel is used
        //private void WaveformCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var viewModel = (ChannelViewModel)DataContext;

        //    // Zoom in when mouse wheel is scrolled up
        //    if (e.Delta > 0)
        //    {
        //        viewModel.ZoomIn();
        //    }
        //    // Zoom out when mouse wheel is scrolled down
        //    else
        //    {
        //        viewModel.ZoomOut();
        //    }

        //    ApplyZoom();
        //}

        //// Apply zoom effect by modifying the LayoutTransform of the WaveformCanvas
        //private void ApplyZoom()
        //{
        //    var canvas = WaveformCanvas;

        //    // Apply the zoom by scaling the canvas transform
        //    canvas.LayoutTransform = new ScaleTransform(_zoomManager.ScaleX, _zoomManager.ScaleY);

        //    // Update the grid to reflect the new scale
        //    _gridPlotter.SetScale(_zoomManager.ScaleX, _zoomManager.ScaleY);
        //    _gridPlotter.DrawGrid();  // Redraw grid with the updated scale
        //}

    }
}