using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServerConfig.Models;

namespace IdentityServerConfig.Services;

public interface IReferenceTokenValidator
{
    bool IsActive(DateTime? expirationDate);

    ReferenceTokenDataModel Validate(CheckReferenceTokenModel checkReferenceTokenModel,
        PersistedGrantDbContext persistedGrantDbContext);
}