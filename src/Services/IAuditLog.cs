namespace IdentityServerConfig.Services;

public interface IAuditLog
{
    void Log(string action);
}