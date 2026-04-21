using System.Windows;

namespace WpfApp2.View
{
    public partial class WindowNewGroup : Window
    {
        public WindowNewGroup()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}