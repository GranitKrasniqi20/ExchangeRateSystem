using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.RepositoryCore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext dbContext;
        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext; 
        }
        public List<User> GetAll()
        {
            var users = dbContext.Users.ToList();
            return users;
        }

        public User? GetUserByEmail(string email)
        {
            var user = dbContext.Users
                .Include(a=>a.ExchangeRates)
                .Where(a => a.Email == email.ToLower()).FirstOrDefault();
            return user;
        }

        public User? GetUserById(int id)
        {
            var user = dbContext.Users
                .Include(a => a.ExchangeRates)
                .Where(a => a.Id == id).FirstOrDefault();
            return user;
        }
    }
}
