using System.Security.Claims;
using IdentityServerConfig.Data;
using IdentityServerConfig.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace IdentityServerConfig.Services;

public class DatabaseAuditLog : IAuditLog
{
    private readonly AuditContext _auditContext;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILogger _logger;

    public DatabaseAuditLog(AuditContext auditContext, AuthenticationStateProvider authenticationStateProvider, ILogger<DatabaseAuditLog> logger)
    {
        _auditContext = auditContext;
        _authenticationStateProvider = authenticationStateProvider;
        _logger = logger;
    }
    
    public void Log(string action)
    {
        var user = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
        var userId = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
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
        var result = _auditContext.SaveChanges();
        
        if (result != 1)
            _logger.LogError("Audit log failed to save");
        
    }
}