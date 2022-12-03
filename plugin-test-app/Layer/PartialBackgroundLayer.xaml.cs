using Intel.Unite.Common.Display;
using System;
using System.Windows;

namespace UnitePluginTestApp.Layer
{
    /// <summary>
    /// Interaction logic for PartialBackgroundLayer.xaml
    /// </summary>
    public partial class PartialBackgroundLayer : Window
    {
        private PhysicalDisplay _physicalDisplayInfo;

        public PartialBackgroundLayer()
        {
            InitializeComponent();
        }

        public PartialBackgroundLayer(PhysicalDisplay physicalDisplayInfo) : this()
        {
            _physicalDisplayInfo = physicalDisplayInfo;
        }

        internal void Allocate(FrameworkElement uiControl, Guid guid)
        {
            PartialBackgroundView.Allocate(uiControl, guid);
            Show();
        }

        internal void DeAllocate(Guid id)
        {
            PartialBackgroundView.DeAllocate(id);
            if (PartialBackgroundView.stackPanelPartialBackgroundViews.Children.Count == 0) Hide();
        }
    }
}
