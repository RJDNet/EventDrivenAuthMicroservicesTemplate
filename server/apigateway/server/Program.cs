using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using ApiGateway.Data;
using ApiGateway.Services;

var builder = WebApplication.CreateBuilder(args);

// DB Connection
var environmentConnectionString = Environment.GetEnvironmentVariable("DefaultConnection");
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Domains
string allowedClients = builder.Configuration["AllowedClients"];
string hostServer = builder.Configuration["HostServer"];

// Auth Providers
string googleClientId = builder.Configuration["ExternalAuthProviders:Google:ClientId"];
string googleClientSecret = builder.Configuration["ExternalAuthProviders:Google:ClientSecret"];

builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    // Can use environment variable.
    if(environmentConnectionString != null) {
        options.UseSqlServer(environmentConnectionString);
    } else { 
        options.UseSqlServer(connectionString);
    }
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Lax log in settings for development
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication();

builder.Services.ConfigureApplicationCookie(options =>
{
    string cookieName = "auth_cookie";

    options.Cookie.Name = cookieName;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
});

builder.Services.AddAntiforgery(options => 
{
    string headerName = "X-XSRF-TOKEN";

    options.HeaderName = headerName;
});

builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(new ValidateAntiForgeryTokenAttribute());
});

// Add Rpc Service
builder.Services.AddSingleton<IRpcClientService, RpcClientService>();
builder.Services.AddHostedService<RpcClientBackgroundService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();    

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use(async (context, next) =>
{
    var requestPath = context.Request.Path.Value;
    string referer = context.Request.Headers.Referer;
    string csrfTokenApiPath = "/api/csrftoken/getcsrftoken";
    string csrfTokenCookieName = "XSRF-TOKEN";

    if(referer.Contains(allowedClients, StringComparison.OrdinalIgnoreCase) &&
    hostServer.Contains(context.Request.Host.Value, StringComparison.OrdinalIgnoreCase) && 
    string.Equals(requestPath, csrfTokenApiPath, StringComparison.OrdinalIgnoreCase))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append(
            csrfTokenCookieName, 
            tokenSet.RequestToken!,
            new CookieOptions { 
                HttpOnly = false, 
                SameSite = SameSiteMode.Strict, 
                Secure = true 
            }
        );
    }

    await next(context);
});

app.MapControllers();

app.Run();
