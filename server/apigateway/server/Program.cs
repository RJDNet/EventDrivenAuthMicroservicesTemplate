using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;
using ApiGateway.Data;
using ApiGateway.Services;
using Microsoft.Extensions.Primitives;

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
    // Need to pass the environment variable in here.
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
    // Lax sign in settings for development
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;

    // // Password settings.
    // options.Password.RequireDigit = true;
    // options.Password.RequireLowercase = true;
    // options.Password.RequireNonAlphanumeric = true;
    // options.Password.RequireUppercase = true;
    // options.Password.RequiredLength = 6;
    // options.Password.RequiredUniqueChars = 1;

    // // Lockout settings.
    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // options.Lockout.MaxFailedAccessAttempts = 5;
    // options.Lockout.AllowedForNewUsers = true;

    // // User settings.
    // options.User.AllowedUserNameCharacters =
    // "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    // options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthentication(options =>
{
    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
// .AddJwtBearer(options =>
// {
//     // options.Audience = "http://localhost:5001/";
//     // options.Authority = "http://localhost:5000/";

//     // options.Audience = "orders";
//     // options.Authority = identityUrl;

//     options.SaveToken = true;
//     options.RequireHttpsMetadata = false;
//     options.TokenValidationParameters = new TokenValidationParameters()
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidAudience = builder.Configuration["JWT:ValidAudience"],
//         ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//     };
// });
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = googleClientId;
    googleOptions.ClientSecret = googleClientSecret;
});
// .AddFacebook(facebookOptions =>
// {
//     IConfigurationSection FBAuthNSection = builder.Configuration.GetSection("Authentication:FB");
//     facebookOptions.ClientId = FBAuthNSection["ClientId"];
//     facebookOptions.ClientSecret = FBAuthNSection["ClientSecret"];
// })
// .AddMicrosoftAccount(microsoftOptions =>
// {
//     microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
//     microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
// })
// .AddTwitter(twitterOptions =>
// {
//     twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
//     twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
//     twitterOptions.RetrieveUserDetails = true;
// });

// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.All;
// });

builder.Services.AddCors();

builder.Services.ConfigureApplicationCookie(options =>
{
    string cookieName = "auth_cookie";

    options.Cookie.Name = cookieName;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
    // options.Cookie.Domain = "http://localhost:5000";
    // options.Cookie.Path = "http://localhost:3000/api/";

    // options.LoginPath = "/Identity/Account/Login";
    // options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAntiforgery(options => 
{
    string headerName = "X-XSRF-TOKEN";

    options.HeaderName = headerName;
});

builder.Services.AddControllersWithViews(options => {
    options.Filters.Add(new ValidateAntiForgeryTokenAttribute());
});

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Add Rpc Service
builder.Services.AddSingleton<IRpcClientService, RpcClientService>();
builder.Services.AddHostedService<RpcClientBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    //app.UseForwardedHeaders();
} 
else
{
    // When in Production, use HTTPS.
    //app.UseHsts();
    //app.UseHttpsRedirection();
}

app.UseCors(configurePolicy => 
{
    if(allowedClients != null) {
        configurePolicy.WithOrigins("http://localhost:3000", "http://localhost:3001");
    }
    configurePolicy.AllowAnyMethod();
    configurePolicy.AllowCredentials();
    configurePolicy.AllowAnyHeader();
});

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
