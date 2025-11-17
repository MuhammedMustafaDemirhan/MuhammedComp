using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class PaymentsController : CustomBaseController
    {
        private readonly IPaymentService _PaymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService PaymentService, IMapper mapper)
        {
            _PaymentService = PaymentService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Payments = _PaymentService.GetAll();
            var dtos = _mapper.Map<List<PaymentDto>>(Payments).ToList();

            return CreateActionResult(CustomResponseDto<List<PaymentDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Payment>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Payment = await _PaymentService.GetByIdAsync(id);
            var PaymentDto = _mapper.Map<PaymentDto>(Payment);
            return CreateActionResult(CustomResponseDto<PaymentDto>.Success(200, PaymentDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Payment>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Payment = await _PaymentService.GetByIdAsync(id);
            Payment.UpdatedBy = userId;

            _PaymentService.ChangeStatus(Payment);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(PaymentDto PaymentDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Payment>(PaymentDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Payment = await _PaymentService.AddAsync(processedEntity);

            var PaymentResponseDto = _mapper.Map<PaymentDto>(Payment);

            return CreateActionResult(CustomResponseDto<PaymentDto>.Success(201, PaymentDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PaymentUpdateDto PaymentDto)
        {
            var userId = GetUserFromToken();

            var currentPayment = await _PaymentService.GetByIdAsync(PaymentDto.Id);

            currentPayment.UpdatedBy = userId;
            currentPayment.CustomerId = PaymentDto.CustomerId;
            currentPayment.Amount = PaymentDto.Amount;

            _PaymentService.Update(currentPayment);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
