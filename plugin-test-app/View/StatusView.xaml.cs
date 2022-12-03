using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UnitePluginTestApp.View
{
    /// <summary>
    /// Interaction logic for StatusHubView.xaml
    /// </summary>
    public partial class StatusView : UserControl
    {
        private readonly Dictionary<Guid, FrameworkElement> _views = new Dictionary<Guid, FrameworkElement>();

        public StatusView()
        {
            InitializeComponent();
        }

        internal void Allocate(FrameworkElement uiControl, Guid id)
        {
            _views.Add(id, uiControl);
            stackPannelStatusViews.Children.Add(uiControl);
        }

        internal void Deallocate(FrameworkElement uiControl, Guid id)
        {
            _views.Remove(id);
            stackPannelStatusViews.Children.Add(uiControl);
        }

        internal void DeAllocate(Guid id)
        {
            stackPannelStatusViews.Children.Remove(_views[id]);
            _views.Remove(id);
        }


    }
}
