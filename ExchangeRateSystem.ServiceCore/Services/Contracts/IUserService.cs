using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.ServiceCore.DTOs;
using ExchangeRateSystem.ServiceCore.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services.Contracts
{
    public interface IUserService
    {
        Response RegisterUser(RegisterUserDTO model);
        Response LoginUser(LoginUserDTO model);
        List<User> GetAll();
        User GetById(int id);
        int CurrentUserId();
    }
}
