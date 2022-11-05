using ExchangeRateSystem.ServiceCore.DTOs.User;
using ExchangeRateSystem.EntityCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs.User
{
    public class TokenUserDTO
    {
        public string Token { get; set; }
        public ExchangeRateSystem.EntityCore.Models.User User { get; set; }
    }
}
