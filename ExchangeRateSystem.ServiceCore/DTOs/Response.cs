using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.DTOs
{
    public class Response
    {
        public static object Headers { get; internal set; }
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }

    public enum ResponseStatus
    {
        Success,
        Fail,
        Redirect
    }
}
