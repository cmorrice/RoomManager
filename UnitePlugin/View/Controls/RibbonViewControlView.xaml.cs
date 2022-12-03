using System.Windows.Controls;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.View.Controls
{
    /// <summary>
    /// Interaction logic for RibbonViewControlView.xaml
    /// </summary>
    public partial class RibbonViewControlView : UserControl
    {
        public RibbonViewControlView()
        {
            InitializeComponent();
            DataContext = SingletonViewModelFactory<RibbonViewControlViewModel>.GetInstance;
        }
    }
}
