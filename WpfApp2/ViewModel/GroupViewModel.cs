using System.Collections.ObjectModel;
using WpfApp2.Model;

namespace WpfApp2.ViewModel
{
    public class GroupViewModel
    {
        public ObservableCollection<Group> ListGroup { get; set; }

        public GroupViewModel()
        {
            ListGroup = new ObservableCollection<Group>
            {
                new Group { Id = 1, GroupName = "ИВТ-21", Specialty = "Информатика", Course = 2 },
                new Group { Id = 2, GroupName = "ПР-22", Specialty = "Программирование", Course = 1 },
                new Group { Id = 3, GroupName = "ДС-23", Specialty = "Data Science", Course = 3 }
            };
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var g in ListGroup)
            {
                if (max < g.Id) max = g.Id;
            }
            return max;
        }
    }
}