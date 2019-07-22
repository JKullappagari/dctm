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
    using Microsoft.Extensions.Logging;

    namespace DCTMRestAPI.Controllers
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
        [Produces("application/json")]
        [Route("api/auth/[controller]")]
        public class TokenController : Controller
        {
            private readonly DCTrackContext _context;
            private readonly ILogger _logger;

            public TokenController(DCTrackContext context,ILogger<TokenController> logger)
            {
                _context = context;
                _logger = logger;
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
            public IActionResult Create([FromBody] TokenModel t)
            {
                try

                {
                    string password = CryptographyUtil.Decrypt(t.Password);
                    if (IsValidUserAndPasswordCombination(t.UserName,password))
                    {
                        string deviceID = string.Empty;
                        if (!string.IsNullOrEmpty(t.DeviceID))
                            deviceID = t.DeviceID;
                        TokenClass token = GenerateToken(t.UserName, deviceID);
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

            /// <summary>
            /// Returns Enrypted string
            /// </summary>
            /// <returns></returns>
            //[ApiExplorerSettings(IgnoreApi = true)]
            [HttpGet("EncryptString/{Text}")]
            [ProducesResponseType(typeof(string), 200)]
            [ProducesResponseType(400)]
            public string GetEncryptString(string Text)
            {
                return CryptographyUtil.Encrypt(Text);
            }


            //[ApiExplorerSettings(IgnoreApi = true)]
            //[HttpGet("DecryptString")]
            //[ProducesResponseType(typeof(string), 200)]
            //[ProducesResponseType(400)]
            //[ProducesResponseType(401)]
            //public string GetDecryptString(string Text)
            //{
            //    return CryptographyUtil.Decrypt(Text);
            //}


            private bool IsValidUserAndPasswordCombination(string username, string password)
            {
                _logger.LogInformation("Checking user credentials...");
                List<TblUser> users = (from g in _context.TblUser
                                       where g.LoginName == username &&
                                       g.Status == true
                                       select g).ToList();

                if (users != null && users.Count > 0)
                {
                    string inputPassword = GetSHA256HashValue(password);
                    string dbPassword = users[0].Password;

                    return !string.IsNullOrEmpty(username) && inputPassword == dbPassword;
                }
                else
                {
                    return false;
                }
            }

            private TokenClass GenerateToken(string username,string deviceID)
            {
                DateTime expDate = DateTime.Now.AddDays(1);
                string expOffset = new DateTimeOffset(expDate).ToUnixTimeSeconds().ToString();
                string roles = string.Empty;
                if (!string.IsNullOrEmpty(deviceID))
                {
                    bool validMobileDevice = (from m in _context.TblMobileDevice
                                              where m.DeviceId.ToLower().CompareTo(deviceID.ToLower()) == 0
                                              && m.Status == true
                                              select m).Count() > 0 ? true : false;
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
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the phar5e us5d for se3uring the t0ken. This is a scr5t")),
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

            private string GetSHA256HashValue(string strInput)
            {
                //byte[] salt = { 652 };

                byte[] intBytes = BitConverter.GetBytes(652);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(intBytes);
                byte[] salt = intBytes;

                String hashVal = CryptographyUtil.ComputeHash(strInput, "SHA256", salt);
                return (hashVal);

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
