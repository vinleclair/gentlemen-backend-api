using Gentlemen.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Gentlemen.Infrastructure
{
    public class GentlemenContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public GentlemenContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        #region Transaction Handling

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            if (!Database.IsInMemory())
            {
                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion
    }
}