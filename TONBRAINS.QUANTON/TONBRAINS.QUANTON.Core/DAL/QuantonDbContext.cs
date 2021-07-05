using Marques.EFCore.SnakeCase;
using Microsoft.EntityFrameworkCore;

namespace TONBRAINS.QUANTON.Core.DAL
{
    public class QuantonDbContext : DbContext
    {
        public QuantonDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TransferLog> TransferLogs { get; set; }
        public DbSet<TransferLogView> ViewTransferLogs { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ToSnakeCase();
        }
    }
}
