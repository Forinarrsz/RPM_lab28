using System.Collections.ObjectModel;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class StudentViewModel
    {
        public ObservableCollection<Student> ListStudent { get; set; }

        public StudentViewModel()
        {
            ListStudent = new ObservableCollection<Student>
            {
                new Student { Id = 1, LastName = "Сидоров", FirstName = "Алексей", MiddleName = "Дмитриевич", GroupId = 1, BirthDate = new DateTime(2003, 3, 15), AverageGrade = 4.5 },
                new Student { Id = 2, LastName = "Козлова", FirstName = "Мария", MiddleName = "Сергеевна", GroupId = 2, BirthDate = new DateTime(2004, 7, 22), AverageGrade = 4.8 },
                new Student { Id = 3, LastName = "Новиков", FirstName = "Иван", MiddleName = "Петрович", GroupId = 1, BirthDate = new DateTime(2002, 11, 8), AverageGrade = 4.2 }
            };
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
    }
}