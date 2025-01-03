using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AspNetCoreIdentityAuthorizationCustom.Identity;
public class UserClaimsPrincipalCustomFactory(
    UserManager<IdentityUser> userManager,
    IOptions<IdentityOptions> optionsAccessor) : UserClaimsPrincipalFactory<IdentityUser>(userManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(IdentityUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim(
                PolicyType.Supervisor.ToString(),
                $"{ClaimType.Create},{ClaimType.Read},{ClaimType.Update},{ClaimType.Delete}"
            ));

        identity.AddClaim(new Claim("Country", "BR"));
        identity.AddClaim(new Claim("Age", "15"));

        return identity;
    }
}
