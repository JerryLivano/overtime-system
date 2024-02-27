using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class OvertimeSystemDbContext : DbContext
    {
        public OvertimeSystemDbContext(DbContextOptions<OvertimeSystemDbContext> options) : base(options) { }

        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Role>? Roles { get; set; }
        public DbSet<AccountRole>? AccountRoles { get; set; }
        public DbSet<OvertimeRequest>? OvertimeRequests { get; set; }
        public DbSet<Overtime>? Overtimes { get; set; }
        public DbSet<Employee>? Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.AccountRoles)
                .WithOne(ar => ar.Account)
                .HasForeignKey(ar => ar.AccountId);

            modelBuilder.Entity<Employee>()
                .HasMany(m => m.Employees)
                .WithOne(e => e.Manager)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Employee)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(a => a.Id);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.OvertimeRequests)
                .WithOne(or => or.Account)
                .HasForeignKey(or => or.AccountId);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.AccountRoles)
                .WithOne(ar => ar.Role)
                .HasForeignKey(ar => ar.RoleId);

            modelBuilder.Entity<OvertimeRequest>()
                .HasOne(or => or.Overtime)
                .WithMany(o => o.OvertimeRequests)
                .HasForeignKey(or => or.OvertimeId);
        }
    }
}
