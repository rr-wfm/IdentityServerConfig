using IdentityServerConfig.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace IdentityServerConfig.Data;

public class AuditContext : DbContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }
    
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>().ToTable("AuditLog", "IdentityServerConfig");
        modelBuilder.Entity<AuditLog>()
            .Property(e => e.Data)
            .HasConversion(
                v => JsonConvert.SerializeObject(v), 
                v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));
    }
}