using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WpfApp2.Model;
using WpfApp2.ViewModel;

namespace WpfApp2.View
{
    public partial class WindowStudent : Window
    {
        private StudentViewModel vmStudent;
        private GroupViewModel vmGroup;
        private ObservableCollection<StudentDPO> studentsDPO;

        public WindowStudent()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            vmStudent = new StudentViewModel();
            vmGroup = new GroupViewModel();

            // Конвертируем Student → StudentDPO для отображения
            studentsDPO = new ObservableCollection<StudentDPO>();
            foreach (var student in vmStudent.ListStudent)
            {
                var dpo = new StudentDPO().CopyFromStudent(student);
                studentsDPO.Add(dpo);
            }
            lvStudent.ItemsSource = studentsDPO;
        }

        // ➕ Добавление студента
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var wnStudent = new WindowNewStudent
            {
                Title = "Новый студент",
                Owner = this,
                DataContext = new StudentDPO { Id = vmStudent.MaxId() + 1, BirthDate = DateTime.Today }
            };
            wnStudent.CbGroup.ItemsSource = vmGroup.ListGroup;

            if (wnStudent.ShowDialog() == true)
            {
                var newDPO = wnStudent.DataContext as StudentDPO;
                if (newDPO != null)
                {
                    // Добавляем в отображаемую коллекцию
                    studentsDPO.Add(newDPO);

                    // Конвертируем и добавляем в основную модель
                    var newStudent = new Student().CopyFromStudentDPO(newDPO);
                    vmStudent.ListStudent.Add(newStudent);

                    // Обновляем ListView
                    lvStudent.ItemsSource = null;
                    lvStudent.ItemsSource = studentsDPO;
                }
            }
        }

        // ✏️ Редактирование студента
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedDPO = lvStudent.SelectedItem as StudentDPO;
            if (selectedDPO == null)
            {
                MessageBox.Show("Выберите студента для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnStudent = new WindowNewStudent
            {
                Title = "Редактирование студента",
                Owner = this,
                DataContext = selectedDPO.ShallowCopy()
            };
            wnStudent.CbGroup.ItemsSource = vmGroup.ListGroup;
            wnStudent.CbGroup.SelectedValue = vmGroup.ListGroup.FirstOrDefault(g => g.GroupName == selectedDPO.GroupName);

            if (wnStudent.ShowDialog() == true)
            {
                var editedDPO = wnStudent.DataContext as StudentDPO;
                if (editedDPO != null)
                {
                    // Обновляем отображаемые данные
                    selectedDPO.FirstName = editedDPO.FirstName;
                    selectedDPO.LastName = editedDPO.LastName;
                    selectedDPO.MiddleName = editedDPO.MiddleName;
                    selectedDPO.GroupName = editedDPO.GroupName;
                    selectedDPO.BirthDate = editedDPO.BirthDate;
                    selectedDPO.AverageGrade = editedDPO.AverageGrade;

                    // Находим и обновляем в основной модели
                    var student = vmStudent.ListStudent.FirstOrDefault(s => s.Id == selectedDPO.Id);
                    if (student != null)
                    {
                        student.CopyFromStudentDPO(selectedDPO);
                    }

                    // Обновляем ListView
                    lvStudent.ItemsSource = null;
                    lvStudent.ItemsSource = studentsDPO;
                }
            }
        }

        // 🗑️ Удаление студента
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedDPO = lvStudent.SelectedItem as StudentDPO;
            if (selectedDPO == null)
            {
                MessageBox.Show("Выберите студента для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить студента: {selectedDPO.LastName} {selectedDPO.FirstName}?",
                "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                // Удаляем из отображаемой коллекции
                studentsDPO.Remove(selectedDPO);

                // Удаляем из основной модели
                var student = vmStudent.ListStudent.FirstOrDefault(s => s.Id == selectedDPO.Id);
                if (student != null)
                {
                    vmStudent.ListStudent.Remove(student);
                }
            }
        }
    }
}