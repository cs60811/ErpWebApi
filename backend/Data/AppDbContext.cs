using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public DbSet<ErpUser> ErpUsers { get; set; } = null!;
        public DbSet<UserCustomer> UserCustomers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 建立 UserId 上的索引，提高查詢效率
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            // 建立 ErpUser 中的 ErpCode 唯一索引
            modelBuilder.Entity<ErpUser>()
                .HasIndex(e => e.ErpCode)
                .IsUnique();

            // 定義關聯關係 (UserProfile - UserCustomer - ErpUser)
            modelBuilder.Entity<UserCustomer>()
                .HasOne(uc => uc.UserProfile)
                .WithMany(u => u.Customers)
                .HasForeignKey(uc => uc.UserProfileId);

            modelBuilder.Entity<UserCustomer>()
                .HasOne(uc => uc.ErpUser)
                .WithMany(e => e.Customers)
                .HasForeignKey(uc => uc.ErpUserId);
        }
    }
}
