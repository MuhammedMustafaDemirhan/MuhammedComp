using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Core.DTO_s
{
    public class UserDto:BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public int GroupId { get; set; }
        public DepartmentDto Department { get; set; }
        public GroupDto Group { get; set; }
    }
}
