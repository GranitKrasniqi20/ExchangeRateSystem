using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate
{
    public class ExchangeRateInfoDTO
    {
        public decimal Rate { get; set; }
        public string TimeStamp = DateTime.Now.ToString();
    }
}
