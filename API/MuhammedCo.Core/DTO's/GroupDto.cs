using MuhammedCo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Core.DTO_s
{
    public class GroupDto:BaseDto
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
