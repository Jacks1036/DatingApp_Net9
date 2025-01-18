using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService (IConfiguration configuration): ITokenService
{
    public string CreateToke(AppUser user)
    {
        var tokenkey=configuration["TokenKey"]?? throw new Exception("Cannot access the Token Key from App settings");
        if(tokenkey.Length<64) throw new Exception("Your token key needs to be longer");
        var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));

        var claims= new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.UserName )
        };

        var signingCredentials= new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor= new SecurityTokenDescriptor
        {
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.UtcNow.AddDays(8),
            SigningCredentials=signingCredentials
        };

        var tokenHandler= new JwtSecurityTokenHandler();
        var token=tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}
