using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;

namespace AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Interfaces;
public interface IAuthorizeCustomAttribute
{
    PolicyType? Policy { get; }
    ClaimType[] Claims { get; }
    bool ReplaceDefaultClaims { get; }
}
