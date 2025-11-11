using MuhammedCo.Core.DTOs;
using MuhammedCo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuhammedCo.Core.Services
{
    public interface IUserService:IService<User>
    {
        User GetByEmail(string email);
        Task<Token> Login(UserLoginDto userLoginDto);
    }
}
