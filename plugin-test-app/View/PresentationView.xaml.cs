using Intel.Unite.Common.Display;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UnitePluginTestApp.View
{
    /// <summary>
    /// Interaction logic for PresentationHubView.xaml
    /// </summary>
    public partial class PresentationView : UserControl
    {
        private readonly Dictionary<Guid, FrameworkElement> _views = new Dictionary<Guid, FrameworkElement>();

        public PresentationView()
        {
            InitializeComponent();
        }

        internal void Allocate(FrameworkElement uiControl, Guid guid)
        {
            _views.Add(guid, uiControl);
            stackPannelPresentationViews.Children.Add(uiControl);
        }

        internal void DeAllocate(FrameworkElement uiControl, Guid guid)
        {
            _views.Add(guid, uiControl);
            stackPannelPresentationViews.Children.Remove(uiControl);
        }

        internal void DeAllocate(DisplayView allocatedDisplayView)
        {
            stackPannelPresentationViews.Children.Remove(_views[allocatedDisplayView.Id]);
            _views.Remove(allocatedDisplayView.Id);
        }
    }
}
