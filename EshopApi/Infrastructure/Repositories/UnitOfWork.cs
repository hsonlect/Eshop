using Microsoft.EntityFrameworkCore.Storage;
using EshopApi.Domain.Interfaces;
using EshopApi.Infrastructure.Data;

namespace EshopApi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EshopDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;

        public UnitOfWork(EshopDbContext context)
        {
            _context = context;
            UserRepository = new UserRepository(_context);
            ProductRepository = new ProductRepository(_context);
            CartItemRepository = new CartItemRepository(_context);
        }

        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICartItemRepository CartItemRepository { get; }

        public int SaveChanges() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync(CancellationToken token) => await _context.SaveChangesAsync(token);

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync(CancellationToken token)
        {
            _transaction = await _context.Database.BeginTransactionAsync(token);
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new Exception("Commit transaction is null");
            }
            try
            {
                _context.SaveChanges();
                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitTransactionAsync(CancellationToken token)
        {
            if (_transaction == null)
            {
                throw new Exception("Commit transaction is null");
            }
            try
            {
                await _context.SaveChangesAsync(token);
                await _transaction.CommitAsync(token);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new Exception("Rollback transaction is null");
            }
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken token)
        {
            if (_transaction == null)
            {
                throw new Exception("Rollback transaction is null");
            }
            await _transaction.RollbackAsync(token);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task ExecuteTransactionAsync(Action action, CancellationToken token)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(token);
            try
            {
                action();
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        }

        public async Task ExecuteTransactionAsync(Func<Task> action, CancellationToken token)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(token);
            try
            {
                await action();
                await _context.SaveChangesAsync(token);
                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync(token);
                throw;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
