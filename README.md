# IdentityServerConfig

> :warning: This is a work-in-progress and generally not recommended to be used at this time

This project is supposed to become at least a read-only view of configuration data for [Duende IdentityServer](https://duendesoftware.com/products/identityserver), a popular framework for implementing OpenID Connect servers. In order to accomplish this we're using IdentityServer's [built-in support](https://docs.duendesoftware.com/identityserver/v6/data/ef/)) for storing configuration data into a database using [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/). By leveraging [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-7.0#blazor-server) we can build a UI on top of this configuration data fairly quickly.

## Build and run
In order to build and run this repository you'll need to have the following:

- Visual Studio or Visual Studio Code (with the C# extension)
- .NET 7 SDK installed
- A database server with some existing IdentityServer configuration (only SQL Server is supported at this time)

Before running locally you'll need to setup the connection string using [.NET's user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows) feature:

```bash
dotnet user-secrets set ConnectionStrings:Configuration <your-connection-string>
```

By default it is assumed that IdentityServer configuration is stored in the `Configuration` schema within the database, but this can be configured by setting the `ConfigurationSchema` configuration setting in `appsettings.Development.json`

## Configuring authentication
Out-of-the-box this application uses OpenID Connect for authentication using Duende's IdentityServer [demo instance](https://demo.duendesoftware.com/). This is fine for development, but in most cases you'll want to override the configuration to point to your own instance of IdentityServer.

This can be done by configuring the following configuration settings, through either appsettings.json or environment variables:

| appsettings.json | Environment Variable | Default value |
|---|---|---|
| OpenIdConnect:Authority | OpenIdConnect__Authority | https://demo.duendesoftware.com |
| OpenIdConnect:ClientId | OpenIdConnect__ClientId | interactive.confidential.short |
| OpenIdConnect:ClientSecret | OpenIdConnect__ClientSecret | secret |

In order to use your own IdentityServer instance, you'll need to add a client to its configuration that has the authorization code with PKCE flow and is authorized for at least the `openid`  and `profile` scopes.

> :warning: Currently this project only requires an authenticated user, but no authorization yet.