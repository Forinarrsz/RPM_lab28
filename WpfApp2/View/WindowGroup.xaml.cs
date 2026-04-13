using System.Windows;
using WpfApp2.ViewModel;

namespace WpfApp2.View
{
    public partial class WindowGroup : Window
    {
        public WindowGroup()
        {
            InitializeComponent();
            DataContext = new GroupViewModel();
        }
    }
}