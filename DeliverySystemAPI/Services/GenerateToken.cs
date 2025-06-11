
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliverySystemAPI.Services
{
    public class GenerateToken
    {
        public string Generate_Token(string user_name,string user_role)
        {
            string secret_key = "nk7863711ksso3111a4e4133zwehtet9"; //Secret key which will be used later during validation    
            byte[] key = Convert.FromBase64String(secret_key);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user_name),
                    new Claim(ClaimTypes.Role, user_role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),//token expires after 1 minutes
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
