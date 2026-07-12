using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DCTMRestAPI.IntegrationTests
{
    /// <summary>Mints HS256 JWTs signed with the test key, matching what the app validates.</summary>
    public static class TestJwt
    {
        public static string Create(string signingKey, string roles = "Public", string username = "tester")
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("roles", roles),          // matches the app's own token shape
                new Claim(ClaimTypes.Role, roles),  // satisfies [Authorize(Roles = "...")]
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    notBefore: DateTime.UtcNow.AddMinutes(-1),
                    expires: DateTime.UtcNow.AddMinutes(30)));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
