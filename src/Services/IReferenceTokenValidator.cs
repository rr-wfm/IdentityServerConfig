using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServerConfig.Models;

namespace IdentityServerConfig.Services;

public interface IReferenceTokenValidator
{
    ReferenceTokenDataModel Validate(CheckReferenceTokenModel checkReferenceTokenModel);
}