using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCTMRestAPI.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authorization;
    using System.ComponentModel.DataAnnotations;
    using global::DCTMRestAPI.Models;
    using global::DCTMRestAPI.Types;
    using global::DCTMRestAPI.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.EntityFrameworkCore;

    namespace DCTMRestAPI.Controllers
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
        [Produces("application/json")]
        [Route("api/auth/[controller]")]
        public class TokenController : Controller
        {
            private readonly DCTrackContext _context;
            private readonly ILogger _logger;
            private readonly IConfiguration _configuration;
            private readonly PasswordHasher _passwordHasher;

            public TokenController(DCTrackContext context, ILogger<TokenController> logger, IConfiguration configuration, PasswordHasher passwordHasher)
            {
                _context = context;
                _logger = logger;
                _configuration = configuration;
                _passwordHasher = passwordHasher;
            }

            /// <summary>
            /// To generate jwt token to access DCTM rest api
            /// </summary>
            /// <param name="t">TokenModel</param>
            /// <returns></returns>
            /// <remarks>
            /// deviceID of TokenModel is optional. deviceID represents mobile client unique id which is already registered in DCTM
            /// 
            /// </remarks>
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] TokenModel t)
            {
                try

                {
                    // Transport of the password:
                    //  - Default: plaintext over TLS (TLS already provides transport confidentiality).
                    //  - Legacy clients that AES-encrypt the password with the shared key opt in with
                    //    the "X-Password-Encoding: aes" header.
                    bool aesEncrypted = string.Equals(
                        Request.Headers["X-Password-Encoding"].ToString(), "aes", StringComparison.OrdinalIgnoreCase);
                    string password = aesEncrypted ? CryptographyUtil.Decrypt(t.Password) : t.Password;

                    if (await IsValidUserAndPasswordCombination(t.UserName, password))
                    {
                        string deviceID = string.Empty;
                        if (!string.IsNullOrEmpty(t.DeviceID))
                            deviceID = t.DeviceID;
                        TokenClass token = await GenerateToken(t.UserName, deviceID);
                        //return new ObjectResult();
                        return Ok(new
                        {
                            tokentype = token.TokenType,
                            token = token.Token,
                            expiration = token.Expiration,
                            roles = token.Roles
                        });
                    }
                    else
                        return BadRequest("Invalid user name or password");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Token validation failed");
                    return BadRequest("Token validation failed");
                }
            }

            // NOTE: The anonymous "EncryptString" endpoint was removed — it was an encryption oracle
            // that exposed the shared symmetric key to any caller. Do not reintroduce it.

            //[ApiExplorerSettings(IgnoreApi = true)]
            //[HttpGet("DecryptString")]
            //[ProducesResponseType(typeof(string), 200)]
            //[ProducesResponseType(400)]
            //[ProducesResponseType(401)]
            //public string GetDecryptString(string Text)
            //{
            //    return CryptographyUtil.Decrypt(Text);
            //}


            private async Task<bool> IsValidUserAndPasswordCombination(string username, string password)
            {
                _logger.LogInformation("Checking user credentials...");
                List<TblUser> users = await (from g in _context.TblUser
                                       where g.LoginName == username &&
                                       g.Status == true
                                       select g).ToListAsync();

                if (string.IsNullOrEmpty(username) || users == null || users.Count == 0)
                    return false;

                var result = _passwordHasher.Verify(users[0].Password, password);
                if (result == PasswordVerificationResult.Failed)
                    return false;

                // Opt-in upgrade of legacy hashes on successful login. OFF by default: tblUser.Password
                // is shared with the main DCTrack application, which must understand the v2 format first.
                if (result == PasswordVerificationResult.SuccessRehashNeeded
                    && _configuration.GetValue<bool>("Security:UpgradePasswordHashesOnLogin"))
                {
                    // Raw UPDATE (not SaveChanges): tblUser has a trigger, and EF Core's UPDATE...OUTPUT
                    // is rejected by SQL Server on trigger tables. A plain parameterized UPDATE is safe.
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE dbo.tblUser SET Password = {0} WHERE UserID = {1}",
                        _passwordHasher.HashPassword(password), users[0].UserId);
                }
                return true;
            }

            private async Task<TokenClass> GenerateToken(string username,string deviceID)
            {
                DateTime expDate = DateTime.Now.AddDays(1);
                string expOffset = new DateTimeOffset(expDate).ToUnixTimeSeconds().ToString();
                string roles = string.Empty;
                if (!string.IsNullOrEmpty(deviceID))
                {
                    bool validMobileDevice = (await (from m in _context.TblMobileDevice
                                              where m.DeviceId.ToLower() == deviceID.ToLower()
                                              && m.Status == true
                                              select m).CountAsync()) > 0 ? true : false;
                    if (validMobileDevice)
                        roles = "Mobile";
                    else
                        roles = "Public";
                }
                else
                    roles = "Public";
                var claims = new Claim[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expOffset),
                new Claim("roles",roles)
                };

                var token = new JwtSecurityToken(
                    new JwtHeader(new SigningCredentials(
                        JwtConfig.GetSigningKey(_configuration),
                                                 SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims));

                //return new JwtSecurityTokenHandler().WriteToken(token);

                TokenClass tokenClass = new TokenClass();
                tokenClass.TokenType = "Bearer";
                tokenClass.Token = new JwtSecurityTokenHandler().WriteToken(token);
                tokenClass.Expiration = token.ValidTo;
                tokenClass.Roles = roles;
                return tokenClass;
            }
        }

       
        public class TokenClass
        {
            public string TokenType { get; set; }
            public string Token { get; set; }

            public DateTime Expiration { get; set; }
            public string Roles { get; set; }


        }

    }
}
