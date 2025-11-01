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
    public class CustomerService(IGenericRepository<Customer> repository, IUnitOfWorks unitOfWorks, ICustomerRepository customerRepository) : Service<Customer>(repository, unitOfWorks), ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
    }
}
