using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UnitePluginTestApp.View
{
    /// <summary>
    /// Interaction logic for QuickAccessControl.xaml
    /// </summary>
    public partial class QuickAccessControl : UserControl
    {
        private Dictionary<Guid, FrameworkElement> _views = new Dictionary<Guid, FrameworkElement>();

        public QuickAccessControl()
        {
            InitializeComponent();
        }

        internal bool Show()
        {
            stackPannelQuickAccessControl.Visibility = Visibility.Visible;
            return true;
        }

        internal void Allocate(FrameworkElement uiControl, Guid id)
        {
            _views.Add(id, uiControl);
            stackPannelQuickAccessControl.Children.Add(uiControl);
        }

        internal void DeAllocate(Guid id)
        {
            stackPannelQuickAccessControl.Children.Remove(_views[id]);
            _views.Remove(id);
        }
    }
}
