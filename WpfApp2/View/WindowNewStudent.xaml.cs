using System;
using System.Linq;
using System.Windows;
using WpfApp2.Model;

namespace WpfApp2.View
{
    public partial class WindowNewStudent : Window
    {
        public WindowNewStudent()
        {
            InitializeComponent();
            using (var context = new AppDbContext())
            {
                var groups = context.Groups.ToList();
                CbGroup.ItemsSource = groups;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CbGroup.SelectedItem == null)
            {
                MessageBox.Show("Выберите группу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
            this.Close();
        }
    }
}