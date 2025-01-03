using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;

namespace AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom;
public class AuthorizationCustomMiddlewareResultHandler(IAuthorizationPolicyProvider policyProvider) : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new AuthorizationMiddlewareResultHandler();
    private readonly IAuthorizationPolicyProvider _policyProvider = policyProvider;

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        var controllerAuthorizeRequirement = context.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeCustomAttribute>();
        var claims = context.User.Claims.ToList();

        if (context.User == null || context.User.Identity?.Name == null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var controllerPolicy = controllerAuthorizeRequirement?.Policy.ToString();
        var controllerClaims = controllerAuthorizeRequirement?.Claims.Select(x => x.ToString());
        if (!string.IsNullOrEmpty(controllerPolicy))
        {
            var defaultAuthorizationClaims = await _policyProvider.GetPolicyAsync(controllerPolicy);
            if (defaultAuthorizationClaims?.Requirements.Count > 0)
            {
                var userPolicy = context.User.Claims.Where(c => c.Type == controllerPolicy);

                if (!userPolicy.Any())
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }

                var requiredClaims = controllerAuthorizeRequirement.ReplaceDefaultClaims
                    ? controllerClaims.ToArray()
                    : defaultAuthorizationClaims.Requirements
                        .OfType<ClaimsAuthorizationRequirement>()
                        .SelectMany(x => x.AllowedValues.SelectMany(value => value.Split(',')))
                        .ToArray()
                        .Concat(controllerClaims)
                        .Distinct();

                if (controllerClaims.Any() || requiredClaims.Any())
                {
                    var userClaims = context.User.Claims
                        .Where(c => c.Type == controllerPolicy)
                        .SelectMany(c => c.Value.Split(","))
                        .Distinct();

                    if (!requiredClaims.All(requiredClaim => userClaims.Contains(requiredClaim)))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        return;
                    }
                }
            }

        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
