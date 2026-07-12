using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DCTMRestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // WebApplication.CreateBuilder already sets up configuration
            // (appsettings.json, appsettings.{Environment}.json, environment variables,
            //  and user secrets in Development) equivalent to the previous ConfigurationBuilder.
            var builder = WebApplication.CreateBuilder(args);

            //Configure logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Logger created");

            try
            {
                Log.Information("Starting web host");

                // Do not emit the Kestrel "Server" response header.
                builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);

                var startup = new Startup(builder.Environment, builder.Configuration);
                startup.ConfigureServices(builder.Services);

                var app = builder.Build();

                startup.Configure(app, app.Environment, app.Services.GetRequiredService<ILoggerFactory>());

                app.Run();
            }
            // Let the WebApplicationFactory / test host stop signals propagate instead of
            // logging them as a fatal crash.
            catch (Exception ex) when (
                ex is not Microsoft.Extensions.Hosting.HostAbortedException &&
                ex.GetType().Name != "StopTheHostException")
            {
                Log.Fatal(ex, "Web Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
