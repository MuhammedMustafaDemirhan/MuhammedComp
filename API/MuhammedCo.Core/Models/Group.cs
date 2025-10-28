using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Core.Models
{
    public class Group:BaseEntity
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<GroupInRole> GroupInRoles { get; set; }
    }
}
