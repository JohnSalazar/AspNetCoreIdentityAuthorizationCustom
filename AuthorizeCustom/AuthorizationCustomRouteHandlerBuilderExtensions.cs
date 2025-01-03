using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Interfaces;
using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;

namespace AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom;
public static class AuthorizationCustomRouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder RequireAuthorizationCustom(this RouteHandlerBuilder builder)
    {
        var attribute = CreateAuthorizeCustomAttribute();
        return builder.WithMetadata(attribute);
    }

    public static RouteHandlerBuilder RequireAuthorizationCustom(this RouteHandlerBuilder builder, PolicyType policy)
    {
        var attribute = CreateAuthorizeCustomAttribute(policy);
        return builder.WithMetadata(attribute);
    }

    public static RouteHandlerBuilder RequireAuthorizationCustom(this RouteHandlerBuilder builder, PolicyType policy, ClaimType[] claims, bool replaceDefaultClaims = false)
    {
        var attribute = CreateAuthorizeCustomAttribute(policy, claims, replaceDefaultClaims);
        return builder.WithMetadata(attribute);
    }

    private static IAuthorizeCustomAttribute CreateAuthorizeCustomAttribute(PolicyType? policy = null, ClaimType[]? claims = null, bool replaceDefaultClaims = false) => new AuthorizeCustomAttribute { Policy = policy, Claims = claims ?? Array.Empty<ClaimType>(), ReplaceDefaultClaims = replaceDefaultClaims };
}
