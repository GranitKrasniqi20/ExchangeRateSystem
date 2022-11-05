using AutoMapper;
using ExchangeRateSystem.ServiceCore.Services;
using ExchangeRateSystem.RepositoryCore.Repositories;
using ExchangeRateSystem.RepositoryCore.Repositories.Contracts;
using ExchangeRateSystem.ServiceCore.Services.Contracts;

namespace ExchangeRateSystem.API.Infrastructure
{
    public class Bootstrapper
    {
        private IServiceCollection serviceCollection;
        public Bootstrapper(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void Run()
        {
            RegisterRepositories();
            RegisterServices();
        }

        private void RegisterRepositories()
        {
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        }

        private void RegisterServices()
        {
            serviceCollection.AddScoped<ICacheService, CacheService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<IExchangeRateService, ExchangeRateService>();
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
