using System.Windows;
using System.Windows.Controls;
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace WpfApp2.View
{
    public partial class WindowGroup : Window
    {
        private GroupViewModel vmGroup;

        public WindowGroup()
        {
            InitializeComponent();
            vmGroup = new GroupViewModel();
            lvGroup.ItemsSource = vmGroup.ListGroup;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var wnGroup = new WindowNewGroup { Title = "Новая группа", Owner = this };
            int maxId = vmGroup.MaxId() + 1;
            var group = new Group { Id = maxId };

            wnGroup.DataContext = group;

            if (wnGroup.ShowDialog() == true)
            {
                vmGroup.ListGroup.Add(group);
                lvGroup.ItemsSource = null;
                lvGroup.ItemsSource = vmGroup.ListGroup;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var wnGroup = new WindowNewGroup { Title = "Редактирование группы", Owner = this };
            var group = lvGroup.SelectedItem as Group;

            if (group != null)
            {
                var tempGroup = group.ShallowCopy();
                wnGroup.DataContext = tempGroup;

                if (wnGroup.ShowDialog() == true)
                {
                    group.GroupName = tempGroup.GroupName;
                    group.Specialty = tempGroup.Specialty;
                    group.Course = tempGroup.Course;

                    lvGroup.ItemsSource = null;
                    lvGroup.ItemsSource = vmGroup.ListGroup;
                }
            }
            else
            {
                MessageBox.Show("Выберите группу для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var group = lvGroup.SelectedItem as Group;

            if (group != null)
            {
                var result = MessageBox.Show($"Удалить группу: {group.GroupName}?",
                    "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.OK)
                {
                    vmGroup.ListGroup.Remove(group);
                }
            }
            else
            {
                MessageBox.Show("Выберите группу для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}