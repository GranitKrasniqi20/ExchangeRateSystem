using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.ServiceCore.DTOs.User;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly ICacheService cacheService;
        private readonly IUserService userService;
        
        public UserController(IConfiguration configuration, ICacheService cacheService, IUserService userService)
        {
            this.configuration = configuration;
            this.cacheService = cacheService;
            this.userService = userService; 
            cacheService.InitCache();//Init cache here ... it is understood that you can delay the login!
        }

        [Route("signUp")]
        [HttpPost]
        public IActionResult RegisterUser([FromBody]RegisterUserDTO model)
        {
            var response = userService.RegisterUser(model);
            return Ok(response);
        }

        [Route("logIn")]
        [HttpPost]
        public IActionResult LoginUser([FromBody] LoginUserDTO model)
        {
            var response = userService.LoginUser(model);
            return Ok(response);
        }
    }
}
