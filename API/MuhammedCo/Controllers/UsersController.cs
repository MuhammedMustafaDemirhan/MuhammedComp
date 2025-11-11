using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MuhammedCo.API.Filters;
using MuhammedCo.Core.DTO_s;
using MuhammedCo.Core.DTOs;
using MuhammedCo.Core.DTOs.UpdateDTOs;
using MuhammedCo.Core.Models;
using MuhammedCo.Core.Services;
using MuhammedCo.Service.Hashing;

namespace MuhammedCo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _UserService;
        private readonly IMapper _mapper;

        public UsersController(IUserService UserService, IMapper mapper)
        {
            _UserService = UserService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var Users = _UserService.GetAll();
            var dtos = _mapper.Map<List<UserDto>>(Users).ToList();

            return CreateActionResult(CustomResponseDto<List<UserDto>>.Success(200, dtos));
        }

        [ServiceFilter(typeof(NotFoundFilter<User>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var User = await _UserService.GetByIdAsync(id);
            var UserDto = _mapper.Map<UserDto>(User);
            return CreateActionResult(CustomResponseDto<UserDto>.Success(200, UserDto));
        }

        [ServiceFilter(typeof(NotFoundFilter<User>))]
        [HttpGet("[action]")]
        public async Task<IActionResult> Remove(int id)
        {
            int userId = 1;

            var User = await _UserService.GetByIdAsync(id);
            User.UpdatedBy = userId;

            _UserService.ChangeStatus(User);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost]
        public async Task<IActionResult> Save(UserDto UserDto)
        {
            int userId = 1;

            var processedEntity = _mapper.Map<User>(UserDto);

            processedEntity.UpdatedBy = userId;
            processedEntity.CreatedBy = userId;

            byte[] passwordHash, passwordSalt;

            HashingHelper.CreatePassword(UserDto.Password, out passwordHash, out passwordSalt);

            processedEntity.PasswordHash = passwordHash;
            processedEntity.PasswordSalt = passwordSalt;

            var User = await _UserService.AddAsync(processedEntity);

            var UserResponseDto = _mapper.Map<UserDto>(User);

            return CreateActionResult(CustomResponseDto<UserDto>.Success(201, UserDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserUpdateDto UserDto)
        {
            var userId = 1;

            var currentUser = await _UserService.GetByIdAsync(UserDto.Id);

            currentUser.UpdatedBy = userId;
            currentUser.Name = UserDto.Name;
            currentUser.DepartmentId = UserDto.DepartmentId;
            currentUser.GroupId = UserDto.GroupId;

            _UserService.Update(currentUser);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            Token token = await _UserService.Login(userLoginDto);

            if (token == null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(401, "Bilgiler Uyuşmuyor"));
            }
            return CreateActionResult(CustomResponseDto<Token>.Success(200, token));
        }
    }
}
