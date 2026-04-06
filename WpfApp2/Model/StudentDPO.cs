using System;
using System.Linq;
using WpfApp2.ViewModel;

namespace WpfApp2.Model
{
    public class StudentDPO
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string GroupName { get; set; } 
        public string Specialty { get; set; }
        public int Course { get; set; }
        public DateTime BirthDate { get; set; }
        public double AverageGrade { get; set; }

        public StudentDPO CopyFromStudent(Student student)
        {
            if (student == null) return null;

            var result = new StudentDPO();
            var vmGroup = new GroupViewModel();
            var group = vmGroup.ListGroup?.FirstOrDefault(g => g.Id == student.GroupId);

            if (group != null)
            {
                result.Id = student.Id;
                result.GroupName = group.GroupName;
                result.Specialty = group.Specialty;
                result.Course = group.Course;
                result.FirstName = student.FirstName;
                result.LastName = student.LastName;
                result.MiddleName = student.MiddleName;
                result.BirthDate = student.BirthDate;
                result.AverageGrade = student.AverageGrade;
            }
            return result;
        }

        public StudentDPO ShallowCopy()
        {
            return (StudentDPO)this.MemberwiseClone();
        }
    }
}