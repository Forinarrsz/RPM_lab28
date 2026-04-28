using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp2.Model
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public double AverageGrade { get; set; }

        // Внешний ключ
        public int GroupId { get; set; }

        // Навигационное свойство для связи с Group
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}