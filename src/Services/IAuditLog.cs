namespace IdentityServerConfig.Services;

public interface IAuditLog
{
     Task Log(string action, IDictionary<string, string> data);
}