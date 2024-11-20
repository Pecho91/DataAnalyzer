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

            // DataContext should be passed in from parent ViewModel, set to an instance of ChannelViewModel.
            _zoomManager = new ZoomManager();
            _gridPlotter = new GridPlotter(WaveformCanvas, Brushes.Blue);
            _channelDataPlotter = new ChannelDataPlotter(WaveformCanvas, Brushes.Black);

            // Listen for changes in the ChannelViewModel's ChannelData property
            this.DataContextChanged += ChannelControl_DataContextChanged;

            // Event handler to draw the grid when the WaveformCanvas is loaded
            WaveformCanvas.Loaded += (s, e) => _gridPlotter.DrawGrid();

            // Set up the UI for zooming with mouse wheel (if necessary)
            //WaveformCanvas.PreviewMouseWheel += WaveformCanvas_MouseWheel;
        }

        // When DataContext changes (e.g. ChannelViewModel is set), update the plot
        private void ChannelControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ChannelViewModel channelViewModel)
            {
                // Bind to the current channel's data and plot it
                channelViewModel.PropertyChanged += (s, ev) =>
                {
                    if (ev.PropertyName == nameof(ChannelViewModel.ChannelData))
                    {
                        // When ChannelData changes, update the plot
                        _channelDataPlotter.PlotChannelData(channelViewModel.ChannelData);
                    }
                };

                // Initial plot when data is available
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