using System;
using System.Security.Cryptography;
using DCTMRestAPI.Types;

namespace DCTMRestAPI.Services
{
    public enum PasswordVerificationResult
    {
        Failed,
        Success,
        /// <summary>The password matched a legacy hash and should be re-hashed with the current scheme.</summary>
        SuccessRehashNeeded
    }

    /// <summary>
    /// Password hashing for the login endpoint. New hashes use PBKDF2-SHA256 with a per-user salt
    /// (marked with a "v2:" prefix). Legacy salted-SHA256 hashes (produced by the old fixed-salt
    /// scheme) are still verified, so existing <c>tblUser.Password</c> values keep working; a match
    /// on a legacy hash is reported as <see cref="PasswordVerificationResult.SuccessRehashNeeded"/>.
    /// </summary>
    public class PasswordHasher
    {
        private const string V2Prefix = "v2:";
        private const int Iterations = 210_000;   // OWASP guidance for PBKDF2-HMAC-SHA256
        private const int SaltSize = 16;          // bytes
        private const int KeySize = 32;           // bytes (256-bit derived key)
        private static readonly HashAlgorithmName Prf = HashAlgorithmName.SHA256;

        /// <summary>Produces a current-scheme (v2, PBKDF2-SHA256) hash.</summary>
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Prf, KeySize);

            byte[] packed = new byte[SaltSize + KeySize];
            Buffer.BlockCopy(salt, 0, packed, 0, SaltSize);
            Buffer.BlockCopy(subkey, 0, packed, SaltSize, KeySize);
            return V2Prefix + Convert.ToBase64String(packed);
        }

        /// <summary>Verifies a password against a stored hash (v2 or legacy).</summary>
        public PasswordVerificationResult Verify(string storedHash, string password)
        {
            if (string.IsNullOrEmpty(storedHash))
                return PasswordVerificationResult.Failed;

            try
            {
                if (storedHash.StartsWith(V2Prefix, StringComparison.Ordinal))
                {
                    return VerifyV2(storedHash, password)
                        ? PasswordVerificationResult.Success
                        : PasswordVerificationResult.Failed;
                }

                // Legacy salted-SHA256: VerifyHash extracts the stored salt and recomputes.
                return CryptographyUtil.VerifyHash(password, "SHA256", storedHash)
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private static bool VerifyV2(string storedHash, string password)
        {
            byte[] packed = Convert.FromBase64String(storedHash.Substring(V2Prefix.Length));
            if (packed.Length != SaltSize + KeySize)
                return false;

            byte[] salt = new byte[SaltSize];
            byte[] expected = new byte[KeySize];
            Buffer.BlockCopy(packed, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(packed, SaltSize, expected, 0, KeySize);

            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Prf, KeySize);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
    }
}
