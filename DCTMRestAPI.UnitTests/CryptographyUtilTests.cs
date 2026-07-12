using System.Text;
using DCTMRestAPI.Types;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class CryptographyUtilTests
    {
        [Theory]
        [InlineData("data source=server;initial catalog=DCTrack;user id=sa;password=Secret123;")]
        [InlineData("simple")]
        [InlineData("")]
        public void Encrypt_then_Decrypt_round_trips(string plain)
        {
            var cipher = CryptographyUtil.Encrypt(plain);

            Assert.NotEqual(plain, cipher);           // actually transformed
            Assert.Equal(plain, CryptographyUtil.Decrypt(cipher));
        }

        [Fact]
        public void Encrypt_is_deterministic_for_same_input()
        {
            // Fixed key + IV in the util => stable ciphertext (relied on by the connection-string tooling).
            Assert.Equal(CryptographyUtil.Encrypt("abc"), CryptographyUtil.Encrypt("abc"));
        }

        [Fact]
        public void Decrypt_is_backward_compatible_with_legacy_ciphertext()
        {
            // Produced by the original RijndaelManaged implementation (same key/IV). The AES-based
            // replacement must still decrypt it, otherwise existing encrypted connection strings break.
            const string legacyCipher =
                "sZUqiI04+oGn2cQHNXPi+zSRiFapPkIBg7ujO8N7hx8L3JVyLPKsgdUxTRW8VaoAG88kdAufR6Hiby5j3xhGq7PW51CXSeOr1fz5FkY1v2E=";

            Assert.Equal(
                "data source=1.2.3.4;initial catalog=DCTrack;user id=sa;password=Secret123;",
                CryptographyUtil.Decrypt(legacyCipher));
        }

        [Fact]
        public void ComputeHash_with_fixed_salt_is_deterministic()
        {
            var salt = Encoding.UTF8.GetBytes("salt1234");

            var h1 = CryptographyUtil.ComputeHash("password", "SHA256", salt);
            var h2 = CryptographyUtil.ComputeHash("password", "SHA256", salt);

            Assert.Equal(h1, h2);
        }

        [Fact]
        public void VerifyHash_true_for_matching_and_false_for_mismatch()
        {
            var salt = Encoding.UTF8.GetBytes("salt1234");
            var hash = CryptographyUtil.ComputeHash("password", "SHA256", salt);

            Assert.True(CryptographyUtil.VerifyHash("password", "SHA256", hash));
            Assert.False(CryptographyUtil.VerifyHash("wrong", "SHA256", hash));
        }
    }
}
