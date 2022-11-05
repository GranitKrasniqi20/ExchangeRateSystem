using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExchangeRateSystem.EntityCore.Models
{
    public class ExchangeRate : Base
    {
        public DateTime Date {get; set;}
        public bool Success { get; set; }
        public decimal Result { get; set; }
        public decimal Rate { get; set; }
        public string TimeStamp { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCodeFrom { get; set; }
        public string CurrencyCodeTo { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
    }
}
