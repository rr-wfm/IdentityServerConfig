using IdentityServerConfig.DAL;
using IdentityServerConfig.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityServerConfig.Services;

public class DatabaseAuditLog : IDatabaseAuditLog
{
    private readonly AuditContext _auditContext;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    
    public DatabaseAuditLog(AuditContext auditContext, AuthenticationStateProvider authenticationStateProvider)
    {
        _auditContext = auditContext;
        _authenticationStateProvider = authenticationStateProvider;
    }
    
    public int Log(string action)
    {
        var user = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
        var userId = user.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))?.Value;
        var userName = user.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;

        if (userId == null || userName == null)
            throw new Exception("User not found");
        
        var audit = new AuditLog
        {
            UserId = userId,
            UserName = userName,
            Action = action,
            Timestamp = DateTime.Now
        };
        
        _auditContext.AuditLogs.Add(audit);
        return _auditContext.SaveChanges();
    }
}