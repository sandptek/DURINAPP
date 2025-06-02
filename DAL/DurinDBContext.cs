using Entities;
using Entities.DB;
using Microsoft.EntityFrameworkCore;
using Task = Entities.DB.Task;

namespace DAL
{
    public class DurinDBContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Hospital> Hospitals { get; set; }
		public DbSet<HospitalUser> HospitalUsers { get; set; }
		public DbSet<AISetting> AISettings { get; set; }
		public DbSet<Plate> Plates { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<TaskComment> TaskComments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<Product>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Product>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<Order>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Order>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<OrderItem>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<OrderItem>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<Hospital>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Hospital>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<HospitalUser>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<HospitalUser>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<AISetting>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<AISetting>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<Task>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<Task>().Property(b => b.deleted).HasDefaultValue(0);
			modelBuilder.Entity<TaskComment>().Property(b => b.createdDate).HasDefaultValueSql("getdate()");
			modelBuilder.Entity<TaskComment>().Property(b => b.deleted).HasDefaultValue(0);
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(AppConfig.sqlConnStr, b => b.MigrationsAssembly("DAL")).EnableSensitiveDataLogging();
        }
    }
}
