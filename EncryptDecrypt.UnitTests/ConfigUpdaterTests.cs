using System;
using System.IO;
using EncryptDecrypt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EncryptDecrypt.UnitTests
{
    /// <summary>
    /// Exercises the tool's core behaviour: encrypting and then decrypting the DCTrackDatabase
    /// connection string inside an appsettings.json file. Each phase uses a fresh configuration
    /// (as the CLI does per invocation) because the options snapshot is bound once.
    /// </summary>
    public class ConfigUpdaterTests
    {
        private static ConfigUpdater UpdaterFor(string dir)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("appsettings.json")
                .Build();
            var services = new ServiceCollection();
            services.ConfigureWritable<ConfigSettings>(config.GetSection("ConnectionStrings"), "appsettings.json");
            return new ConfigUpdater(services);
        }

        [Fact]
        public void Encrypt_then_decrypt_round_trips_connection_string_in_file()
        {
            const string plain = "data source=host;initial catalog=DCTrack;user id=sa;password=Secret123;";
            var dir = Path.Combine(Path.GetTempPath(), "encdec_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, "appsettings.json");
            File.WriteAllText(file, "{\"ConnectionStrings\":{\"DCTrackDatabase\":\"" + plain + "\"}}");

            var prevCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(dir);

                UpdaterFor(dir).Update("ConnectionStrings", "DCTrackDatabase", "encrypt");
                var encrypted = ReadConnString(file);
                Assert.DoesNotContain("source", encrypted.ToLowerInvariant());   // no longer plaintext
                Assert.NotEqual(plain, encrypted);

                UpdaterFor(dir).Update("ConnectionStrings", "DCTrackDatabase", "decrypt");
                Assert.Equal(plain, ReadConnString(file));                        // restored
            }
            finally
            {
                Directory.SetCurrentDirectory(prevCwd);
                try { Directory.Delete(dir, true); } catch { /* best effort */ }
            }
        }

        private static string ReadConnString(string file)
        {
            var config = new ConfigurationBuilder().AddJsonFile(file).Build();
            return config.GetConnectionString("DCTrackDatabase");
        }
    }
}
