using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.Model;

namespace WpfApp2.View
{
    public partial class WindowNewStudent : Window
    {
        // ✅ Публичное свойство для доступа к ComboBox извне
        public ComboBox CbGroup => CbGroupCombo;

        public WindowNewStudent()
        {
            InitializeComponent();
            Loaded += WindowNewStudent_Loaded;
        }

        private void WindowNewStudent_Loaded(object sender, RoutedEventArgs e)
        {
            // Синхронизация DatePicker с Binding
            if (DataContext is StudentDPO dpo)
            {
                DpBirthDate.SelectedDate = dpo.BirthDate;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is StudentDPO dpo)
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(dpo.LastName) ||
                    string.IsNullOrWhiteSpace(dpo.FirstName))
                {
                    MessageBox.Show("Заполните фамилию и имя", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Синхронизация DatePicker
                if (DpBirthDate.SelectedDate.HasValue)
                {
                    dpo.BirthDate = DpBirthDate.SelectedDate.Value;
                }

                DialogResult = true;
                Close();
            }
        }
    }
}