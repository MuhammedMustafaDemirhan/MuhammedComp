using MuhammedCo.Core.Models;
using MuhammedCo.Core.Repositories;
using MuhammedCo.Core.Services;
using MuhammedCo.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Service.Services
{
    public class GroupInRoleService : Service<GroupInRole>, IGroupInRoleService
    {
        public GroupInRoleService(IGenericRepository<GroupInRole> repository, IUnitOfWorks unitOfWorks) : base(repository, unitOfWorks)
        {
        }
    }
}
