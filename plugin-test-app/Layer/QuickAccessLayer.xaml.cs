using System;
using System.Windows;
using Intel.Unite.Common.Display;


namespace UnitePluginTestApp.Layer
{
    /// <summary>
    /// Interaction logic for QuickAccessLayer.xaml
    /// </summary>
    public partial class QuickAccessLayer : Window
    {
        public QuickAccessLayer()
        {
            InitializeComponent();
        }

        internal void AllocateIcon(FrameworkElement uiControl, Guid id)
        {
            quickAccessIcons.Allocate(uiControl, id);
        }

        internal bool ShowQuickAccessAppView(DisplayView displayView)
        {
            return quickAccessControl.Show();
        }

        internal void AllocateControl(FrameworkElement uiControl, Guid id)
        {
            quickAccessControl.Allocate(uiControl, id);
        }

        internal void AllocateStatus(FrameworkElement uiControl, Guid id)
        {
            statusView.Allocate(uiControl, id);
        }
    }
}
