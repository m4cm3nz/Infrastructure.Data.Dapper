using Infrastructure.Data.Abstractions;

namespace Refere.Infrastructure.Data.Dapper
{
    public class Repository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
