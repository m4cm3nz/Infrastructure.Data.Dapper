using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Dapper
{
    internal class RelayDbConnectionFactory : IDbConnectionFactory
    {
        private readonly Func<Task<DbConnection>> _factory;

        public RelayDbConnectionFactory(Func<Task<DbConnection>> factory) => _factory = factory;

        public async Task<DbConnection> CreateAsync(CancellationToken cancellationToken = default) => await _factory();
    }
}
