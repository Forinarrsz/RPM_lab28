using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp2.Helper;
using WpfApp2.Model;
using WpfApp2.View;

namespace WpfApp2.ViewModel
{
    public class GroupViewModel : INotifyPropertyChanged
    {
        private Group selectedGroup;

        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                OnPropertyChanged();
                ((RelayCommand)EditGroup).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteGroup).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Group> ListGroup { get; set; } = new ObservableCollection<Group>();

        public GroupViewModel()
        {
            ListGroup.Add(new Group { Id = 1, Name = "ИВТ-11", Specialty = "Информатика и вычислительная техника", Course = 1 });
            ListGroup.Add(new Group { Id = 2, Name = "ИВТ-12", Specialty = "Информатика и вычислительная техника", Course = 1 });
            ListGroup.Add(new Group { Id = 3, Name = "ПР-11", Specialty = "Прикладная робототехника", Course = 1 });
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var g in ListGroup)
            {
                if (max < g.Id)
                    max = g.Id;
            }
            return max;
        }

        #region AddGroup
        private RelayCommand addGroup;
        public ICommand AddGroup
        {
            get
            {
                return addGroup ?? (addGroup = new RelayCommand(obj =>
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
                    }
                    SelectedGroup = group;
                }));
            }
        }
        #endregion

        #region EditGroup
        private RelayCommand editGroup;
        public ICommand EditGroup
        {
            get
            {
                return editGroup ?? (editGroup = new RelayCommand(obj =>
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
                    }
                }, (obj) => SelectedGroup != null && ListGroup.Count > 0));
            }
        }
        #endregion

        #region DeleteGroup
        private RelayCommand deleteGroup;
        public ICommand DeleteGroup
        {
            get
            {
                return deleteGroup ?? (deleteGroup = new RelayCommand(obj =>
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
                    }
                }, (obj) => SelectedGroup != null && ListGroup.Count > 0));
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