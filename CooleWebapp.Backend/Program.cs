using AspNet.Security.OpenIdConnect.Primitives;
using CooleWebapp.Auth.Model;
using CooleWebapp.Database;
using CooleWebapp.Database.Model;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddCooleWebappDatabase(builder.Configuration);

builder.Services.AddIdentity<WebappUser, IdentityRole>()
  .AddEntityFrameworkStores<WebappDbContext>()
  .AddDefaultTokenProviders();

builder.Services
  .AddOpenIddict()
  .AddCore(options =>
  {
    options.UseEntityFrameworkCore().UseDbContext<WebappDbContext>();
  })
  .AddServer(options =>
  {
    options
      .SetTokenEndpointUris("/connect/token")
      .AllowPasswordFlow()
      .AllowRefreshTokenFlow()
      .RegisterScopes(
        OpenIdConnectConstants.Scopes.Email,
        OpenIdConnectConstants.Scopes.Profile,
        OpenIddictConstants.Scopes.Roles)
       // Accept anonymous clients (i.e clients that don't send a client_id).
      .AcceptAnonymousClients()
      .AddDevelopmentEncryptionCertificate()
      .AddDevelopmentSigningCertificate()
      .UseAspNetCore()
      .EnableTokenEndpointPassthrough();
    options.IgnoreEndpointPermissions();
  })
  .AddValidation(options =>
  {
    options.UseLocalServer();
    options.UseAspNetCore();
  });

var app = builder.Build();
await DbSetup.InitializeCooleWebappDatabase(app.Services, app.Lifetime.ApplicationStopping);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseOpenApi();
  app.UseSwaggerUi3();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
