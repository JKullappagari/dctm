using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DCTMRestAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Rewrite;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using NetEscapades.AspNetCore.SecurityHeaders;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using DCTMRestAPI.Types;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting.Internal;
using Swashbuckle.AspNetCore.SwaggerGen;
using DCTMRestAPI.Extensions;

namespace DCTMRestAPI
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration _configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            configuration = new ConfigurationBuilder()
                                    .SetBasePath(hostingEnvironment.ContentRootPath)
                                    .AddJsonFile("appsettings.json")
                                    //.AddCustomConfiguration(hostingEnvironment.ContentRootPath, optional: true, reloadOnChange: true)
                                    .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                                    .Build();

            //var builder = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            //.AddCustomConfiguration(hostingEnvironment.ContentRootPath,false,true)
            //.AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true);
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //logging
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            // EF Database Context
            services.AddDbContext<DCTrackContext>(options =>
            {
                string connString = _configuration.GetConnectionString("DCTrackDatabase");

                if (connString.ToLower().Contains("source"))
                    options.UseSqlServer(connString);
                else
                    options.UseSqlServer(CryptographyUtil.Decrypt(connString));

                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning));
            });
            //Paging
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                                           .ActionContext;
                return new UrlHelper(actionContext);
            });

            //Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Jwt";
                options.DefaultChallengeScheme = "Jwt";
            }).AddJwtBearer("Jwt", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the phar5e us5d for se3uring the t0ken. This is a scr5t")),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });

            //Require HTTPS
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.2", new Info { Title = "DCTrackMobile REST API", Version = "v1.2" });
                c.OperationFilter<FormFileSwaggerFilter>();

                //Shows Authorize button
                //User has to enter Bearer followed by JWT token
                //this will be backup option --
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header"

                });


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
            //IIS Option
            services.Configure<IISOptions>(options =>
            {
            });
            //Data Protection
            services.AddDataProtection()
                .UseCryptographicAlgorithms(
                new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });
            // Custom class for accessing Congfiguration section
            //services.Configure<SecurityOptions>(Configuration.GetSection("SecurityOptions"));

            services.AddMvc();
            //AutoMapper
            services.AddAutoMapper();
            //Security Headers
            //services.AddCustomHeaders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseSecurityHeaders(new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
            );



            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler();
            //}
            //Redirect from HTTP to HTTPS
            var options = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(options);
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-docs";
                c.DocumentTitle = "DCTrackMobile Rest API";

                //c.IndexStream = () => new FileStream($"{AppDomain.CurrentDomain.BaseDirectory}/wwwroot/swagger/ui/swagger.html", FileMode.Open);

#if DEBUG
                c.SwaggerEndpoint("/swagger/v1.2/swagger.json", "DCTrackMobile REST API V1.2");
#else
                c.SwaggerEndpoint("/dctmrest/swagger/v1.2/swagger.json", "DCTrackMobile REST API v1.2");
#endif

#if DEBUG
                //c.InjectOnCompleteJavaScript("../swagger-ui/authorization1.js"); 
                //c.InjectJavascript("../swagger-ui/authorization1.js");
#else
                    //c.InjectJavascript("../swagger-ui/authorization1.js");
#endif
                c.DocExpansion(DocExpansion.None);

            });



            // exception handling
            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500; // or another Status accordingly to Exception Type
            //        context.Response.ContentType = "application/json";

            //        var error = context.Features.Get<IExceptionHandlerFeature>();
            //        if (error != null)
            //        {
            //            var ex = error.Error;

            //            await context.Response.WriteAsync(new ErrorInfo()
            //            {
            //                Entity = 
            //                Code = < your custom code based on Exception Type >,
            //                Message = ex.Message // or your custom message
            //                // other custom data
            //            }.ToString(), Encoding.UTF8);
            //        }
            //    });
            //});
            app.UseMvc();
        }
    }

    internal static class SwashbuckleExtensions
    {
        public static void InjectJavascript(this SwaggerUIOptions options, string path, string type = "text/javascript")
        {
            var builder = new StringBuilder(options.HeadContent);
            builder.AppendLine($"<script src='{path}' type='{type}'></script>");
            options.HeadContent = builder.ToString();
        }
    }

    internal class BasicAuthDocumentFilter : IDocumentFilter
    {
        // BasicAuthDocumentFilter.cs

        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirements = new Dictionary<string, IEnumerable<string>>()
        {
            { "basic", new string[] { } }  // in swagger you specify empty list unless using OAuth2 scopes
        };

            swaggerDoc.Security = new[] { securityRequirements };
        }

    }
}
