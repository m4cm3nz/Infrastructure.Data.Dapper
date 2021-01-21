using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using Infrastructure.Data.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data.Dapper
{
    public abstract class BaseRepository<TEntity> :
        IRepository<TEntity>
        where TEntity : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new
                ArgumentException(nameof(unitOfWork));
        }

        public virtual async Task<dynamic> Add(TEntity entity)
        {
            var connection = await _unitOfWork.GetOpenedConnectionAsync();
            return await connection.InsertAsync(entity, _unitOfWork.Transaction);
        }

        public virtual async Task Update(TEntity entity, dynamic id)
        {
            var connection = await _unitOfWork.GetOpenedConnectionAsync();
            await connection.UpdateAsync(entity, _unitOfWork.Transaction);
        }

        /// <summary>
        /// Busca o objeto correspondente ao id informado e exclui caso encontrado.
        /// </summary>
        /// <param name="id">identificador único do objeto.</param>
        /// <remarks>Caso você ja possua a instância do objeto que deseja excluir utilize:
        /// <code>Task DeleteBy(TEntity entity)</code>
        /// </remarks>
        /// <exception cref="ArgumentNullException"
        public async Task DeleteBy(dynamic id)
        {
            var entity = await GetByID(id);
            await DeleteBy(entity);
        }

        public virtual async Task<TEntity> GetByID(dynamic id)
        {
            var connection = await _unitOfWork.GetOpenedConnectionAsync();
            return await connection.GetAsync<TEntity>((int)id, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return (await GetAll()).Where(predicate.Compile());
        }

        public async Task DeleteBy(TEntity entity)
        {
            var toDelete = entity ?? throw new ArgumentNullException(nameof(entity));

            var connection = await _unitOfWork.GetOpenedConnectionAsync();
            await connection.DeleteAsync(toDelete, _unitOfWork.Transaction);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var connection = await _unitOfWork.GetOpenedConnectionAsync();
            return await connection.GetAllAsync<TEntity>(_unitOfWork.Transaction);
        }

        public async Task<bool> FindByID(dynamic identity)
        {
            return (await GetByID(identity)) != null;
        }
    }
}
