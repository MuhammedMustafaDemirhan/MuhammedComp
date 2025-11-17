using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using MuhammedCo.API.Filters;
using MuhammedCo.Core.DTO_s;
using MuhammedCo.Core.DTOs;
using MuhammedCo.Core.DTOs.UpdateDTOs;
using MuhammedCo.Core.Models;
using MuhammedCo.Core.Services;

namespace MuhammedCo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : CustomBaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Root, Root.Customers, Root.Customers.Get")]
        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var customers = _customerService.GetAll();
            var dtos = _mapper.Map<List<CustomerDto>>(customers).ToList();

            return CreateActionResult(CustomResponseDto<List<CustomerDto>>.Success(200, dtos));
        }

        [Authorize(Roles = "Root, Root.Customers, Root.Customers.Get")]
        [ServiceFilter(typeof(NotFoundFilter<Customer>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return CreateActionResult(CustomResponseDto<CustomerDto>.Success(200, customerDto));
        }

        [Authorize(Roles = "Root, Root.Customers, Root.Customers.Delete")]
        [ServiceFilter(typeof(NotFoundFilter<Customer>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var customer = await _customerService.GetByIdAsync(id);
            customer.UpdatedBy = userId;

            _customerService.ChangeStatus(customer);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Root, Root.Customers, Root.Customers.Add")]
        [HttpPost]
        public async Task<IActionResult> Save(CustomerDto customerDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Customer>(customerDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var customer = await _customerService.AddAsync(processedEntity);

            var customerResponseDto = _mapper.Map<CustomerDto>(customer);

            return CreateActionResult(CustomResponseDto<CustomerDto>.Success(201, customerDto));
        }

        [Authorize(Roles = "Root, Root.Customers, Root.Customers.Update")]
        [HttpPut]
        public async Task<IActionResult> Update(CustomerUpdateDto customerDto)
        {
            var userId = GetUserFromToken();

            var currentCustomer = await _customerService.GetByIdAsync(customerDto.Id);

            currentCustomer.UpdatedBy = userId;
            currentCustomer.Name = customerDto.Name;

            _customerService.Update(currentCustomer);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
