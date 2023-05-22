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
    
    public async Task Log(string action, IDictionary<string, string> data)
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;
        var userId = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        var userName = user.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;

        if (userId == null || userName == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var audit = new AuditLog
        {
            UserId = userId,
            UserName = userName,
            Action = action,
            Timestamp = DateTime.UtcNow,
            Data = data
        };
        
        _auditContext.AuditLogs.Add(audit);
        var result = await _auditContext.SaveChangesAsync();
        
        if (result != 1)
            _logger.LogError("Audit log failed to save");
        
    }
}