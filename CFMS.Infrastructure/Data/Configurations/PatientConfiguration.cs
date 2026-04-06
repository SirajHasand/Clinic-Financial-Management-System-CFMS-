using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CFMS.Domain.Entities;
using CFMS.Domain.ValueObjects;

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

        //  Store Address as ONE column using ValueConverter
        builder.Property(p => p.Address)
            .HasConversion(
                v => v.ToString(),              // Address -> string (save)
                v => ParseAddress(v)            // string -> Address (read)
            )
            .HasColumnName("Address")
            .HasMaxLength(500);

        builder.HasIndex(p => p.NationalId)
            .IsUnique()
            .HasFilter("\"NationalId\" IS NOT NULL");

        builder.HasIndex(p => p.PhoneNumber);
    }

    // ✅ Helper method to convert string back to Address
    private static Address ParseAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new Address("", "", "");

        var parts = value.Split(", ");

        return new Address(
            parts.Length > 0 ? parts[0] : "",
            parts.Length > 1 ? parts[1] : "",
            parts.Length > 2 ? parts[2] : ""
        );
    }
}