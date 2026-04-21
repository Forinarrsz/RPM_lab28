using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2.Model
{
    public class Group : INotifyPropertyChanged
    {
        private string name;
        private string specialty;
        private int course;

        public int Id { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GroupName));
            }
        }

        public string GroupName
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Specialty
        {
            get => specialty;
            set
            {
                specialty = value;
                OnPropertyChanged();
            }
        }

        public int Course
        {
            get => course;
            set
            {
                course = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Group ShallowCopy()
        {
            return (Group)this.MemberwiseClone();
        }
    }
}