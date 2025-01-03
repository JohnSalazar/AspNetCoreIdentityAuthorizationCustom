using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Interfaces;
using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom;

public class AuthorizeCustomAttribute : AuthorizeAttribute, IAuthorizeCustomAttribute
{
    public ClaimType[] Claims { get; init; } = Array.Empty<ClaimType>();

    public bool ReplaceDefaultClaims { get; init; } = false;

    public PolicyType? Policy { get; init; } = null;

    public AuthorizeCustomAttribute() { }

    public AuthorizeCustomAttribute(PolicyType policy)
    {
        Policy = policy;
    }

    public AuthorizeCustomAttribute(PolicyType policy, ClaimType[] claims, bool replaceDefaultClaims = false)
    {
        Policy = policy;
        Claims = claims;
        ReplaceDefaultClaims = replaceDefaultClaims;
    }
}
