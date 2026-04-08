using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CFMS.Domain.Entities;

namespace CFMS.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.NationalId)
            .HasMaxLength(20);

        builder.HasIndex(p => p.NationalId)
            .IsUnique()
            .HasFilter("\"NationalId\" IS NOT NULL");

        builder.HasIndex(p => p.PhoneNumber);
    }
}