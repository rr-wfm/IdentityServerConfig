# IdentityServerConfig

> :warning: This is a work-in-progress and generally not recommended to be used at this time

This project is supposed to become at least a read-only view of configuration data for [Duende IdentityServer](https://duendesoftware.com/products/identityserver), a popular framework for implementing OpenID Connect servers. In order to accomplish this we're using IdentityServer's [built-in support](https://docs.duendesoftware.com/identityserver/v6/data/ef/)) for storing configuration data into a database using [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/). By leveraging [Blazor Server](https://learn.microsoft.com/en-us/aspnet/core/blazor/hosting-models?view=aspnetcore-7.0#blazor-server) we can build a UI on top of this configuration data fairly quickly.

## Build and run
In order to build and run this repository you'll need to have the following:

- Visual Studio or Visual Studio Code (with the C# extension)
- .NET 7 SDK installed
- A database server with some existing IdentityServer configuration
