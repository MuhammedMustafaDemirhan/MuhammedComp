using MuhammedCo.Core.DTOs;
using MuhammedCo.Core.Models;
using MuhammedCo.Core.Repositories;
using MuhammedCo.Core.Services;
using MuhammedCo.Core.UnitOfWorks;
using MuhammedCo.Service.Hashing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Service.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _tokenHandler;
        public UserService(IGenericRepository<User> repository, IUnitOfWorks unitOfWorks, IUserRepository userRepository, ITokenHandler tokenHandler) : base(repository, unitOfWorks)
        {
            _userRepository = userRepository;
            _tokenHandler = tokenHandler;
        }

        public User GetByEmail(string email)
        {
            User user = _userRepository.Where(x => x.Email == email).FirstOrDefault();

            return user ?? user;
        }

        public async Task<Token> Login(UserLoginDto userLoginDto)
        {
            Token token = new Token();

            var user = GetByEmail(userLoginDto.Email);

            if (user == null)
                return null;

            var result = false;

            result = HashingHelper.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt);

            List<Role> roles = new List<Role>();

            if(result)
            {
                token = _tokenHandler.CreateToken(user, roles);
                return token;
            }
            return null;
        }
    }
}
