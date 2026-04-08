using Microsoft.EntityFrameworkCore;
using CFMS.Domain.Entities;
using CFMS.Domain.Common;
using System.Linq.Expressions;
using CFMS.Domain.ValueObjects;

namespace CFMS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Drug> Drugs { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Loan> Loans { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Address as owned type for Customer
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Province).HasMaxLength(100);
                address.Property(a => a.District).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(200);
            });
        });

        // Configure Address as owned type for Patient
        modelBuilder.Entity<Patient>(builder =>
        {
            builder.OwnsOne(p => p.Address, address =>
            {
                address.Property(a => a.Province).HasMaxLength(100);
                address.Property(a => a.District).HasMaxLength(100);
                address.Property(a => a.Street).HasMaxLength(200);
            });
        });

        // Apply all configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure all DateTime properties to use UTC
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v.ToUniversalTime(),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }
        }

        // Global query filter for soft delete - ONLY for entities with IsDeleted
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SoftDeleteBaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Create the filter expression
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, "IsDeleted");
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                
                // Call HasQueryFilter using the correct method
                entityType.SetQueryFilter(lambda);
            }
        }
    }
}