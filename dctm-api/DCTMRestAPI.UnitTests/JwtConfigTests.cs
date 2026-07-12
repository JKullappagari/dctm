using System;
using System.Text;
using DCTMRestAPI.Types;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace DCTMRestAPI.UnitTests
{
    public class JwtConfigTests
    {
        private static IConfiguration ConfigWithKey(string key)
        {
            var mock = new Mock<IConfiguration>();
            mock.Setup(c => c[JwtConfig.SigningKeyPath]).Returns(key);
            return mock.Object;
        }

        [Fact]
        public void GetSigningKey_returns_key_for_valid_configuration()
        {
            const string key = "a-sufficiently-long-signing-key-of-32+bytes-xxxxx";
            var config = ConfigWithKey(key);

            var signingKey = JwtConfig.GetSigningKey(config);

            Assert.Equal(Encoding.UTF8.GetBytes(key), signingKey.Key);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("too-short")] // < 32 bytes
        public void GetSigningKey_throws_for_missing_or_weak_key(string key)
        {
            var config = ConfigWithKey(key);

            var ex = Assert.Throws<InvalidOperationException>(() => JwtConfig.GetSigningKey(config));
            Assert.Contains(JwtConfig.SigningKeyPath, ex.Message);
        }
    }
}
