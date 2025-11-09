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
    public class ProductsController : CustomBaseController
    {
        private readonly IProductService _ProductService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService ProductService, IMapper mapper)
        {
            _ProductService = ProductService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Products = _ProductService.GetAll();
            var dtos = _mapper.Map<List<ProductDto>>(Products).ToList();

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Product = await _ProductService.GetByIdAsync(id);
            var ProductDto = _mapper.Map<ProductDto>(Product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, ProductDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = 1;

            var Product = await _ProductService.GetByIdAsync(id);
            Product.UpdatedBy = userId;

            _ProductService.ChangeStatus(Product);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto ProductDto)
        {
            int userId = 1;

            var processedEntity = _mapper.Map<Product>(ProductDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Product = await _ProductService.AddAsync(processedEntity);

            var ProductResponseDto = _mapper.Map<ProductDto>(Product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, ProductDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto ProductDto)
        {
            var userId = 1;

            var currentProduct = await _ProductService.GetByIdAsync(ProductDto.Id);

            currentProduct.UpdatedBy = userId;
            currentProduct.Name = ProductDto.Name;
            currentProduct.UnitPrice = ProductDto.UnitPrice;

            _ProductService.Update(currentProduct);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
