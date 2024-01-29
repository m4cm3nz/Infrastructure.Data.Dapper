using Infrastructure.Data.Abstractions;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Infrastructure.Data.Dapper
{
    public sealed class UnitOfWork(DbConnection connection) : IUnitOfWork
    {
        private DbConnection _connection = connection;
        private readonly Guid _id = Guid.NewGuid();

        public async Task<IDbConnection> GetOpenedConnectionAsync()
        {
            if (_connection.State == ConnectionState.Closed)
                await _connection.OpenAsync();
            else if (_connection.State == ConnectionState.Broken)
            {
                await _connection.CloseAsync();
                await _connection.OpenAsync();
            }
            return _connection;
        }

        public IDbTransaction Transaction { get; private set; } = null;
        public Guid Id
        {
            get { return _id; }
        }

        public void Begin()
        {
            Transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            Transaction.Commit();
            DisposeTransaction();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            DisposeTransaction();
        }

        public void Dispose()
        {
            DisposeTransaction();
            _connection.Dispose();
            _connection = null;
        }

        private void DisposeTransaction()
        {
            Transaction?.Dispose();
            Transaction = null;
        }
    }
}
