using ExchangeRateSystem.ServiceCore.DTOs.ExchangeRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs.User
{
    public class UserDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ExchangeRateDTO> ExchangeRates { get; set; }
    }
}
