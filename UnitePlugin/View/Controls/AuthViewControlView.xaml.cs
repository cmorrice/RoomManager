using System.Windows.Controls;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.View.Controls
{
    /// <summary>
    /// Interaction logic for AuthViewControlView.xaml
    /// </summary>
    public partial class AuthViewControlView : UserControl
    {
        public AuthViewControlView()
        {
            InitializeComponent();
            DataContext = SingletonViewModelFactory<AuthViewControlViewModel>.GetInstance;
        }
    }
}
