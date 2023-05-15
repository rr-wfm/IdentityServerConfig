using System.Security.Cryptography;
using System.Text;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityServerConfig.Models;

namespace IdentityServerConfig.Services;

public class ReferenceTokenValidator : IReferenceTokenValidator
{
    public bool IsActive(DateTime? expirationDate)
    {
        return expirationDate == null || expirationDate >= DateTime.UtcNow;
    }

    public ReferenceTokenDataModel Validate(CheckReferenceTokenModel checkReferenceTokenModel,
        PersistedGrantDbContext persistedGrantDbContext)
    {
        ReferenceTokenDataModel returnValue = new();
        
        //hash the token
        var token = string.Concat(checkReferenceTokenModel.ReferenceToken, ":reference_token");
        var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        var hexString = BitConverter.ToString(hash).Replace("-", "");

        //check if the token exists in the database
        var persistedGrant = persistedGrantDbContext.PersistedGrants.FirstOrDefault(c => c.Key.Equals(hexString));
        if (persistedGrant == null)
        {
            returnValue.Status = ReferenceTokenDataModel.StatusCode.Invalid;
            returnValue.ClientId = checkReferenceTokenModel.ClientId;
            return returnValue;
        }

        //check if the client id matches with the reference token
        var clientCheck = persistedGrant.ClientId.Equals(checkReferenceTokenModel.ClientId);
        if (clientCheck)
        {
            returnValue.Status = IsActive(persistedGrant.Expiration) ? ReferenceTokenDataModel.StatusCode.Active : ReferenceTokenDataModel.StatusCode.Expired; 
            returnValue.CreationTime = persistedGrant.CreationTime;
            returnValue.Expiration = persistedGrant.Expiration;
        }
        else
        {
            returnValue.Status = ReferenceTokenDataModel.StatusCode.Invalid;
            returnValue.ClientId = checkReferenceTokenModel.ClientId;
        }

        return returnValue;
    }
}