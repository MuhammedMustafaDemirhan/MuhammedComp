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
    public class RolesController : CustomBaseController
    {
        private readonly IRoleService _RoleService;
        private readonly IMapper _mapper;

        public RolesController(IRoleService RoleService, IMapper mapper)
        {
            _RoleService = RoleService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Roles = _RoleService.GetAll();
            var dtos = _mapper.Map<List<RoleDto>>(Roles).ToList();

            return CreateActionResult(CustomResponseDto<List<RoleDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<Role>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Role = await _RoleService.GetByIdAsync(id);
            var RoleDto = _mapper.Map<RoleDto>(Role);
            return CreateActionResult(CustomResponseDto<RoleDto>.Success(200, RoleDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Role>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Role = await _RoleService.GetByIdAsync(id);
            Role.UpdatedBy = userId;

            _RoleService.ChangeStatus(Role);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(RoleDto RoleDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Role>(RoleDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Role = await _RoleService.AddAsync(processedEntity);

            var RoleResponseDto = _mapper.Map<RoleDto>(Role);

            return CreateActionResult(CustomResponseDto<RoleDto>.Success(201, RoleDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoleUpdateDto RoleDto)
        {
            var userId = GetUserFromToken();

            var currentRole = await _RoleService.GetByIdAsync(RoleDto.Id);

            currentRole.UpdatedBy = userId;
            currentRole.Name = RoleDto.Name;

            _RoleService.Update(currentRole);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
