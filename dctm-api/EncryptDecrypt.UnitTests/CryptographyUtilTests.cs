using EncryptDecrypt;
using Xunit;

namespace EncryptDecrypt.UnitTests
{
    public class CryptographyUtilTests
    {
        [Theory]
        [InlineData("data source=server;initial catalog=DCTrack;user id=sa;password=Secret123;")]
        [InlineData("simple")]
        public void Encrypt_then_Decrypt_round_trips(string plain)
        {
            var cipher = CryptographyUtil.Encrypt(plain);

            Assert.NotEqual(plain, cipher);
            Assert.Equal(plain, CryptographyUtil.Decrypt(cipher));
        }

        [Fact]
        public void Decrypt_is_backward_compatible_with_legacy_ciphertext()
        {
            // Produced by the original RijndaelManaged implementation (same key/IV). The AES-based
            // replacement must still decrypt it, otherwise previously-encrypted appsettings break.
            const string legacyCipher =
                "sZUqiI04+oGn2cQHNXPi+zSRiFapPkIBg7ujO8N7hx8L3JVyLPKsgdUxTRW8VaoAG88kdAufR6Hiby5j3xhGq7PW51CXSeOr1fz5FkY1v2E=";

            Assert.Equal(
                "data source=1.2.3.4;initial catalog=DCTrack;user id=sa;password=Secret123;",
                CryptographyUtil.Decrypt(legacyCipher));
        }
    }
}
