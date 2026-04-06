using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CFMS.Domain.Entities;

namespace CFMS.Infrastructure.Data.Configurations;

public class DrugConfiguration : IEntityTypeConfiguration<Drug>
{
    public void Configure(EntityTypeBuilder<Drug> builder)
    {
        builder.ToTable("Drugs");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(d => d.BatchNumber)
            .HasMaxLength(50);
            
        builder.Property(d => d.Description)
            .HasMaxLength(500);
            
        builder.Property(d => d.PurchasePrice)
            .HasPrecision(18, 2);
            
        builder.Property(d => d.SellingPrice)
            .HasPrecision(18, 2);
            
        builder.HasIndex(d => d.Name);
        builder.HasIndex(d => d.BatchNumber);
    }
}