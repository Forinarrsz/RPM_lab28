using System.Windows;

namespace WpfApp2.View
{
    public partial class WindowStudent : Window
    {
        public WindowStudent()
        {
            InitializeComponent();
            
            this.DataContext = new WpfApp2.ViewModel.StudentViewModel();
        }
    }
}