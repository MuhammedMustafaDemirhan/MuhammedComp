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
    public class GroupsController : CustomBaseController
    {
        private readonly IGroupService _GroupService;
        private readonly IMapper _mapper;

        public GroupsController(IGroupService GroupService, IMapper mapper)
        {
            _GroupService = GroupService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Root, Root.Groups, Root.Groups.Get")]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var Groups = _GroupService.GetAll();
            var dtos = _mapper.Map<List<GroupDto>>(Groups).ToList();

            return CreateActionResult(CustomResponseDto<List<GroupDto>>.Success(200, dtos));
        }

        [Authorize(Roles = "Root, Root.Groups, Root.Groups.Get")]
        [ServiceFilter(typeof(NotFoundFilter<Group>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Group = await _GroupService.GetByIdAsync(id);
            var GroupDto = _mapper.Map<GroupDto>(Group);
            return CreateActionResult(CustomResponseDto<GroupDto>.Success(200, GroupDto));
        }

        [Authorize(Roles = "Root, Root.Groups, Root.Groups.Delete")]
        [ServiceFilter(typeof(NotFoundFilter<Group>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = GetUserFromToken();

            var Group = await _GroupService.GetByIdAsync(id);
            Group.UpdatedBy = userId;

            _GroupService.ChangeStatus(Group);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [Authorize(Roles = "Root, Root.Groups, Root.Groups.Add")]
        [HttpPost]
        public async Task<IActionResult> Save(GroupDto GroupDto)
        {
            int userId = GetUserFromToken();

            var processedEntity = _mapper.Map<Group>(GroupDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            var Group = await _GroupService.AddAsync(processedEntity);

            var GroupResponseDto = _mapper.Map<GroupDto>(Group);

            return CreateActionResult(CustomResponseDto<GroupDto>.Success(201, GroupDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(GroupUpdateDto GroupDto)
        {
            var userId = GetUserFromToken();

            var currentGroup = await _GroupService.GetByIdAsync(GroupDto.Id);

            currentGroup.UpdatedBy = userId;
            currentGroup.Name = GroupDto.Name;

            _GroupService.Update(currentGroup);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
