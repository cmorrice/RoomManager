using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UnitePluginTestApp.View
{
    /// <summary>
    /// Interaction logic for PartialBackgroundHubView.xaml
    /// </summary>
    public partial class PartialBackgroundView : UserControl
    {
        private Dictionary<Guid, FrameworkElement> _views = new Dictionary<Guid, FrameworkElement>();

        public PartialBackgroundView()
        {
            InitializeComponent();
        }

        internal void Allocate(FrameworkElement uiControl, Guid id)
        {
            _views.Add(id, uiControl);
            stackPanelPartialBackgroundViews.Children.Add(uiControl);
        }

        internal void DeAllocate(FrameworkElement uiControl, Guid id)
        {
            _views.Remove(id);
            stackPanelPartialBackgroundViews.Children.Remove(uiControl);
        }

        internal void DeAllocate(Guid id)
        {
            stackPanelPartialBackgroundViews.Children.Remove(_views[id]);
            _views.Remove(id);
        }

    }
}
