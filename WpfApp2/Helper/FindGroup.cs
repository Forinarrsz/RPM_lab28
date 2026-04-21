using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Model;

namespace WpfApp2.Helper
{
    public class FindGroup
    {
        private readonly int _groupId;

        public FindGroup(int groupId)
        {
            _groupId = groupId;
        }

        public bool Find(Group group)
        {
            return group.Id == _groupId;
        }
    }
}
