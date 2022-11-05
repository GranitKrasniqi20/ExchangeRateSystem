using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Utilities
{
    public static class GeneralHelper
    {
        public static DateTime ConvertTimeStampToDate(string timeStamp)
        {
            var baseDate = new DateTime(1970, 01, 01);
            var date = baseDate.AddSeconds(double.Parse(timeStamp)).ToLocalTime();
            //DateTime dateTime = new DateTime
            //(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(timeStamp)).ToLocalTime();
           
            return date;
        }

        public static string ConvertDateToTimestamp(DateTime date)
        {
            var baseDate = new DateTime(1970, 01, 01);
            var timeStamp = date.Subtract(baseDate).TotalSeconds.ToString();
            return timeStamp;
        }
    }
}
