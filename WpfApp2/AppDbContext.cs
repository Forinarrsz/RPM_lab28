using System.Data.Entity;
using WpfApp2.Model;

namespace WpfApp2
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=AppDbContext")
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }

    }
}