using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Model; // Убедитесь, что пространство имен Model подключено

namespace WpfApp2.Model
{
    public class StudentDPO : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private string middleName;
        public string Birthday { get; set; }
        private string groupName;
        private double averageGrade;

        public int Id { get; set; }
        public int GroupId { get; set; }

        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        public string MiddleName
        {
            get => middleName;
            set { middleName = value; OnPropertyChanged(); }
        }

        public DateTime Birthday
        {
            get => birthday;
            set { birthday = value; OnPropertyChanged(); }
        }

        public string GroupName
        {
            get => groupName;
            set { groupName = value; OnPropertyChanged(); }
        }

        public double AverageGrade
        {
            get => averageGrade;
            set { averageGrade = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public StudentDPO ShallowCopy()
        {
            return (StudentDPO)this.MemberwiseClone();
        }
        public void CopyFromStudent(Student student)
        {
            this.Id = student.Id;
            this.GroupId = student.GroupId;
            this.FirstName = student.FirstName;
            this.LastName = student.LastName;
            this.MiddleName = student.MiddleName;
            this.Birthday = student.Birthday;
            this.AverageGrade = student.AverageGrade;

            // Примечание: GroupName обычно заполняется отдельно через ViewModel, 
            // так как Student хранит только GroupId.

        }
    }
}