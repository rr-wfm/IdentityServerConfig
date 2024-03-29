using System.Security.Cryptography;
using System.Text;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServerConfig.Models;
using IdentityServerConfig.Pages;

namespace IdentityServerConfig.Services;

public class ReferenceTokenValidator : IReferenceTokenValidator
{
    private readonly PersistedGrantDbContext _persistedGrantDbContext;
    public ReferenceTokenValidator(PersistedGrantDbContext persistedGrantDbContext)
    {
        _persistedGrantDbContext = persistedGrantDbContext;
    }
    
    public ReferenceTokenDataModel Validate(CheckReferenceTokenModel checkReferenceTokenModel)
    {
        ReferenceTokenDataModel returnValue = new();
        
        //hash the token
        var token = string.Concat(checkReferenceTokenModel.ReferenceToken, ":reference_token");
        var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        var hexString = BitConverter.ToString(hash).Replace("-", "");

        //check if the token exists in the database
        var persistedGrant = _persistedGrantDbContext.PersistedGrants.FirstOrDefault(c => c.Key.Equals(hexString));
        if (persistedGrant == null)
        {
            returnValue.Status = ReferenceTokenDataModel.StatusCode.Invalid;
            return returnValue;
        }

        //check if the client id matches with the reference token
        var clientCheck = persistedGrant.ClientId.Equals(checkReferenceTokenModel.ClientId);
        if (clientCheck)
        {
            returnValue.Status = ReferenceTokens.IsActive(persistedGrant.Expiration) ? ReferenceTokenDataModel.StatusCode.Active : ReferenceTokenDataModel.StatusCode.Expired; 
            returnValue.CreationTime = persistedGrant.CreationTime;
            returnValue.Expiration = persistedGrant.Expiration;
        }
        else
        {
            returnValue.Status = ReferenceTokenDataModel.StatusCode.Invalid;
        }

        return returnValue;
    }
}