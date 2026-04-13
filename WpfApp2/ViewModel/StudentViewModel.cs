using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private StudentDPO selectedStudentDPO;

        public StudentDPO SelectedStudentDPO
        {
            get { return selectedStudentDPO; }
            set
            {
                selectedStudentDPO = value;
                OnPropertyChanged("SelectedStudentDPO");
            }
        }

        public ObservableCollection<Student> ListStudent { get; set; } = new ObservableCollection<Student>();
        public ObservableCollection<StudentDPO> ListStudentDPO { get; set; } = new ObservableCollection<StudentDPO>();

        public StudentViewModel()
        {
            ListStudent.Add(new Student { Id = 1, GroupId = 1, FirstName = "Иван", LastName = "Иванов", Birthday = new DateTime(2000, 1, 15) });
            ListStudent.Add(new Student { Id = 2, GroupId = 2, FirstName = "Петр", LastName = "Петров", Birthday = new DateTime(2001, 2, 20) });
            ListStudent.Add(new Student { Id = 3, GroupId = 1, FirstName = "Сидор", LastName = "Сидоров", Birthday = new DateTime(2000, 3, 10) });

            UpdateStudentDPOList();
        }

        private void UpdateStudentDPOList()
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
                if (max < s.Id)
                    max = s.Id;
            }
            return max;
        }

        #region AddStudent
        private RelayCommand addStudent;
        public RelayCommand AddStudent
        {
            get
            {
                return addStudent ?? (addStudent = new RelayCommand(obj =>
                {
                    WindowNewStudent wnStudent = new WindowNewStudent
                    {
                        Title = "Новый студент"
                    };
                    int maxId = MaxId() + 1;
                    StudentDPO studentDPO = new StudentDPO
                    {
                        Id = maxId,
                        Birthday = DateTime.Now
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
                        }
                    }
                }));
            }
        }
        #endregion

        #region EditStudent
        private RelayCommand editStudent;
        public RelayCommand EditStudent
        {
            get
            {
                return editStudent ?? (editStudent = new RelayCommand(obj =>
                {
                    WindowNewStudent wnStudent = new WindowNewStudent
                    {
                        Title = "Редактирование студента"
                    };
                    StudentDPO studentDPO = SelectedStudentDPO;
                    StudentDPO tempStudent = new StudentDPO
                    {
                        Id = studentDPO.Id,
                        FirstName = studentDPO.FirstName,
                        LastName = studentDPO.LastName,
                        Birthday = studentDPO.Birthday,
                        GroupName = studentDPO.GroupName
                    };
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
                        }
                    }
                }, (obj) => SelectedStudentDPO != null && ListStudentDPO.Count > 0));
            }
        }
        #endregion

        #region DeleteStudent
        private RelayCommand deleteStudent;
        public RelayCommand DeleteStudent
        {
            get
            {
                return deleteStudent ?? (deleteStudent = new RelayCommand(obj =>
                {
                    StudentDPO student = SelectedStudentDPO;
                    MessageBoxResult result = MessageBox.Show(
                        "Удалить данные по студенту:\n" + student.LastName + " " + student.FirstName,
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
                        }
                    }
                }, (obj) => SelectedStudentDPO != null && ListStudentDPO.Count > 0));
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