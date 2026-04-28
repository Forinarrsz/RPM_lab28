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
    public class GroupViewModel : INotifyPropertyChanged
    {
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

        public string Error { get; set; }

        public GroupViewModel()
        {
            ListGroup = GetGroups();
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

                    Group group = new Group();
                    wnGroup.DataContext = group;

                    if (wnGroup.ShowDialog() == true)
                    {
                        using (var context = new AppDbContext())
                        {
                            try
                            {
                                context.Groups.Add(group);
                                context.SaveChanges();
                                ListGroup.Clear();
                                ListGroup = GetGroups();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка добавления данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
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
                    Group editGroup = SelectedGroup;
                    WindowNewGroup wnGroup = new WindowNewGroup
                    {
                        Title = "Редактирование группы",
                        Owner = Application.Current.MainWindow
                    };
                    Group tempGroup = new Group
                    {
                        Id = editGroup.Id,
                        Name = editGroup.Name,
                        Specialty = editGroup.Specialty,
                        Course = editGroup.Course
                    };
                    wnGroup.DataContext = tempGroup;

                    if (wnGroup.ShowDialog() == true)
                    {
                        using (var context = new AppDbContext())
                        {
                            try
                            { 
                                Group group = context.Groups.Find(editGroup.Id);
                                if (group != null)
                                {
                                    if (group.Name != tempGroup.Name)
                                        group.Name = tempGroup.Name?.Trim();
                                    if (group.Specialty != tempGroup.Specialty)
                                        group.Specialty = tempGroup.Specialty?.Trim();
                                    if (group.Course != tempGroup.Course)
                                        group.Course = tempGroup.Course;

                                    context.SaveChanges();
                                    ListGroup.Clear();
                                    ListGroup = GetGroups();
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
                        // Если отменили редактирование — перезагружаем список
                        ListGroup.Clear();
                        ListGroup = GetGroups();
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
                        using (var context = new AppDbContext())
                        {
                            try
                            {
                                // Находим сущность в контексте по Id
                                Group delGroup = context.Groups.Find(group.Id);
                                if (delGroup != null)
                                {
                                    context.Groups.Remove(delGroup);
                                    context.SaveChanges();
                                    ListGroup.Remove(group);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("\nОшибка удаления данных!\n" + ex.Message, "Предупреждение");
                            }
                        }
                    }
                }, (obj) => SelectedGroup != null && ListGroup.Count > 0));
            }
        }

        #endregion

        #region Methods

        private ObservableCollection<Group> GetGroups()
        {
            using (var context = new AppDbContext())
            {
                var query = from g in context.Groups
                            orderby g.Name
                            select g;

                return new ObservableCollection<Group>(query.ToList());
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