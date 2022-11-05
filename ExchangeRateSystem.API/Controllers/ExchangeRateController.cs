using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using ExchangeRateSystem.ServiceCore.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExchangeRateSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        public IConfiguration configuration;
        private readonly IExchangeRateService exchangeRateService;
        private readonly IExchangeRateRepository exchangeRateRepository;

        public ExchangeRateController(IConfiguration configuration, IExchangeRateService exchangeRateService, IExchangeRateRepository exchangeRateRepository)
        {
            this.configuration = configuration;
            this.exchangeRateService = exchangeRateService;
            this.exchangeRateRepository = exchangeRateRepository;
        }

        [Route("RegisterExchangeRate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public IActionResult RegisterExchangeRate([FromBody] ExchangeRateQueryDTO model)
        {
            var response = exchangeRateService.RegisterExchangeRate(model);
            return Ok(response);
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public List<ExchangeRate> GetAllExchangeRates()
        {
            var exchangeRates = exchangeRateRepository.GetAll();
            return exchangeRates;
        }
    }
}
