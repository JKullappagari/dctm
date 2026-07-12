using System;
using DCTMRestAPI.Services;
using DCTMRestAPI.Types;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _hasher = new();

        [Fact]
        public void HashPassword_produces_v2_hash_that_verifies()
        {
            var hash = _hasher.HashPassword("Secret123");

            Assert.StartsWith("v2:", hash);
            Assert.Equal(PasswordVerificationResult.Success, _hasher.Verify(hash, "Secret123"));
        }

        [Fact]
        public void Verify_v2_with_wrong_password_fails()
        {
            var hash = _hasher.HashPassword("Secret123");

            Assert.Equal(PasswordVerificationResult.Failed, _hasher.Verify(hash, "wrong"));
        }

        [Fact]
        public void HashPassword_uses_a_fresh_salt_each_time()
        {
            Assert.NotEqual(_hasher.HashPassword("Secret123"), _hasher.HashPassword("Secret123"));
        }

        [Fact]
        public void Verify_legacy_fixed_salt_hash_reports_rehash_needed()
        {
            // Reproduce the old fixed-salt SHA-256 hash (salt = big-endian bytes of 652).
            byte[] salt = BitConverter.GetBytes(652);
            if (BitConverter.IsLittleEndian) Array.Reverse(salt);
            string legacy = CryptographyUtil.ComputeHash("Secret123", "SHA256", salt);

            Assert.Equal(PasswordVerificationResult.SuccessRehashNeeded, _hasher.Verify(legacy, "Secret123"));
            Assert.Equal(PasswordVerificationResult.Failed, _hasher.Verify(legacy, "wrong"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-valid-base64-!!!")]
        public void Verify_null_or_garbage_fails(string stored)
        {
            Assert.Equal(PasswordVerificationResult.Failed, _hasher.Verify(stored, "x"));
        }
    }
}
