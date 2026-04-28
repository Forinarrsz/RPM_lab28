using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Student> ListStudent { get; set; } = new ObservableCollection<Student>();

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
                EditStudent?.RaiseCanExecuteChanged();
                DeleteStudent?.RaiseCanExecuteChanged();
            }
        }

        public string Error { get; set; }
        public string Message { get; set; }

        public StudentViewModel()
        {
            ListStudent = GetStudents();
        }

        #region Commands

        private RelayCommand _addStudent;
        public RelayCommand AddStudent
        {
            get
            {
                return _addStudent ?? (_addStudent = new RelayCommand(obj =>
                {
                    WindowNewStudent wnStudent = new WindowNewStudent
                    {
                        Title = "Новый студент",
                        Owner = Application.Current.MainWindow
                    };

                    Student newStudent = new Student
                    {
                        Birthday = DateTime.Now
                    };
                    wnStudent.DataContext = newStudent;

                    if (wnStudent.ShowDialog() == true)
                    {
                        using (var context = new AppDbContext())
                        {
                            try
                            {
                                if (newStudent.GroupId <= 0)
                                {
                                    MessageBox.Show("Необходимо выбрать группу студента.", "Предупреждение");
                                    return;
                                }

                                context.Students.Add(newStudent);
                                context.SaveChanges();

                                ListStudent.Clear();
                                ListStudent = GetStudents();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка добавления данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
                    }
                }, (obj) => true));
            }
        }

        private RelayCommand _editStudent;
        public RelayCommand EditStudent
        {
            get
            {
                return _editStudent ?? (_editStudent = new RelayCommand(obj =>
                {
                    Student editStudent = SelectedStudent;
                    WindowNewStudent wnStudent = new WindowNewStudent
                    {
                        Title = "Редактирование студента",
                        Owner = Application.Current.MainWindow
                    };

                    
                    Student tempStudent = new Student
                    {
                        Id = editStudent.Id,
                        GroupId = editStudent.GroupId,
                        FirstName = editStudent.FirstName,
                        LastName = editStudent.LastName,
                        MiddleName = editStudent.MiddleName,
                        Birthday = editStudent.Birthday,
                        AverageGrade = editStudent.AverageGrade
                    };
                    wnStudent.DataContext = tempStudent;

                    if (wnStudent.ShowDialog() == true)
                    {
                        using (var context = new AppDbContext())
                        {
                            try
                            {
                                // Находим сущность в контексте по Id
                                Student student = context.Students.Find(editStudent.Id);
                                if (student != null)
                                {
                                    // Обновляем свойства только если они изменились
                                    if (student.GroupId != tempStudent.GroupId)
                                        student.GroupId = tempStudent.GroupId;
                                    if (student.FirstName != tempStudent.FirstName)
                                        student.FirstName = tempStudent.FirstName?.Trim();
                                    if (student.LastName != tempStudent.LastName)
                                        student.LastName = tempStudent.LastName?.Trim();
                                    if (student.MiddleName != tempStudent.MiddleName)
                                        student.MiddleName = tempStudent.MiddleName?.Trim();
                                    if (student.Birthday != tempStudent.Birthday)
                                        student.Birthday = tempStudent.Birthday;
                                    if (student.AverageGrade != tempStudent.AverageGrade)
                                        student.AverageGrade = tempStudent.AverageGrade;

                                    context.SaveChanges();

                                    ListStudent.Clear();
                                    ListStudent = GetStudents();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка редактирования данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
                    }
                    else
                    {
                        ListStudent.Clear();
                        ListStudent = GetStudents();
                    }
                }, (obj) => SelectedStudent != null && ListStudent.Count > 0));
            }
        }

        private RelayCommand _deleteStudent;
        public RelayCommand DeleteStudent
        {
            get
            {
                return _deleteStudent ?? (_deleteStudent = new RelayCommand(obj =>
                {
                    Student student = SelectedStudent;
                    MessageBoxResult result = MessageBox.Show(
                        $"Удалить данные по студенту:\n{student.LastName} {student.FirstName}",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        using (var context = new AppDbContext())
                        {
                            try
                            {
                                Student delStudent = context.Students.Find(student.Id);
                                if (delStudent != null)
                                {
                                    context.Students.Remove(delStudent);
                                    context.SaveChanges();
                                    ListStudent.Remove(student);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка удаления данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
                    }
                }, (obj) => SelectedStudent != null && ListStudent.Count > 0));
            }
        }

        #endregion

        #region Methods

      
        private ObservableCollection<Student> GetStudents()
        {
            using (var context = new AppDbContext())
            {
                var query = from s in context.Students.Include("Group")
                            orderby s.LastName
                            select s;

                return new ObservableCollection<Student>(query.ToList());
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}