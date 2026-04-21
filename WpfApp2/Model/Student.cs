using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2.Model
{
    public class Student : INotifyPropertyChanged
    {
        private string firstName;
        private string lastName;
        private string middleName;
        public string birthday { get; set; }
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
        public void CopyFromStudentDPO(StudentDPO dpo)
        {
            this.Id = dpo.Id;
            this.GroupId = dpo.GroupId;
            this.FirstName = dpo.FirstName;
            this.LastName = dpo.LastName;
            this.MiddleName = dpo.MiddleName;
            this.birthday = dpo.birthday;
            this.AverageGrade = dpo.AverageGrade;
        }

        public void CopyFromStudent(Student other)
        {
            this.Id = other.Id;
            this.GroupId = other.GroupId;
            this.FirstName = other.FirstName;
            this.LastName = other.LastName;
            this.MiddleName = other.MiddleName;
            this.birthday = other.birthday;
            this.AverageGrade = other.AverageGrade;
        }
    }
}