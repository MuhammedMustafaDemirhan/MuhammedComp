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
    public class SalesController : CustomBaseController
    {
        private readonly ISaleService _SaleService;
        private readonly IMapper _mapper;

        public SalesController(ISaleService SaleService, IMapper mapper)
        {
            _SaleService = SaleService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Sales = _SaleService.GetAll();
            var dtos = _mapper.Map<List<SaleDto>>(Sales).ToList();

            return CreateActionResult(CustomResponseDto<List<SaleDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Sale>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Sale = await _SaleService.GetByIdAsync(id);
            var SaleDto = _mapper.Map<SaleDto>(Sale);
            return CreateActionResult(CustomResponseDto<SaleDto>.Success(200, SaleDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Sale>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Sale = await _SaleService.GetByIdAsync(id);
            Sale.UpdatedBy = userId;

            _SaleService.ChangeStatus(Sale);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(SaleDto SaleDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Sale>(SaleDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Sale = await _SaleService.AddAsync(processedEntity);

            var SaleResponseDto = _mapper.Map<SaleDto>(Sale);

            return CreateActionResult(CustomResponseDto<SaleDto>.Success(201, SaleDto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SaleProduct(SaleDto SaleDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Sale>(SaleDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            await _SaleService.SaleProduct(processedEntity);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SaleUpdateDto SaleDto)
        {
            var userId = GetUserFromToken();

            var currentSale = await _SaleService.GetByIdAsync(SaleDto.Id);

            currentSale.UpdatedBy = userId;
            currentSale.CustomerId = SaleDto.CustomerId;
            currentSale.ProductId = SaleDto.ProductId;
            currentSale.Quantity = SaleDto.Quantity;
            currentSale.UnitPrice = SaleDto.UnitPrice;
            currentSale.TotalPrice = SaleDto.TotalPrice;

            _SaleService.Update(currentSale);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
