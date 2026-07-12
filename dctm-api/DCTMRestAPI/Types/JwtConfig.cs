using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DCTMRestAPI.Types
{
    /// <summary>
    /// Central access point for the JWT signing key.
    /// The key is read from configuration (never hard-coded in source) and is
    /// validated to be strong enough for HMAC-SHA256 (at least 256 bits / 32 bytes).
    /// Both token issuance (TokenController) and token validation (Startup) resolve
    /// the key through here so they can never drift apart.
    /// </summary>
    public static class JwtConfig
    {
        /// <summary>Configuration path of the signing key (env var: Jwt__SigningKey).</summary>
        public const string SigningKeyPath = "Jwt:SigningKey";

        // HMAC-SHA256 requires a key of at least 256 bits.
        private const int MinKeyBytes = 32;

        public static SymmetricSecurityKey GetSigningKey(IConfiguration configuration)
        {
            var key = configuration[SigningKeyPath];

            if (string.IsNullOrWhiteSpace(key) || Encoding.UTF8.GetByteCount(key) < MinKeyBytes)
            {
                throw new InvalidOperationException(
                    $"JWT signing key is missing or too short. Provide a high-entropy random value of at least " +
                    $"{MinKeyBytes} bytes at configuration key '{SigningKeyPath}'. " +
                    $"Use user secrets in Development and the environment variable 'Jwt__SigningKey' " +
                    $"(or a secret store such as Azure Key Vault) in Production.");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
