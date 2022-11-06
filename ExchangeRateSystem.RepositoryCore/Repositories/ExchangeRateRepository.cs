using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.RepositoryCore.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly DatabaseContext dbContext;
        public ExchangeRateRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<ExchangeRate> GetAll()
        {
            var exchangeRates = dbContext.ExchangeRates.ToList();
            return exchangeRates;
        }

        public List<ExchangeRate> ExchangeRatesByUserId(int userId)
        {
            var exchangeRates = dbContext.ExchangeRates.Where(a => a.UserId == userId).ToList();
            return exchangeRates;
        }
    }
}
