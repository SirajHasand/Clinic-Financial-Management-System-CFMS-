namespace CFMS.Domain.Common;

/// <summary>
/// Base entity with common properties for ALL entities
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Base entity with soft delete functionality - use only for entities that need deletion
/// </summary>
public abstract class SoftDeleteBaseEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
}
