using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.DTOs;
using ExchangeRateSystem.ServiceCore.DTOs.User;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using ExchangeRateSystem.ServiceCore.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services
{
    public class UserService : IUserService
    {
        public IConfiguration configuration;
        private readonly DatabaseContext dbContext;
        private readonly IMemoryCache cache;
        private readonly ICacheService cacheService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;

        public UserService(IConfiguration configuration, DatabaseContext dbContext, IMemoryCache cache, ICacheService cacheService,
        IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.cache = cache;
            this.cacheService = cacheService;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }


        public Response RegisterUser(RegisterUserDTO model)
        {
            User user = new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email.ToLower();//Because unique constraint
            user.Password = EncryptionUtility.Encrypt(model.Password);
            user.DateInserted = DateTime.Now;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return user.Id > 0 ? Result.Success(user, "Success") : Result.Fail("Problem during user creation");
        }

        public Response LoginUser(LoginUserDTO model)
        {
            var user = AuthenticateUser(model.Email, model.Password);
            if (user.Id > 0)
            {
                var token = GenerateJsonWebToken(user);
                if(token == null || token == "")
                {
                    Result.Fail("Token not created!");
                }

                user.Password = "";//Dont return passwords hash.

                var result = new TokenUserDTO
                {
                    Token = token,
                    User = user
                };
                return Result.Success(result, "Success");
            }
            return  Result.Fail("Wrong credentials!");
        }  
        
        public List<User> GetAll()
        {
            var users = cacheService.GetAll<User>();
            return users;
        }

        public User GetById(int id)
        {
            var user = userRepository.GetUserById(id);  
            if(user != null)
            {
                return user;
            }
            return new User();
        }

        public int CurrentUserId()
        {
            string? userEmail = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Email)?.Value;
            var currentUser = cacheService.GetAll<User>().Where(x => x.Email == userEmail).FirstOrDefault();
            return currentUser != null ? currentUser.Id : 0;
        }
        private User AuthenticateUser(string email, string password)
        {
            var user = userRepository.GetUserByEmail(email);

            if (user != null)
            {
                if (EncryptionUtility.Verify(password, user.Password))
                {
                    return user;
                }
            }
            return new User();
        }

        private string GenerateJsonWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var datetimeNow = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Issuer"],
                claims: claims,
                expires: datetimeNow.AddYears(1), //Temporary, this will be changed to days or months.
                signingCredentials: credentials

                );

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}
