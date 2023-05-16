using Duende.IdentityServer.EntityFramework.DbContexts;

namespace IdentityServerConfig.Services;

public class ReferenceTokenRevoker : IRevoker
{
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    private readonly ILogger _logger;
    
    public ReferenceTokenRevoker(PersistedGrantDbContext persistedGrantDbContext, ILogger<ReferenceTokenRevoker> logger)
    {
        _persistedGrantDbContext = persistedGrantDbContext;
        _logger = logger;
    }
    
    public void Revoke(long id)
    {
        var referenceToken = _persistedGrantDbContext.PersistedGrants.Find(id);
        if (referenceToken == null || referenceToken.Type != "reference_token" ||
            referenceToken.Expiration < DateTime.UtcNow)
        {
            _logger.LogError("id: {id} not found or not a reference token or expired", id);
            return;
        }
        
        referenceToken.Expiration = DateTime.UtcNow;
        _persistedGrantDbContext.SaveChanges();
    }
}