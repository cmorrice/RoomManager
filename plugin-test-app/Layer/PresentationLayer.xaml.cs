using System;
using System.Windows;
using Intel.Unite.Common.Display;

namespace UnitePluginTestApp.Layer
{
    /// <summary>
    /// Interaction logic for PresentationLayer.xaml
    /// </summary>
    public partial class PresentationLayer : Window
    {
        private PhysicalDisplay _physicalDisplayInfo;

        public PresentationLayer()
        {
            InitializeComponent();
        }

        public PresentationLayer(PhysicalDisplay physicalDisplayInfo) : this()
        {
            _physicalDisplayInfo = physicalDisplayInfo;
        }

        internal void Allocate(FrameworkElement uiControl, Guid guid)
        {
            presentationView.Allocate(uiControl, guid);
            Show();
        }

        internal void DeAllocate(FrameworkElement uiControl, Guid guid)
        {
            presentationView.DeAllocate(uiControl, guid);
            if (presentationView.stackPannelPresentationViews.Children.Count == 0) Hide();
        }

        internal void DeAllocate(DisplayView allocatedDisplayView)
        {
            presentationView.DeAllocate(allocatedDisplayView);
            if (presentationView.stackPannelPresentationViews.Children.Count == 0) Hide();
        }
    }
}
