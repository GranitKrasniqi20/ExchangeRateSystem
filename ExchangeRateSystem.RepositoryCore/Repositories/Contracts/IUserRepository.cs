using ExchangeRateSystem.EntityCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.RepositoryCore.Repositories.Contracts
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User? GetUserByEmail(string email);
        User? GetUserById(int id);
    }
}
