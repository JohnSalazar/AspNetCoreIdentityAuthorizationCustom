using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom;
using AspNetCoreIdentityAuthorizationCustom.AuthorizeCustom.Types;
using AspNetCoreIdentityAuthorizationCustom.Data;
using AspNetCoreIdentityAuthorizationCustom.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalCustomFactory>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationCustomMiddlewareResultHandler>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(nameof(PolicyType.AdminFull), policy => policy.RequireClaim(nameof(PolicyType.AdminFull), $"{ClaimType.Create},{ClaimType.Read},{ClaimType.Update},{ClaimType.Delete},{ClaimType.Admin}"))
    .AddPolicy(nameof(PolicyType.Admin), policy => policy.RequireClaim(nameof(PolicyType.Admin), $"{ClaimType.Create},{ClaimType.Read},{ClaimType.Update},{ClaimType.Delete}"))
    .AddPolicy("BROnly", policy => policy.RequireClaim("Country", "BR"))
    .AddPolicy("Over15", policy => policy.RequireAssertion(context =>
                                                            context.User.HasClaim(c =>
                                                            c.Type == "Age" && int.Parse(c.Value) >= 15
                                                        )));

builder.Services.AddControllers(
    config =>
    {
        var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

        config.Filters.Add(new AuthorizeFilter(policy));
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapIdentityApi<IdentityUser>();

app.MapPost("/add-seed-claims", async (UserManager<IdentityUser> userManager, string email) =>
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        return Results.NotFound();
    }

    await userManager.AddClaimAsync(user, new Claim(nameof(PolicyType.Admin), $"{ClaimType.Create},{ClaimType.Read},{ClaimType.Update},{ClaimType.Delete}"));

    return Results.Ok();
});

app.MapGet("/get-claims", async (UserManager<IdentityUser> userManager, string email) =>
{

    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        return Results.NotFound();
    }

    var claims = await userManager.GetClaimsAsync(user);

    return Results.Ok(claims);
});

app.MapGet("/weatherforecastminimal", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
         new WeatherForecast
         (
             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
             Random.Shared.Next(-20, 55),
             summaries[Random.Shared.Next(summaries.Length)]
         ))
         .ToArray();
    return forecast;
})
.WithName("GetWeatherForecastMinimal")
.RequireAuthorizationCustom();
//.RequireAuthorizationCustom(PolicyType.Admin);
//.RequireAuthorizationCustom(PolicyType.Admin, [ClaimType.Admin], replaceDefaultClaims: true);
//.RequireAuthorizationCustom(PolicyType.Admin, [ClaimType.Create, ClaimType.Read, ClaimType.Update]);

app.MapGet("/logout", async (SignInManager<IdentityUser> signedInManager) =>
{
    await signedInManager.SignOutAsync();
    return Results.Ok("Logged out");
})
.WithName("Logout")
.RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
