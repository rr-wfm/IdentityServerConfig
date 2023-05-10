using Microsoft.AspNetCore.Authorization;

namespace IdentityServerConfig.Components;

public static class PolicyAuthorization
{
    public static void AddPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy("ReferenceTokenView", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.reference-token"
                        or "identity-server-config.reference-token:view")));

        options.AddPolicy("ReferenceTokenRevoke", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.reference-token"
                        or "identity-server-config.reference-token:revoke")));

        options.AddPolicy("ClientSecretCreate", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.client-secret"
                        or "identity-server-config.client-secret:create")));

        options.AddPolicy("ClientSecretDelete", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.client-secret"
                        or "identity-server-config.client-secret:delete")));
    }
}