using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.EntityCore.Models
{
    public class User : Base
    {
        public User()
        {
            ExchangeRates = new HashSet<ExchangeRate>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ExchangeRate> ExchangeRates { get; set; }
    }
}
