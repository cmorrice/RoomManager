using System.Windows.Controls;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.View.Controls
{
    /// <summary>
    /// Interaction logic for PartialBackgroundControlView.xaml
    /// </summary>
    public partial class PartialBackgroundControlView : UserControl
    {
        public PartialBackgroundControlView()
        {
            InitializeComponent();
            DataContext = SingletonViewModelFactory<PartialBackgroundControlViewModel>.GetInstance;
        }
    }
}
