using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UcomGridView.Data.Entities;
using UcomGridView.Data.Entities.Interfaces;

namespace UcomGridView.Data.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(UserTable);
            modelBuilder.Entity<Status>(StatusTable);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var dateTime = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ITrackable entity)
                {
                    if (entry.State == EntityState.Added)
                        entity.CreatedAt = dateTime;

                    entity.UpdatedAt = dateTime;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UserTable(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Users");
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityTypeBuilder.Property(x => x.Firstname).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(x => x.Lastname).HasMaxLength(50).IsRequired();
            entityTypeBuilder.Property(x => x.Email).HasMaxLength(128).IsRequired();
            entityTypeBuilder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
            entityTypeBuilder.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
            entityTypeBuilder.HasAlternateKey(x => x.Email);
            entityTypeBuilder.Property(x => x.IsDeleted).HasDefaultValue(false);
            entityTypeBuilder.Property(x => x.AvatarPath).HasMaxLength(42).IsRequired(false);
            entityTypeBuilder.ToSqlQuery("CONSTRAINT [users_age_check] CHECK([Age]>0)");
            entityTypeBuilder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).IsRequired();
        }

        private void StatusTable(EntityTypeBuilder<Status> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Statuses");
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();
            entityTypeBuilder.Property(x => x.Name).HasMaxLength(20).IsRequired();
            entityTypeBuilder.HasAlternateKey(x => x.Name);
        }
    }
}
