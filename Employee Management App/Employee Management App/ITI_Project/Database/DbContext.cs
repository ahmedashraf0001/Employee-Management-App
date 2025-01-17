using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI_Project.Database
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext()
        {

        }
        public CompanyDbContext(DbContextOptions options) :
            base(options)
        {

        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.db");
            optionsBuilder.UseLazyLoadingProxies()
                          .UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Departments");

                entity.Property(e => e.Name);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Name).HasMaxLength(50);
            });
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.ToTable("Employees");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
                entity.Property(e => e.Name).HasColumnName("Name");

                entity.HasOne(e => e.Department).WithMany(e => e.Employees)
                .HasForeignKey(e => e.DepartmentId);
            });
        }
    }
}
