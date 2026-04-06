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
            // Валидация: проверка заполнения обязательных полей
            if (string.IsNullOrWhiteSpace(TbGroupName.Text))
            {
                MessageBox.Show("Введите название группы", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                TbGroupName.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}