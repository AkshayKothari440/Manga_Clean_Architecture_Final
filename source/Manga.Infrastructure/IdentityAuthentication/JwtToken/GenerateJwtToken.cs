using Manga.Application.Authentication;
using Manga.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manga.Infrastructure.IdentityAuthentication.JwtToken
{
    public class GenerateJwtToken:IGenerateToken
    {
        private readonly JWTConfig jwtConfig;
        public GenerateJwtToken(JWTConfig jwtConfig)
        {
            this.jwtConfig = jwtConfig;
        }

        async Task<string> IGenerateToken.GetToken(string username, AppIdentityUser user)
        {
            var claims=new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,username),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtConfig.JwtExpireDays));

            var token = new JwtSecurityToken(
               jwtConfig.JwtIssuer,
               jwtConfig.JwtIssuer,
               claims,
               expires: expires,
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
