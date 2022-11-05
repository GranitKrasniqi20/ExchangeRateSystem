using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate
{
    public class ExchangeRateQueryDTO
    {
        [Required]
        [Range(0.0000000001d, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [RegularExpression(@"^[0-9]\d{0,9}(\.\d{1,10})?%?$", ErrorMessage = "Amount incorrect")]
        public decimal Amount { get; set; }
        [Required]
        public string CurrencyCodeFrom { get; set; }
        [Required]
        public string CurrencyCodeTo { get; set; }
    }
}
