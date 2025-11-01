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
    public class DepartmentService : Service<Department>, IDepartmentService
    {
        public DepartmentService(IGenericRepository<Department> repository, IUnitOfWorks unitOfWorks) : base(repository, unitOfWorks)
        {
        }
    }
}
