using WpfApp2.View;
using System.Windows;
using WpfApp2.View;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void mnuStudents_Click(object sender, RoutedEventArgs e)
        {
            new WindowStudent().ShowDialog();
        }

        private void mnuGroups_Click(object sender, RoutedEventArgs e)
        {
            new WindowGroup().ShowDialog();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}