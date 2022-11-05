using ExchangeRateSystem.EntityCore.Models;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateSystem.ServiceCore.Services
{
    public class CacheService : ICacheService
    {
        private IMemoryCache cache;
        private IUserRepository userRepository;
        private IExchangeRateRepository exchangeRateRepository;
        public CacheService(IMemoryCache cache, IUserRepository userRepository, IExchangeRateRepository exchangeRateRepository)
        {
            this.cache = cache;
            this.userRepository = userRepository;
            this.exchangeRateRepository = exchangeRateRepository;   
        }

        public void InitCache()
        {
            var name = typeof(User).Name;
            cache.Set(name, GetList<User>());

            name = typeof(ExchangeRate).Name;
            cache.Set(name, GetList<ExchangeRate>());
        }
        private object GetList<T>()
        {
            if (typeof(T) == typeof(User))
            {
                return userRepository.GetAll();
            }
            if (typeof(T) == typeof(ExchangeRate))
            {
                return exchangeRateRepository.GetAll();
            }
            else
            {
                return new List<T>();
            }

        }

        public List<T> GetAll<T>()
        {
            var name = typeof(T).Name;
            List<T> list;
            // Look for cache key.
            if (!cache.TryGetValue(name, out list))
            {
                //throw new NullReferenceException();
                list = (List<T>)GetList<T>();
                //// Save data in cache.
                cache.Set(name, list);
            }
            return list;
        }
    }
}
