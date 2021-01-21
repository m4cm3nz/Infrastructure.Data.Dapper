using Infrastructure.Data.Abstractions;

namespace Infrastructure.Data.Dapper
{
    public class Repository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        public Repository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
