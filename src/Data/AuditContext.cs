using System.Text.Json;
using IdentityServerConfig.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerConfig.Data;

public class AuditContext : DbContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs { get; set; }
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>().ToTable("AuditLog", "IdentityServerConfig");
        modelBuilder.Entity<AuditLog>()
            .Property(e => e.Data)
            .HasConversion(
                v => JsonSerializer.Serialize(v, v.GetType(), _jsonSerializerOptions),
                v => JsonSerializer.Deserialize<object>(v, _jsonSerializerOptions));
    }
}