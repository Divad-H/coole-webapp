using CooleWebapp.Auth;
using CooleWebapp.Auth.Model;
using CooleWebapp.Backend.ErrorHandling;
using CooleWebapp.Database;
using CooleWebapp.Database.Model;
using Microsoft.AspNetCore.Identity;
using CooleWebapp.EmailService;
using CooleWebapp.Auth.Managers;
using System.Text.Json.Serialization;
using CooleWebapp.Images;
using CooleWebapp.Application.Products;
using System.Globalization;
using CooleWebapp.Application.Shop;
using CooleWebapp.Application.Accounting;
using CooleWebapp.Application.Users;
using CooleWebapp.Application.Dashboard;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

builder.Services.AddControllers(options =>
{
  options.Filters.Add<HttpResponseExceptionFilter>();
}).AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();
builder.Services.AddCooleWebappDatabase(builder.Configuration);
builder.Services.AddImageServices();
builder.Services.AddProductsServices();
builder.Services.AddShopServices();
builder.Services.AddAccountingServices();
builder.Services.AddUsersServices();
builder.Services.AddDashboardServices();

builder.Services
  .AddScoped<IUserRoleStore<WebappUser>, UserStore>();
builder.Services
  .AddScoped<IUserStore<WebappUser>>(sp => sp.GetRequiredService<IUserRoleStore<WebappUser>>());
builder.Services
  .AddScoped<IRoleStore<IdentityRole>, RoleStore>();
builder.Services
  .AddIdentity<WebappUser, IdentityRole>(options =>
  {
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = true;
  })
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
        Scopes.Email,
        Scopes.Profile,
        Scopes.Roles)
       // Accept anonymous clients (i.e clients that don't send a client_id).
      .AcceptAnonymousClients()
      .UseAspNetCore()
      .EnableTokenEndpointPassthrough();

    if (builder.Environment.IsDevelopment())
    {
      options
        .AddDevelopmentEncryptionCertificate()
        .AddDevelopmentSigningCertificate();
    }
    else
    {
      var thumbprints = builder.Configuration
        .GetValue<string>("WEBSITE_LOAD_CERTIFICATES")?
        .Split(',', StringSplitOptions.TrimEntries);
      var directory = builder.Configuration
        .GetValue<string>("WEBSITE_PRIVATE_CERTS_PATH");
      if (thumbprints is not null && thumbprints.Length >= 2)
      {
        var certificates = thumbprints
          .Select(t => Path.Combine(directory ?? "/var/ssl/private/", $"{t}.p12"))
          .Select(path =>
          {
            var bytes = File.ReadAllBytes(path);
            return new X509Certificate2(bytes);
          })
          .ToArray();

        options
          .AddEncryptionCertificate(certificates[1])
          .AddSigningCertificate(certificates[0]);
      }
    }

    options.IgnoreEndpointPermissions();
  })
  .AddValidation(options =>
  {
    options.UseLocalServer();
    options.UseAspNetCore();
  });

builder.Services.AddWebappAuth(builder.Configuration);

builder.Services.AddEmailSender(builder.Configuration);

var app = builder.Build();
await DbSetup.InitializeCooleWebappDatabase(app.Services, app.Lifetime.ApplicationStopping);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseOpenApi();
  app.UseSwaggerUi3();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
