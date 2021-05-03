using Infrastructure.Data.Abstractions;
using Infrastructure.Data.Dapper;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IDapperBuilder
    {
        IServiceCollection Services { get; }

        IDapperBuilder AddDbConnectionFactory(Func<Task<DbConnection>> factory);
        IDapperBuilder AddDbConnectionFactory<TImplementation>() where TImplementation : class, IDbConnectionFactory;
    }

    internal class DapperBuilder : IDapperBuilder
    {
        public IServiceCollection Services { get; }

        public DapperBuilder(IServiceCollection services)
        {
            Services = services;

            Services.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
            Services.TryAddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        }

        public IDapperBuilder AddDbConnectionFactory(Func<Task<DbConnection>> factory)
        {
            Services.AddSingleton<IDbConnectionFactory>(new RelayDbConnectionFactory(factory));

            return this;
        }

        public IDapperBuilder AddDbConnectionFactory<TImplementation>() where TImplementation : class, IDbConnectionFactory
        {
            Services.AddScoped<IDbConnectionFactory, TImplementation>();

            return this;
        }

        public void Validate()
        {
            if (!Services.Any(sd => sd.ServiceType == typeof(IDbConnectionFactory)))
            {
                throw new InvalidOperationException($"You must provide an implementation for the service {nameof(IDbConnectionFactory)}");
            }
        }
    }
}
