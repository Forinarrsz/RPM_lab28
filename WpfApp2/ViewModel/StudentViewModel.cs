using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        readonly string path = @"C:\Users\User\source\repos\Forinarrsz\RPM_lab28\WpfApp2\DataModels\StudentData.json";

        public ObservableCollection<Student> ListStudent { get; set; } = new ObservableCollection<Student>();
        public ObservableCollection<StudentDPO> ListStudentDPO { get; set; } = new ObservableCollection<StudentDPO>();

        private StudentDPO _selectedStudentDPO;
        public StudentDPO SelectedStudentDPO
        {
            get => _selectedStudentDPO;
            set
            {
                _selectedStudentDPO = value;
                OnPropertyChanged();
                EditStudent?.RaiseCanExecuteChanged();
                DeleteStudent?.RaiseCanExecuteChanged();
            }
        }

        string _jsonStudents = String.Empty;
        public string Error { get; set; }
        public string Message { get; set; }

        public StudentViewModel()
        {
            ListStudent = LoadStudent();
            ListStudentDPO = GetListStudentDPO();
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

                    int maxId = MaxId() + 1;
                    StudentDPO studentDPO = new StudentDPO
                    {
                        Id = maxId,
                        Birthday = DateTime.Now.ToString("dd.MM.yyyy")
                    };
                    wnStudent.DataContext = studentDPO;

                    if (wnStudent.ShowDialog() == true)
                    {
                        var selectedGroup = wnStudent.CbGroup?.SelectedItem as Group;
                        if (selectedGroup != null)
                        {
                            studentDPO.GroupName = selectedGroup.Name;
                            ListStudentDPO.Add(studentDPO);

                            Student student = new Student
                            {
                                Id = studentDPO.Id,
                                GroupId = selectedGroup.Id,
                                FirstName = studentDPO.FirstName,
                                LastName = studentDPO.LastName,
                                Birthday = studentDPO.Birthday 
                            };
                            ListStudent.Add(student);
                            SaveChanges(ListStudent);
                        }
                        else
                        {
                            Message = "Необходимо выбрать группу студента.";
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
                    WindowNewStudent wnStudent = new WindowNewStudent
                    {
                        Title = "Редактирование студента",
                        Owner = Application.Current.MainWindow
                    };

                    StudentDPO studentDPO = SelectedStudentDPO;
                    StudentDPO tempStudent = studentDPO.ShallowCopy();
                    wnStudent.DataContext = tempStudent;

                    if (wnStudent.ShowDialog() == true)
                    {
                        var selectedGroup = wnStudent.CbGroup?.SelectedItem as Group;
                        if (selectedGroup != null)
                        {
                            studentDPO.GroupName = selectedGroup.Name;
                            studentDPO.FirstName = tempStudent.FirstName;
                            studentDPO.LastName = tempStudent.LastName;
                            studentDPO.Birthday = tempStudent.Birthday;

                            var student = ListStudent.FirstOrDefault(s => s.Id == studentDPO.Id);
                            if (student != null)
                            {
                                student.GroupId = selectedGroup.Id;
                                student.FirstName = studentDPO.FirstName;
                                student.LastName = studentDPO.LastName;
                                student.Birthday = studentDPO.Birthday;
                            }
                            SaveChanges(ListStudent);
                        }
                        else
                        {
                            Message = "Необходимо выбрать группу студента.";
                        }
                    }
                }, (obj) => SelectedStudentDPO != null && ListStudentDPO.Count > 0));
            }
        }

        private RelayCommand _deleteStudent;
        public RelayCommand DeleteStudent
        {
            get
            {
                return _deleteStudent ?? (_deleteStudent = new RelayCommand(obj =>
                {
                    StudentDPO student = SelectedStudentDPO;
                    MessageBoxResult result = MessageBox.Show(
                        $"Удалить данные по студенту:\n{student.LastName} {student.FirstName}",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        ListStudentDPO.Remove(student);
                        var studentModel = ListStudent.FirstOrDefault(s => s.Id == student.Id);
                        if (studentModel != null)
                        {
                            ListStudent.Remove(studentModel);
                            SaveChanges(ListStudent);
                        }
                    }
                }, (obj) => SelectedStudentDPO != null && ListStudentDPO.Count > 0));
            }
        }

        #endregion

        #region Methods

        public ObservableCollection<Student> LoadStudent()
        {
            try
            {
                _jsonStudents = File.ReadAllText(path);
                if (_jsonStudents != null)
                {
                    ListStudent = JsonConvert.DeserializeObject<ObservableCollection<Student>>(_jsonStudents);
                    return ListStudent;
                }
            }
            catch (Exception e)
            {
                Error = "Ошибка чтения JSON файла:\n" + e.Message;
            }
            return new ObservableCollection<Student>();
        }

        public ObservableCollection<StudentDPO> GetListStudentDPO()
        {
            ListStudentDPO.Clear();
            foreach (var student in ListStudent)
            {
                var group = FindGroupById(student.GroupId);
                ListStudentDPO.Add(new StudentDPO
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Birthday = student.Birthday,
                    GroupName = group?.Name ?? "Неизвестно"
                });
            }
            return ListStudentDPO;
        }

        private Group FindGroupById(int groupId)
        {
            var groupVm = new GroupViewModel();
            return groupVm.ListGroup.FirstOrDefault(g => g.Id == groupId);
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var s in ListStudent)
            {
                if (max < s.Id) max = s.Id;
            }
            return max;
        }

        private void SaveChanges(ObservableCollection<Student> listStudents)
        {
            var jsonStudent = JsonConvert.SerializeObject(listStudents, Formatting.Indented);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonStudent);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла\n" + e.Message;
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