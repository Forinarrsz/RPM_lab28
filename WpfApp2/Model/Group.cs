using System;

namespace WpfApp2.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Specialty { get; set; }
        public int Course { get; set; }

       
        public Group ShallowCopy()
        {
            return (Group)this.MemberwiseClone();
        }

        public override string ToString() => GroupName;
    }
}