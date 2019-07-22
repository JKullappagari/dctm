using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DCTMRestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",optional: false,reloadOnChange: true)
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true,reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            //Serilog.Configuration.ILoggerSettings logSettings = configuration.GetSection("Serilog").;
            //Configure logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Logger created");

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Web Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options => options.AddServerHeader = false)
                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    var env = hostingContext.HostingEnvironment;
                //    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                //    config.AddEnvironmentVariables();

                //    Log.Logger = new LoggerConfiguration().ReadFrom.Settings(config.Sources.g["Serilog"]).CreateLogger();
                //})
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddSerilog(dispose: true);
                //    logging.AddConsole();
                //    logging.AddDebug();
                //})
                .UseStartup<Startup>()
                .Build();

    }
}
