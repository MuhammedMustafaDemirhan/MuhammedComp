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
    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IGenericRepository<Product> repository, IUnitOfWorks unitOfWorks) : base(repository, unitOfWorks)
        {
        }
    }
}
