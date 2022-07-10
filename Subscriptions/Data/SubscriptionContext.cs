using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Subscriptions.Domain;

namespace Subscriptions.Data
{
    public class SubscriptionContext: DbContext
    {
        public SubscriptionContext(DbContextOptions<SubscriptionContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionContext).Assembly);
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Subscription> Tags { get; set; }

        private IDbContextTransaction _transaction;

        public void BeginTransaction() {
            _transaction = Database.BeginTransaction();
        }

        public void Commit() {
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            catch (System.Exception)
            {
                _transaction.Dispose();
            }
        }

        public void Rollback() {
            _transaction.Rollback();
            _transaction.Dispose();
        }
    }
}