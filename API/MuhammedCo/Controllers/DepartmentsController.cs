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
    public class DepartmentsController : CustomBaseController
    {
        private readonly IDepartmentService _DepartmentService;
        private readonly IMapper _mapper;

        public DepartmentsController(IDepartmentService DepartmentService, IMapper mapper)
        {
            _DepartmentService = DepartmentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Root, Root.Departments, Root.Departments.Get")]
        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Departments = _DepartmentService.GetAll();
            var dtos = _mapper.Map<List<DepartmentDto>>(Departments).ToList();

            return CreateActionResult(CustomResponseDto<List<DepartmentDto>>.Success(200, dtos));
        }

        [Authorize(Roles = "Root, Root.Departments, Root.Departments.Get")]
        [ServiceFilter(typeof(NotFoundFilter<Department>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Department = await _DepartmentService.GetByIdAsync(id);
            var DepartmentDto = _mapper.Map<DepartmentDto>(Department);
            return CreateActionResult(CustomResponseDto<DepartmentDto>.Success(200, DepartmentDto));
        }

        [Authorize(Roles = "Root, Root.Departments, Root.Departments.Delete")]
        [ServiceFilter(typeof(NotFoundFilter<Department>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Department = await _DepartmentService.GetByIdAsync(id);
            Department.UpdatedBy = userId;

            _DepartmentService.ChangeStatus(Department);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Root, Root.Departments, Root.Departments.Add")]
        [HttpPost]
        public async Task<IActionResult> Save(DepartmentDto DepartmentDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Department>(DepartmentDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Department = await _DepartmentService.AddAsync(processedEntity);

            var DepartmentResponseDto = _mapper.Map<DepartmentDto>(Department);

            return CreateActionResult(CustomResponseDto<DepartmentDto>.Success(201, DepartmentDto));
        }

        [Authorize(Roles = "Root, Root.Departments, Root.Departments.Update")]
        [HttpPut]
        public async Task<IActionResult> Update(DepartmentUpdateDto DepartmentDto)
        {
            var userId = GetUserFromToken();

            var currentDepartment = await _DepartmentService.GetByIdAsync(DepartmentDto.Id);

            currentDepartment.UpdatedBy = userId;
            currentDepartment.Name = DepartmentDto.Name;

            _DepartmentService.Update(currentDepartment);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
