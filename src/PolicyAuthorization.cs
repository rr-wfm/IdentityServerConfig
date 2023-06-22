namespace Microsoft.AspNetCore.Authorization;

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
        
        options.AddPolicy("ClientView", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.client"
                        or "identity-server-config.client:view")));
        
        options.AddPolicy("ApiResourceView", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.api-resource"
                        or "identity-server-config.api-resource:view")));
        
        options.AddPolicy("ScopeView", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                    c.Type is "identity-server-config" or "identity-server-config.scope"
                        or "identity-server-config.scope:view")));
    }
}