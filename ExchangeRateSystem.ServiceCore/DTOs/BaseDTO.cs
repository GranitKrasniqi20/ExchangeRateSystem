using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs
{
    public class BaseDTO
    {
        public int Id { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool Active { get; set; }
    }
}
