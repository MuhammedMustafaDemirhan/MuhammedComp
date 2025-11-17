using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Root, Root.Products, Root.Products.Get")]
        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Products = _ProductService.GetAll();
            var dtos = _mapper.Map<List<ProductDto>>(Products).ToList();

            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, dtos));
        }

        [Authorize(Roles = "Root, Root.Products, Root.Products.Get")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Product = await _ProductService.GetByIdAsync(id);
            var ProductDto = _mapper.Map<ProductDto>(Product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, ProductDto));
        }

        [Authorize(Roles = "Root, Root.Products, Root.Products.Delete")]
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Product = await _ProductService.GetByIdAsync(id);
            Product.UpdatedBy = userId;

            _ProductService.ChangeStatus(Product);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Root, Root.Products, Root.Products.Add")]
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto ProductDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Product>(ProductDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Product = await _ProductService.AddAsync(processedEntity);

            var ProductResponseDto = _mapper.Map<ProductDto>(Product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, ProductDto));
        }

        [Authorize(Roles = "Root, Root.Products, Root.Products.BuyProduct")]
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyProduct(ProductDto ProductDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Product>(ProductDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            await _ProductService.BuyProduct(processedEntity);

            var ProductResponseDto = _mapper.Map<Product>(ProductDto);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Root, Root.Products, Root.Products.Update")]
        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto ProductDto)
        {
            var userId = GetUserFromToken();

            var currentProduct = await _ProductService.GetByIdAsync(ProductDto.Id);

            currentProduct.UpdatedBy = userId;
            currentProduct.Name = ProductDto.Name;
            currentProduct.UnitPrice = ProductDto.UnitPrice;

            _ProductService.Update(currentProduct);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
