using System;
using System.Linq;
using WpfApp2.ViewModel;

namespace WpfApp2.Model
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int GroupId { get; set; }
        public DateTime BirthDate { get; set; }
        public double AverageGrade { get; set; }

        public Student CopyFromStudentDPO(StudentDPO p)
        {
            if (p == null) return this;

            var vmGroup = new GroupViewModel();
            var group = vmGroup.ListGroup?.FirstOrDefault(g => g.GroupName == p.GroupName);

            if (group != null)
            {
                this.Id = p.Id;
                this.GroupId = group.Id;
                this.FirstName = p.FirstName;
                this.LastName = p.LastName;
                this.MiddleName = p.MiddleName;
                this.BirthDate = p.BirthDate;
                this.AverageGrade = p.AverageGrade;
            }
            return this;
        }
        public Student ShallowCopy()
        {
            return (Student)this.MemberwiseClone();
        }
    }
}