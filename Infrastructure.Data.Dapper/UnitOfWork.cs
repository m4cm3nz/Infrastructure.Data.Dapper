using Infrastructure.Data.Abstractions;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Refere.Infrastructure.Data.Dapper
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private DbConnection _connection = null;

        public UnitOfWork(DbConnection connection)
        {
            _id = Guid.NewGuid();
            _connection = connection;
        }

        private Guid _id = Guid.Empty;

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
            if (Transaction != null)
                Transaction.Dispose();
            Transaction = null;
        }
    }
}
