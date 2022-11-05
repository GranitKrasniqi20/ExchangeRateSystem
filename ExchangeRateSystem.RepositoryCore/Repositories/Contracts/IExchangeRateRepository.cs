using ExchangeRateSystem.EntityCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.RepositoryCore.Repositories.Contracts
{
    public interface IExchangeRateRepository
    {
        List<ExchangeRate> GetAll();
        ExchangeRate? GetLastExchangeRate();
        List<ExchangeRate> ExchangeRatesByUserId(int userId);
    }
}
