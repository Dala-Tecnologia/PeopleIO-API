using System.ComponentModel.DataAnnotations;

namespace PeopleIO.Domain.Entity;

public abstract class BaseEntity
{
    private const string DefaultUser = "System";
    
    [Key]
    public Guid Id { get; set; }= Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public string CreatedBy { get; private set; } = DefaultUser;
    public DateTimeOffset? UpdatedAt { get; private set; } 
    public string? UpdatedBy { get; private set; }

    protected BaseEntity(string createdBy, DateTimeOffset createdAt)
    {
        CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
        CreatedAt = createdAt;
    }

    protected BaseEntity(){}

    public void SetCreated(string user, DateTimeOffset? at = null)
    {
        CreatedBy = user ?? throw new ArgumentNullException(nameof(user));
        CreatedAt = at ?? DateTimeOffset.UtcNow;
    }

    public void SetUpdated(string user = DefaultUser, DateTimeOffset? at = null)
    {
        UpdatedAt = at ?? DateTimeOffset.UtcNow;
        UpdatedBy = user;
    }
}