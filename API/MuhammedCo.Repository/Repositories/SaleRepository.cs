using MuhammedCo.Core.Models;
using MuhammedCo.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Repository.Repositories
{
    public class SaleRepository(AppDbContext context):GenericRepository<Sale>(context),ISaleRepository
    {
    }
}
