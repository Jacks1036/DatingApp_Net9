using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static class IdentityServiceExtentions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenkey = configuration["TokenKey"] ?? throw new Exception("Cannot access the Token Key from App settings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey)),
            ValidateAudience = false,
            ValidateIssuer = false

        };

    });
        return services;

    }

}
