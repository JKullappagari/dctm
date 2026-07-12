using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace EncryptDecrypt
{
    class Program
    {
        //
        static void Main(string[] args)
        {
            // Program.exe <-g|--greeting|-$ <greeting>> [name <fullname>]
            // [-?|-h|--help] [-u|--uppercase]
            CommandLineApplication app =
              new CommandLineApplication();
            app.UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue;
            app.Name = "cryptosettings";
            app.HelpOption("-? | -h | --help");
            //CommandArgument names = null;
            CommandOption encrypt = app.Option(
               "-pe <ConnectionStrings_section_name>",
               "Specify Connectionstrings section name of appsettings json file to encrypt value of specified Key",
               CommandOptionType.SingleValue);

            CommandOption decrypt = app.Option(
             "-pd <ConnectionStrings_section_name>",
             "Specify Connectionstrings section name of appsettings json file to decrypt value of specified Key",
             CommandOptionType.SingleValue);

            CommandOption keyName = app.Option(
            "-key <ConnectionStrings_key_name>",
            "Specify key name of ConnectionStrings section of appsettings json file",
            CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                if (encrypt.HasValue())
                {
                    try
                    {
                        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                            .Build();

                        var serviceCollection = new ServiceCollection();


                        if (keyName.HasValue())
                        {
                            // create service collection
                            
                            serviceCollection.ConfigureWritable<ConfigSettings>(configuration.GetSection(encrypt.Value()), "appsettings.json");
                            //serviceCollection.AddScoped<IWritableOptions<ConfigSettings>>();
                            var serviceProvier = serviceCollection.BuildServiceProvider();

                            ConfigUpdater updater = new ConfigUpdater(serviceCollection);
                            updater.Update(encrypt.Value(), keyName.Value(), "encrypt");

                        }
                        else
                        {

                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                else if (decrypt.HasValue())
                {
                    try
                    {
                        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                            .Build();

                        var serviceCollection = new ServiceCollection();


                        if (keyName.HasValue())
                        {
                            // create service collection

                            serviceCollection.ConfigureWritable<ConfigSettings>(configuration.GetSection(decrypt.Value()), "appsettings.json");
                            //serviceCollection.AddScoped<IWritableOptions<ConfigSettings>>();
                            var serviceProvier = serviceCollection.BuildServiceProvider();

                            ConfigUpdater updater = new ConfigUpdater(serviceCollection);
                            updater.Update(decrypt.Value(), keyName.Value(), "decrypt");

                        }
                        else
                        {

                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                else
                {
                    Console.WriteLine("-pe or -pd option with key name is must");
                }
                return 0;
            });
            app.Execute(args);
        }


    }

    public class ConfigSettings
    {
        public string DCTrackDatabase { get; set; }
    }


    public class ConfigUpdater
    {
        private readonly IWritableOptions<ConfigSettings> _options;

        public ConfigUpdater(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            _options = sp.GetService<IWritableOptions<ConfigSettings>>();
        }

        public void Update(string Section, string Key, string Value)
        {
            
            switch (Section)
            {

                case "ConnectionStrings":
                    string value = string.Empty;
                    if(Value.CompareTo("encrypt") == 0)
                    {
                        ConfigSettings original = (ConfigSettings)_options.Value;
                        if (!string.IsNullOrEmpty(original.DCTrackDatabase))
                        {
                            if(original.DCTrackDatabase.ToLower().Contains("source"))
                            {
                                value = CryptographyUtil.Encrypt(original.DCTrackDatabase);
                                _options.Update(opt => { opt.DCTrackDatabase = value; });
                                Console.WriteLine("Encrypted successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Already encrypted!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ConnectionStrings item DCTrackDatabase is missing");
                        }
                    }
                    else if(Value.CompareTo("decrypt") == 0)
                    {
                        ConfigSettings original = (ConfigSettings)_options.Value;
                        if (!string.IsNullOrEmpty(original.DCTrackDatabase))
                        {
                            if (original.DCTrackDatabase.ToLower().Contains("source"))
                            {
                                Console.WriteLine("Already decrypted!");
                            }
                            else
                            {
                                value = CryptographyUtil.Decrypt(original.DCTrackDatabase);
                                _options.Update(opt => { opt.DCTrackDatabase = value; });
                                Console.WriteLine("Decrypted successfully!");
                            }

                        }
                        else
                        {
                            Console.WriteLine("ConnectionStrings item DCTrackDatabase is missing");
                        }
                    }
                    
                    break;
            }

        }
    }

}