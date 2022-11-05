using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.ServiceCore.DTOs;
using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services.Contracts
{
    public interface IExchangeRateService
    {
        Response RegisterExchangeRate(ExchangeRateQueryDTO model);
        bool HasMoreTenExchangeRateInLastHour();
    }
}
