using ExchangeRateSystem.ServiceCore.DTOs;
using ExchangeRateSystem.ServiceCore.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate
{
    public class ExchangeRateDTO : BaseDTO
    {
        public DateTime Date { get; set; }
        public bool Success { get; set; }
        public decimal Result { get; set; }
        public decimal Rate { get; set; }
        public string TimeStamp { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCodeFrom { get; set; }
        public string CurrencyCodeTo { get; set; }
        public int UserId { get; set; }

        public UserDTO User { get; set; }
    }
}
