using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfApp2.Model
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Specialty { get; set; }
        public int Course { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public Group()
        {
            Students = new HashSet<Student>();
        }
    }
}