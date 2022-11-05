using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using RestSharp;
using Newtonsoft.Json.Linq;
using ExchangeRateSystem.ServiceCore.Utilities;
using Microsoft.Extensions.Configuration;
using ExchangeRateSystem.ServiceCore.DTOs;

namespace ExchangeRateSystem.Tests.FakeRepositories
{
    public class ExchangeRateRepositoryFake : IExchangeRateRepository, IExchangeRateService
    {
        public IConfiguration configuration;
        public static List<EntityCore.Models.ExchangeRate> exchangeRatesFakeList;

        private static int currentUserId;
        public ExchangeRateRepositoryFake()
        {
            if (exchangeRatesFakeList == null)
            {
                exchangeRatesFakeList = new List<EntityCore.Models.ExchangeRate>();
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 1, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now.AddMinutes(-20) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 2, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667534567", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 2, DateInserted = DateTime.Now.AddMinutes(-18) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 3, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667543256", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 3, DateInserted = DateTime.Now.AddMinutes(-15) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 4, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now.AddMinutes(-12) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 5, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 3, DateInserted = DateTime.Now.AddMinutes(-10) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 6, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now.AddMinutes(-8) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 7, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 2, DateInserted = DateTime.Now.AddMinutes(-5) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 8, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now.AddMinutes(-2) });
                exchangeRatesFakeList.Add(new EntityCore.Models.ExchangeRate { Id = 9, Date = DateTime.Now.AddMinutes(-1), Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = "1667518023", Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now.AddMinutes(-0) });
            }
            currentUserId = 1;
        }
        public List<EntityCore.Models.ExchangeRate> ExchangeRatesByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public List<EntityCore.Models.ExchangeRate> GetAll()
        {
            return exchangeRatesFakeList.ToList();
        }

        public EntityCore.Models.ExchangeRate? GetLastExchangeRate()
        {
            throw new NotImplementedException();
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

            EntityCore.Models.ExchangeRate exchangeRate;

            if (HasExchangeRateWithTheseCurrencyCodeEarlierThanThirtyMinutes(model.CurrencyCodeFrom, model.CurrencyCodeTo))
            {
                exchangeRate = new EntityCore.Models.ExchangeRate();
                var lastExchangeRate = GetLastExchangeRateByFromAndToCurrencyCode(model.CurrencyCodeFrom, model.CurrencyCodeTo);

                exchangeRate.Amount = model.Amount;
                exchangeRate.CurrencyCodeFrom = lastExchangeRate.CurrencyCodeFrom;
                exchangeRate.CurrencyCodeTo = lastExchangeRate.CurrencyCodeTo;
                exchangeRate.Success = lastExchangeRate.Success;
                exchangeRate.Date = lastExchangeRate.Date;
                exchangeRate.Result = model.Amount * lastExchangeRate.Rate;
                exchangeRate.Rate = lastExchangeRate.Rate;
                exchangeRate.TimeStamp = lastExchangeRate.TimeStamp;
            }
            else
            {
                var client = new RestClient($"https://api.apilayer.com/fixer/convert?" +
                $"to={model.CurrencyCodeTo}&from={model.CurrencyCodeFrom}&amount={model.Amount}");

                var request = new RestRequest();
                request.AddHeader("apikey", "0NR09rCLsf3BSvUAnld9yI211CE2vUnn");

                var response = client.Execute(request);
                if (!response.IsSuccessful)
                {
                    return Result.Fail("You have exceeded your daily\\/monthly API rate limit." +
                    "Please review and upgrade your subscription plan at https:\\/\\/promptapi.com\\/subscriptions to continue.");
                }

                response.Content += "";
                dynamic api = JObject.Parse(response.Content);

                exchangeRate = new EntityCore.Models.ExchangeRate();
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

            exchangeRate.Id = exchangeRatesFakeList.Count + 1;
            exchangeRatesFakeList.Add(exchangeRate);
            return Result.Success(exchangeRate, "Success");
        }

        public bool HasMoreTenExchangeRateInLastHour()
        {
            var list = exchangeRatesFakeList.Where(x => x.UserId == currentUserId);
            var exchangeRates = list.Where(a => a.DateInserted >= DateTime.Now.AddHours(-1)).ToList();
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

        private EntityCore.Models.ExchangeRate GetLastExchangeRateByFromAndToCurrencyCode(string currencyCodeFrom, string currencyCodeTo)
        {
            var lastExchangeRate = exchangeRatesFakeList.Where(a => a.CurrencyCodeFrom == currencyCodeFrom && a.CurrencyCodeTo == currencyCodeTo).OrderByDescending(a => a.Id).FirstOrDefault();
            return lastExchangeRate != null ? lastExchangeRate : new();
        }
    }
}
