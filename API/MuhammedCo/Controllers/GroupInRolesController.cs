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
    public class GroupInRolesController : CustomBaseController
    {
        private readonly IGroupInRoleService _GroupInRoleService;
        private readonly IMapper _mapper;

        public GroupInRolesController(IGroupInRoleService GroupInRoleService, IMapper mapper)
        {
            _GroupInRoleService = GroupInRoleService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var GroupInRoles = _GroupInRoleService.GetAll();
            var dtos = _mapper.Map<List<GroupInRoleDto>>(GroupInRoles).ToList();

            return CreateActionResult(CustomResponseDto<List<GroupInRoleDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<GroupInRole>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var GroupInRole = await _GroupInRoleService.GetByIdAsync(id);
            var GroupInRoleDto = _mapper.Map<GroupInRoleDto>(GroupInRole);
            return CreateActionResult(CustomResponseDto<GroupInRoleDto>.Success(200, GroupInRoleDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<GroupInRole>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = 1;

            var GroupInRole = await _GroupInRoleService.GetByIdAsync(id);
            GroupInRole.UpdatedBy = userId;

            _GroupInRoleService.ChangeStatus(GroupInRole);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(GroupInRoleDto GroupInRoleDto)
        {
            int userId = 1;

            var processedEntity = _mapper.Map<GroupInRole>(GroupInRoleDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var GroupInRole = await _GroupInRoleService.AddAsync(processedEntity);

            var GroupInRoleResponseDto = _mapper.Map<GroupInRoleDto>(GroupInRole);

            return CreateActionResult(CustomResponseDto<GroupInRoleDto>.Success(201, GroupInRoleDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(GroupInRoleUpdateDto GroupInRoleDto)
        {
            var userId = 1;

            var currentGroupInRole = await _GroupInRoleService.GetByIdAsync(GroupInRoleDto.Id);

            currentGroupInRole.UpdatedBy = userId;
            currentGroupInRole.GroupId = GroupInRoleDto.GroupId;
            currentGroupInRole.RoleId = GroupInRoleDto.RoleId;

            _GroupInRoleService.Update(currentGroupInRole);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
