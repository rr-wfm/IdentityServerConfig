using Duende.IdentityServer.EntityFramework.Storage;
using IdentityServerConfig.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var openIdConnectConfiguration = builder.Configuration.GetSection("OpenIdConnect");

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        
    })
    .AddCookie()
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = openIdConnectConfiguration.GetValue("Authority", "https://demo.duendesoftware.com/");
        options.ClientId = openIdConnectConfiguration.GetValue("ClientId", "interactive.confidential.short");
        options.ClientSecret = openIdConnectConfiguration.GetValue("ClientSecret", "secret");
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.UseTokenLifetime = false;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.TokenValidationParameters = new TokenValidationParameters { NameClaimType = "name" };

        options.Events = new OpenIdConnectEvents
        {
            OnAccessDenied = context =>
            {
                context.HandleResponse();
                context.Response.Redirect("/");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
   options.AddPolicies();
});
builder.Services.AddConfigurationDbContext(options =>
                {
                    options.DefaultSchema = builder.Configuration.GetValue("ConfigurationSchema", "Configuration");
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(builder.Configuration.GetConnectionString("Configuration"));
                });
builder.Services.AddOperationalDbContext(options =>
{
    options.DefaultSchema = builder.Configuration.GetValue("OperationalSchema", "Operational");
    options.ConfigureDbContext = b =>
        b.UseSqlServer(builder.Configuration.GetConnectionString("Operational"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseForwardedHeaders();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
