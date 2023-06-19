namespace IdentityServerConfig.Services;

public interface IReferenceTokenRevoker
{
    Task Revoke(long id, string clientId);
}