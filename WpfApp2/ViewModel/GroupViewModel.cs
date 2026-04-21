using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class GroupViewModel : INotifyPropertyChanged
    {
       
        readonly string path = @"C:\Users\User\source\repos\Forinarrsz\RPM_lab28\WpfApp2\DataModels\GroupData.json";

        public ObservableCollection<Group> ListGroup { get; set; } = new ObservableCollection<Group>();

        private Group _selectedGroup;
        public Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged();
                EditGroup?.RaiseCanExecuteChanged();
                DeleteGroup?.RaiseCanExecuteChanged();
            }
        }

        string _jsonGroups = String.Empty;
        public string Error { get; set; }

        public GroupViewModel()
        {
            ListGroup = LoadGroup();
        }

        #region Commands

        private RelayCommand _addGroup;
        public RelayCommand AddGroup
        {
            get
            {
                return _addGroup ?? (_addGroup = new RelayCommand(obj =>
                {
                    WindowNewGroup wnGroup = new WindowNewGroup
                    {
                        Title = "Новая группа",
                        Owner = Application.Current.MainWindow
                    };

                    int maxId = MaxId() + 1;
                    Group group = new Group { Id = maxId };
                    wnGroup.DataContext = group;

                    if (wnGroup.ShowDialog() == true)
                    {
                        ListGroup.Add(group);
                        SaveChanges(ListGroup);
                    }
                    SelectedGroup = group;
                }, (obj) => true));
            }
        }

        private RelayCommand _editGroup;
        public RelayCommand EditGroup
        {
            get
            {
                return _editGroup ?? (_editGroup = new RelayCommand(obj =>
                {
                    WindowNewGroup wnGroup = new WindowNewGroup
                    {
                        Title = "Редактирование группы",
                        Owner = Application.Current.MainWindow
                    };

                    Group group = SelectedGroup;
                    Group tempGroup = group?.ShallowCopy();
                    wnGroup.DataContext = tempGroup;

                    if (wnGroup.ShowDialog() == true)
                    {
                        group.Name = tempGroup.Name;
                        group.Specialty = tempGroup.Specialty;
                        group.Course = tempGroup.Course;
                        SaveChanges(ListGroup);
                    }
                }, (obj) => SelectedGroup != null && ListGroup.Count > 0));
            }
        }

        private RelayCommand _deleteGroup;
        public RelayCommand DeleteGroup
        {
            get
            {
                return _deleteGroup ?? (_deleteGroup = new RelayCommand(obj =>
                {
                    Group group = SelectedGroup;
                    MessageBoxResult result = MessageBox.Show(
                        $"Удалить данные по группе: {group.Name}?",
                        "Предупреждение",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.OK)
                    {
                        ListGroup.Remove(group);
                        SaveChanges(ListGroup);
                    }
                }, (obj) => SelectedGroup != null && ListGroup.Count > 0));
            }
        }

        #endregion

        #region Methods

        public ObservableCollection<Group> LoadGroup()
        {
            try
            {
                _jsonGroups = File.ReadAllText(path);
                if (_jsonGroups != null)
                {
                    ListGroup = JsonConvert.DeserializeObject<ObservableCollection<Group>>(_jsonGroups);
                    return ListGroup;
                }
            }
            catch (Exception e)
            {
                Error = "Ошибка чтения JSON файла:\n" + e.Message;
            }
            return new ObservableCollection<Group>();
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var g in ListGroup)
            {
                if (max < g.Id) max = g.Id;
            }
            return max;
        }

        private void SaveChanges(ObservableCollection<Group> listGroup)
        {
            var jsonGroup = JsonConvert.SerializeObject(listGroup, Formatting.Indented);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonGroup);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла\n" + e.Message;
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

//save