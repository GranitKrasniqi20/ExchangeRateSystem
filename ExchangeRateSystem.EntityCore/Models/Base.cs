using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.EntityCore.Models
{
    public class Base
    {
        public Base()
        {
            Active = true;
        }
        public int Id { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool Active { get; set; }
    }
}
