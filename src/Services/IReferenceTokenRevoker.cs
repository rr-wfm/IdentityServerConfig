namespace IdentityServerConfig.Services;

public interface IRevoker
{
    void Revoke(long id);
}