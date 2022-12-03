using System.Windows.Controls;
using UnitePlugin.ViewModel.Controls;
using UnitePlugin.ViewModel.Factory;

namespace UnitePlugin.View.Controls
{
    /// <summary>
    /// Interaction logic for PresentationControlView.xaml
    /// </summary>
    public partial class PresentationControlView : UserControl
    {
        public PresentationControlView()
        {
            InitializeComponent();
            DataContext = SingletonViewModelFactory<PresentationControlViewModel>.GetInstance;
        }
    }
}
