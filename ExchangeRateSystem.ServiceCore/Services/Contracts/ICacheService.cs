﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services.Contracts
{
    public interface ICacheService
    {
        void InitCache();
        public List<T> GetAll<T>();
    }
}
