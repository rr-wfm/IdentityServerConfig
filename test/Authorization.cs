using System.Security.Claims;
using FluentAssertions;
using IdentityServerConfig.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServerConfig.UnitTest;

[TestFixture]
public class Authorization
{
    private IAuthorizationPolicyProvider _authorizationPolicyProvider;
    private IAuthorizationService _authorizationService;
    
    [OneTimeSetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicies();
        });

        var serviceProvider = serviceCollection.BuildServiceProvider();
        _authorizationService = serviceProvider.GetRequiredService<IAuthorizationService>();
        _authorizationPolicyProvider = serviceProvider.GetRequiredService<IAuthorizationPolicyProvider>();
    }

    [TestCase("identity-server-config.reference-token:view", true)]
    [TestCase("identity-server-config.reference-token", true)]
    [TestCase("identity-server-config", true)]
    [TestCase("some-other-claim", false)]
    public async Task TestReferenceTokenViewPolicy(string userClaim, bool expectedResult)
    {
        var claim = new Claim(userClaim, "true");
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {claim}));
        
        var policy = await _authorizationPolicyProvider.GetPolicyAsync("ReferenceTokenView");
        var result = await _authorizationService.AuthorizeAsync(user, null, policy.Requirements);
        result.Succeeded.Should().Be(expectedResult);
    }

    [TestCase("identity-server-config.reference-token:revoke", true)]
    [TestCase("identity-server-config.reference-token", true)]
    [TestCase("identity-server-config", true)]
    [TestCase("some-other-claim", false)]
    public async Task TestRevokeReferenceTokenPolicy(string userClaim, bool expectedResult)
    {
        var claim = new Claim(userClaim, "true");
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {claim}));
        
        var policy = await _authorizationPolicyProvider.GetPolicyAsync("ReferenceTokenRevoke");
        var result = await _authorizationService.AuthorizeAsync(user, null, policy.Requirements);
        result.Succeeded.Should().Be(expectedResult);
    }

    [TestCase("identity-server-config.client-secret:create", true)]
    [TestCase("identity-server-config.client-secret", true)]
    [TestCase("identity-server-config", true)]
    [TestCase("some-other-claim", false)]
    public async Task TestCreateClientSecret(string userClaim, bool expectedResult)
    {
        var claim = new Claim(userClaim, "true");
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {claim}));
        
        var policy = await _authorizationPolicyProvider.GetPolicyAsync("ClientSecretCreate");
        var result = await _authorizationService.AuthorizeAsync(user, null, policy.Requirements);
        result.Succeeded.Should().Be(expectedResult);
    }
    
    [TestCase("identity-server-config.client-secret:delete", true)]
    [TestCase("identity-server-config.client-secret", true)]
    [TestCase("identity-server-config", true)]
    [TestCase("some-other-claim", false)]
    public async Task TestDeleteClientSecret(string userClaim, bool expectedResult)
    {
        var claim = new Claim(userClaim, "true");
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {claim}));
        
        var policy = await _authorizationPolicyProvider.GetPolicyAsync("ClientSecretDelete");
        var result = await _authorizationService.AuthorizeAsync(user, null, policy.Requirements);
        result.Succeeded.Should().Be(expectedResult);
    }
    
}

