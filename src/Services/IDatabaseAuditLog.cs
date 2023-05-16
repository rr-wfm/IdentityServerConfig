namespace IdentityServerConfig.Services;

public interface IDatabaseAuditLog
{
    int Log(string action);
}