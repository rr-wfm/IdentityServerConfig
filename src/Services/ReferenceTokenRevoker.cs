using Duende.IdentityServer.EntityFramework.DbContexts;

namespace IdentityServerConfig.Services;

public class ReferenceTokenRevoker : IReferenceTokenRevoker
{
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ILogger _logger;
    private readonly IAuditLog _auditLog;
    
    public ReferenceTokenRevoker(PersistedGrantDbContext persistedGrantDbContext, ILogger<ReferenceTokenRevoker> logger, IAuditLog auditLog)
    {
        _persistedGrantDbContext = persistedGrantDbContext;
        _logger = logger;
        _auditLog = auditLog;
    }
    
    public async Task Revoke(long referenceTokenId, string clientId)
    {
        Dictionary<string, string> data = new()
        {
            { "ClientId", clientId },
            { "ReferenceTokenId", referenceTokenId.ToString() }
        };
        
        await _auditLog.Log("ReferenceTokenRevoke", data);
        var referenceToken = await _persistedGrantDbContext.PersistedGrants.FindAsync(referenceTokenId);
        if (referenceToken == null || referenceToken.Type != "reference_token" ||
            referenceToken.Expiration < DateTime.UtcNow)
        {
            _logger.LogError("id: {referenceTokenId} not found or not a reference token or expired", referenceTokenId);
            return;
        }
        
        referenceToken.Expiration = DateTime.UtcNow;
        await _persistedGrantDbContext.SaveChangesAsync();
    }
}