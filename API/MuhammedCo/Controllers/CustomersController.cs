using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuhammedCo.Core.DTO_s;
using MuhammedCo.Core.DTOs;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var customers = _customerService.GetAll();
            var dtos = _mapper.Map<List<CustomerDto>>(customers).ToList();

            return CreateActionResult(CustomResponseDto<List<CustomerDto>>.Success(200, dtos));
        }
    }
}
