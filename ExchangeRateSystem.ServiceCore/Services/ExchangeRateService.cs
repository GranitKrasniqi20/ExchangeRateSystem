using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.DTOs;
using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using ExchangeRateSystem.ServiceCore.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        public IConfiguration configuration;
        private readonly DatabaseContext dbContext;
        private readonly ICacheService cacheService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IExchangeRateRepository exchangeRateRepository;
        private readonly IUserService userService;

        public ExchangeRateService(IConfiguration configuration, DatabaseContext dbContext, ICacheService cacheService,
        IHttpContextAccessor httpContextAccessor, IExchangeRateRepository exchangeRateRepository, IUserService userService)
        {
            this.configuration = configuration;
            this.dbContext = dbContext; 
            this.cacheService = cacheService;
            this.httpContextAccessor = httpContextAccessor;
            this.exchangeRateRepository = exchangeRateRepository;
            this.userService = userService;
        }

        public Response RegisterExchangeRate(ExchangeRateQueryDTO model)
        {
            if (model.CurrencyCodeFrom.ToLower() == model.CurrencyCodeTo.ToLower())
            {
                return Result.Fail("Cannot convert amount in same currency codes");
            }

            if (HasMoreTenExchangeRateInLastHour())
            {
                return Result.Fail("You have exceeded the allowed number of currency exchanges per hour");
            }

            var currentUserId = userService.CurrentUserId();

            if (currentUserId == 0)
            {
                return Result.Fail("Problem with the logged in user");
            }

            ExchangeRate exchangeRate;
            if (HasExchangeRateWithTheseCurrencyCodeEarlierThanThirtyMinutes(model.CurrencyCodeFrom, model.CurrencyCodeTo))
            {
                exchangeRate = new ExchangeRate();
                var lastExchangeRate = GetLastExchangeRateByFromAndToCurrencyCode(model.CurrencyCodeFrom, model.CurrencyCodeTo);

                exchangeRate.Amount = model.Amount;//New amount
                exchangeRate.CurrencyCodeFrom = lastExchangeRate.CurrencyCodeFrom;
                exchangeRate.CurrencyCodeTo = lastExchangeRate.CurrencyCodeTo;
                exchangeRate.Success = lastExchangeRate.Success;
                exchangeRate.Date = lastExchangeRate.Date;
                exchangeRate.Result = model.Amount * lastExchangeRate.Rate;//the result is calculated by multiplying new amount and same rate
                exchangeRate.Rate = lastExchangeRate.Rate;
                exchangeRate.TimeStamp = lastExchangeRate.TimeStamp;
            }
            else
            {
                var client = new RestClient(configuration["Fixer:ApiUrl"] +
                $"to={model.CurrencyCodeTo}&from={model.CurrencyCodeFrom}&amount={model.Amount}");

                var request = new RestRequest();
                request.AddHeader("apikey", configuration["Fixer:ApiKey"]);

                var response = client.Execute(request);
                if (!response.IsSuccessful)
                {
                    return Result.Fail("You have exceeded your daily\\/monthly API rate limit." +
                    " Please review and upgrade your subscription plan at https:\\/\\/promptapi.com\\/subscriptions to continue.");
                }

                response.Content += "";
                dynamic api = JObject.Parse(response.Content);

                exchangeRate = new ExchangeRate();
                exchangeRate.Amount = api.query.amount;
                exchangeRate.CurrencyCodeFrom = api.query.from;
                exchangeRate.CurrencyCodeTo = api.query.to;
                exchangeRate.Success = api.success;
                exchangeRate.Date = api.date;
                exchangeRate.Result = api.result;
                exchangeRate.Rate = api.info.rate;
                exchangeRate.TimeStamp = api.info.timestamp;

            }

            exchangeRate.UserId = currentUserId;
            exchangeRate.DateInserted = DateTime.Now;

            dbContext.Add(exchangeRate);
            dbContext.SaveChanges();
            return exchangeRate.Id > 0 ? Result.Success(exchangeRate, "Success") : Result.Fail("Problem during exchange rate creation");

        }

        public bool HasMoreTenExchangeRateInLastHour()
        {
            var currentUserId = userService.CurrentUserId();
            var exchangeRates = exchangeRateRepository.ExchangeRatesByUserId(currentUserId).Where(a => a.DateInserted >= DateTime.Now.AddHours(-1)).ToList();
            var isGretarThanTen = exchangeRates.Count() > 10;
            return isGretarThanTen;
        }


        private bool HasExchangeRateWithTheseCurrencyCodeEarlierThanThirtyMinutes(string currencyCodeFrom, string currencyCodeTo)
        {
            var dateTime = GetDateTimeFromLastExchangeRate(currencyCodeFrom, currencyCodeTo);
            if (DateTime.Now < dateTime.AddMinutes(30))
            {
                return true;
            }
            return false;
        }
        private DateTime GetDateTimeFromLastExchangeRate(string currencyCodeFrom, string currencyCodeTo)
        {
            var lastExchangeRate = GetLastExchangeRateByFromAndToCurrencyCode(currencyCodeFrom, currencyCodeTo);
            if (lastExchangeRate.Id > 0)
            {
                return GeneralHelper.ConvertTimeStampToDate(lastExchangeRate.TimeStamp);
            }
            return new DateTime();
        }

        private ExchangeRate GetLastExchangeRateByFromAndToCurrencyCode(string currencyCodeFrom, string currencyCodeTo)
        {
            //var exchangeRates = exchangeRateRepository.GetAll();
            var exchangeRates = cacheService.GetAll<ExchangeRate>();
            var lastExchangeRate = exchangeRates.Where(a => a.CurrencyCodeFrom == currencyCodeFrom && a.CurrencyCodeTo == currencyCodeTo).OrderByDescending(a => a.Id).FirstOrDefault();
            return lastExchangeRate != null ? lastExchangeRate : new();    
        }

    }
}
