using ExchangeRateSystem.API.Controllers;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using ExchangeRateSystem.ServiceCore.Services;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using ExchangeRateSystem.ServiceCore.Utilities;
using ExchangeRateSystem.Tests.FakeRepositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRate.Tests
{
    public class ExchangeRateControllerFakeTest
    {
        public IConfiguration configuration;
        private readonly ExchangeRateController exchangeRateController;
        private readonly IExchangeRateRepository exchangeRateRepository;
        private readonly IExchangeRateService exchangeRateService;
        private static int currentUserId = 0;
        Random random = new Random();
        List<string> currencyCodes;

        public ExchangeRateControllerFakeTest()
        {
            exchangeRateService = new ExchangeRateRepositoryFake();
            exchangeRateRepository = new ExchangeRateRepositoryFake();
            exchangeRateController = new ExchangeRateController(null, exchangeRateService, exchangeRateRepository);

            currentUserId = 1; //supposed current user id
        }


        [Fact]
        public void RegisteExchangeRate_MustBeAddedNewRecord()
        {
            var listBeforeTest = exchangeRateController.GetAllExchangeRates().ToList();

            ExchangeRateQueryDTO exchangeRateQueryDTO = new ExchangeRateQueryDTO();

            exchangeRateQueryDTO.Amount = (decimal)(random.Next(1, 100000) * random.NextDouble());

            currencyCodes = new List<string> { "eur", "usd", "jpy", "gbp", "chf", "cad" };

            var positionElementFrom = random.Next(0, currencyCodes.Count);
            exchangeRateQueryDTO.CurrencyCodeFrom = currencyCodes[positionElementFrom];
            currencyCodes.RemoveAt(positionElementFrom);
           
            var positionElementTo = random.Next(0, currencyCodes.Count);
            exchangeRateQueryDTO.CurrencyCodeTo = currencyCodes[positionElementTo];

            var actionResult = exchangeRateController.RegisterExchangeRate(exchangeRateQueryDTO);
            var result = actionResult;
            var listAfterTest = exchangeRateController.GetAllExchangeRates().ToList();

            Assert.NotNull(result);
            Assert.Equal(listBeforeTest?.Count(), listAfterTest?.Count()-1);
            Assert.NotEqual(listBeforeTest?.Count(), listAfterTest?.Count());
        }

        [Fact]
        public void RegisteExchangeRate_MustBeAddedNewRecordWithoutApiCall()
        {
            //ExchangeRateRepositoryFake exchangeRateRepositoryFake = new ExchangeRateRepositoryFake();
            var listBeforeTest = exchangeRateController.GetAllExchangeRates().ToList();

            var timeStampNow = GeneralHelper.ConvertDateToTimestamp(DateTime.UtcNow);
            var count = ExchangeRateRepositoryFake.exchangeRatesFakeList.Count();
            var newExchangeRate = new ExchangeRateSystem.EntityCore.Models.ExchangeRate { Id = count + 1, Date = DateTime.Now, Success = true, Result = (decimal)100.1, Rate = (decimal)0.2, TimeStamp = timeStampNow, Amount = (decimal)78.34, CurrencyCodeFrom = "eur", CurrencyCodeTo = "usd", UserId = 1, DateInserted = DateTime.Now };
            ExchangeRateRepositoryFake.exchangeRatesFakeList.Add(newExchangeRate);//Add new record now, so certainly earlier than 30 minutes
            

            ExchangeRateQueryDTO exchangeRateQueryDTO = new ExchangeRateQueryDTO();
            exchangeRateQueryDTO.Amount = (decimal)(random.Next(1, 100000) * random.NextDouble());

            //CurrencyCodes should be the same as CurrencyCodes of new record (newExhangeRate)
            exchangeRateQueryDTO.CurrencyCodeFrom = newExchangeRate.CurrencyCodeFrom;
            exchangeRateQueryDTO.CurrencyCodeTo = newExchangeRate.CurrencyCodeTo;

            var actionResult = exchangeRateController.RegisterExchangeRate(exchangeRateQueryDTO);
            var result = actionResult;
            var listAfterTest = exchangeRateController.GetAllExchangeRates().ToList();

            Assert.NotNull(result);
            Assert.Equal(listBeforeTest?.Count(), listAfterTest?.Count() - 2);
            Assert.NotEqual(listBeforeTest?.Count(), listAfterTest?.Count() - 1);
            Assert.NotEqual(listBeforeTest?.Count(), listAfterTest?.Count());
        }

    }
}
