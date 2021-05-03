using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data.Dapper
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> CreateAsync(CancellationToken cancellationToken = default);
    }
}
