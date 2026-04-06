using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CFMS.Domain.Entities;

namespace CFMS.Infrastructure.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.FullName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(d => d.Specialization)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(d => d.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(d => d.Email)
            .HasMaxLength(100);
            
        builder.Property(d => d.ConsultationFee)
            .HasPrecision(18, 2);
            
        builder.HasIndex(d => d.Email)
            .IsUnique()
            .HasFilter("\"Email\" IS NOT NULL");
    }
}