using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SafeIn_mvs_test.Models;
using SafeIn_mvs_test.Repositories;
using SafeIn_mvs_test.Services;
using Segment.Model;
using System;
using System.Globalization;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddSingleton<IUserManagementService, UserManagementService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
builder.Services.AddMvc().AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix)
                                        .AddDataAnnotationsLocalization();

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<RequestLocalizationOptions>(
    opt =>
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("uk")
        };
        opt.DefaultRequestCulture = new RequestCulture("en");
        opt.SupportedCultures = supportedCultures;
        opt.SupportedUICultures = supportedCultures;
    });

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = "Cookies";
//    options.DefaultChallengeScheme = "oidc";
//})
//.AddCookie("Cookies")
//.AddOpenIdConnect("oidc", options =>
//{
//    options.Authority = "https://your-authorization-server.com";
//    options.ClientId = "your-client-id";
//    options.ClientSecret = "your-client-secret";
//    options.ResponseType = "code";
//    options.Scope.Add("openid");
//    options.Scope.Add("profile");
//    options.Scope.Add("email");
//    options.SaveTokens = true;
//    options.GetClaimsFromUserInfoEndpoint = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        NameClaimType = "name"
//    };
//    options.Events = new OpenIdConnectEvents
//    {
//        OnTokenValidated = async context =>
//        {
//            var oldTokens = new Tokens {
               
//                AccessToken = await context.HttpContext.GetTokenAsync("access_token"),
//                RefreshToken = await context.HttpContext.GetTokenAsync("refresh_token")
//            };
//            var newTokens = await "https://localhost:7090/"
//            .AppendPathSegment("api/token")
//            .PostJsonAsync(oldTokens).ReceiveJson<Tokens>();

//            // Update the authentication cookie with the new tokens
//            var authenticationProperties = new AuthenticationProperties();
//            authenticationProperties.UpdateTokenValue("refresh_token", newTokens.RefreshToken);
//            authenticationProperties.UpdateTokenValue("access_token", newTokens.AccessToken);
//            await context.HttpContext.SignInAsync(context.Principal, authenticationProperties);
//        }
//    };
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserManagement}/{action=Login}/{email?}");

app.Run();
