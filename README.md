# IdentityServerConfig

> :warning: This is a work-in-progress and generally not recommended to be used at this time

This project is supposed to become at least a read-only view of configuration data for [Duende IdentityServer](https://duendesoftware.com/products/identityserver), a popular framework for implementing OpenID Connect servers. In order to accomplish this we're using IdentityServer's [built-in support](https://docs.duendesoftware.com/identityserver/v6/data/ef/)) for storing configuration data into a database using [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/). By leveraging [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-7.0#blazor-server) we can build a UI on top of this configuration data fairly quickly.

## Build and run
In order to build and run this repository you'll need to have the following:

- Visual Studio or Visual Studio Code (with the C# extension)
- .NET 7 SDK installed
- A database server with some existing IdentityServer configuration (only SQL Server is supported at this time)

Before running locally you'll need to setup the connection strings using [.NET's user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows) feature:

```bash
dotnet user-secrets set ConnectionStrings:Configuration <your-connection-string>
dotnet user-secrets set ConnectionStrings:Operational <your-connection-string>
```

By default it is assumed that IdentityServer configuration is stored in the `Configuration` schema within the database, but this can be configured by setting the `ConfigurationSchema` configuration setting in `appsettings.Development.json`

### Audit logs

IdentityServerConfig also keeps a log of the changes that are made by users. This is stored in the `IdentityServerConfig` schema within the database. The schema can be stored in the same database as the IdentityServer. 

You will need to set the connection string using the [.NET's user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows) feature:

```bash
dotnet user-secrets set ConnectionStrings:AuditLog <your-connection-string>
```

To create the audit log table, run the `IdentityServerConfig.Migrations` project. This will create the table in the database.


## Configuring authentication
Out-of-the-box this application uses OpenID Connect for authentication using Duende's IdentityServer [demo instance](https://demo.duendesoftware.com/). This is fine for development, but in most cases you'll want to override the configuration to point to your own instance of IdentityServer.

This can be done by configuring the following configuration settings, through either appsettings.json or environment variables:

| appsettings.json           | Environment Variable        | Default value                   |
|----------------------------|-----------------------------|---------------------------------|
| OpenIdConnect:Authority    | OpenIdConnect__Authority    | https://demo.duendesoftware.com |
| OpenIdConnect:ClientId     | OpenIdConnect__ClientId     | interactive.confidential.short  |
| OpenIdConnect:ClientSecret | OpenIdConnect__ClientSecret | secret                          |

It is also possible to use the [.NET's user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows) feature to configure the authentication settings:

```bash
dotnet user-secrets set OpenIdConnect:Authority <your-identity-server-instance>
dotnet user-secrets set OpenIdConnect:ClientId <your-client-id>
dotnet user-secrets set OpenIdConnect:ClientSecret <your-client-secret>
```

In order to use your own IdentityServer instance, you'll need to add a client to its configuration that has the `AlwaysIncludeUserClaimsInIdToken` set to `true` and has the authorization code with PKCE flow and is authorized for at least the `openid`  and `profile` scopes.

## Authorization
The authorization for this application is handled though the UserClaims table in the IdentityServer Database. The authorization is currently only configured for the Reference Token page. In order to authorize a user to view this page, you'll need to add a claim to the UserClaims table. The claims types are build up as follows:
`<application>.<page>:<action>`. The claim value is not used, so it can be left empty.

The `<application>` is always `identity-server-config`, the `<page>` is the name of the page, and the `<action>` is the name of the action. The action is optional, and if not specified, the user will be authorized for all actions on the page.
If the page is not specified, the user will be authorized for all pages in the application.
The claims are always written in [kebab-case](https://betterprogramming.pub/string-case-styles-camel-pascal-snake-and-kebab-case-981407998841).

Current accepted claims types are:
```
identity-server-config
identity-server-config.reference-token
identity-server-config.reference-token:view
identity-server-config.reference-token:revoke
identity-server-config.client
identity-server-config.client:view
identity-server-config.api-resource
identity-server-config.api-resource:view
identity-server-config.scope
identity-server-config.scope:view
```

For future development, the following claims types are reserved:
```
identity-server-config.client-secret
identity-server-config.client-secret:create
identity-server-config.client-secret:delete
```
These claims types are not yet implemented, but will be used to authorize the client secrets page.

