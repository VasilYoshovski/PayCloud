using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayCloud.Services.Identity.Contracts;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PayCloud.Services.Identity
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration config;

        public TokenManager(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(string username, string role, string userId)
        {
            var secret = config.GetSection("TokenSecrets:JWT").Value;

            byte[] key = Convert.FromBase64String(secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                      new Claim(ClaimTypes.Name, username),
                      new Claim("userRole", role),
                      new Claim("userId", userId)}
                ),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var secret = config.GetSection("TokenSecrets:JWT").Value;

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(secret);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out SecurityToken securityToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                var secret = config.GetSection("TokenSecrets:JWT").Value;

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                {
                    return false;
                }

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

