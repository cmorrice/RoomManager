using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Intel.Unite.Common.Display;

namespace UnitePluginTestApp.View
{
    /// <summary>
    /// Interaction logic for QuickAccessIcons.xaml
    /// </summary>
    public partial class QuickAccessIcons : UserControl
    {
        private Dictionary<Guid, FrameworkElement> _views = new Dictionary<Guid, FrameworkElement>();

        public QuickAccessIcons()
        {
            InitializeComponent();
        }

        internal void Allocate(FrameworkElement uiControl, Guid id)
        {
            _views.Add(id, uiControl);
            stackPannelQuickAccessIcons.Children.Add(uiControl);
        }

        internal void Deallocate(FrameworkElement uiControl, Guid id)
        {
            _views.Remove(id);
            stackPannelQuickAccessIcons.Children.Add(uiControl);
        }

        internal void DeAllocate(DisplayView allocatedDisplayView)
        {
            stackPannelQuickAccessIcons.Children.Remove(_views[allocatedDisplayView.Id]);
            _views.Remove(allocatedDisplayView.Id);
        }

    }
}
