using System.Windows.Controls;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.View.Controls
{
    /// <summary>
    /// Interaction logic for StatusViewView.xaml
    /// </summary>
    public partial class StatusViewControlView : UserControl
    {
        public StatusViewControlView()
        {
            InitializeComponent();
            DataContext = SingletonViewModelFactory<StatusViewControlViewModel>.GetInstance;
        }
    }
}
