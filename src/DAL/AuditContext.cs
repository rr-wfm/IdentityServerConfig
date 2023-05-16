using IdentityServerConfig.Models;
using Microsoft.EntityFrameworkCore;


namespace IdentityServerConfig.DAL;

public class AuditContext : DbContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }
    
    public DbSet<AuditLog> AuditLogs { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<AuditLog>().ToTable("AuditLog", "IdentityServerConfig");
    }
}