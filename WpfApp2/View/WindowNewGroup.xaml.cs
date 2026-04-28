using System.Windows;
using WpfApp2.Model;
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
            if (string.IsNullOrWhiteSpace(((Group)this.DataContext).Name))
            {
                MessageBox.Show("Название группы не может быть пустым!", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}