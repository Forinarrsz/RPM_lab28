using System.Windows;

namespace WpfApp2.View
{
    public partial class WindowNewStudent : Window
    {
        public WindowNewStudent()
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