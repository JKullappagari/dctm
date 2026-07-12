using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DCTMRestAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.HttpOverrides;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using NetEscapades.AspNetCore.SecurityHeaders;
using DCTMRestAPI.Types;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace DCTMRestAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IConfiguration _configuration { get; }

        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
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
            });

            //Authentication
            // Resolve and validate the signing key eagerly so a missing/weak key
            // fails fast at startup rather than on the first authenticated request.
            var signingKey = JwtConfig.GetSigningKey(_configuration);
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
                    IssuerSigningKey = signingKey,

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
            });
            services.AddAuthorization();

            //Require HTTPS (enforced outside Development so both http and https are usable locally)
            if (!_hostingEnvironment.IsDevelopment())
            {
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });
            }

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.2", new OpenApiInfo { Title = "DCTrackMobile REST API", Version = "v1.2" });

                //Shows Authorize button
                //User has to enter Bearer followed by JWT token
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securityScheme, new string[] { } }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    c.IncludeXmlComments(xmlPath);

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

            // MVC controllers with Newtonsoft.Json serialization (preserves .NET Core 2.2 behavior)
            services.AddControllers()
                    .AddNewtonsoftJson();
            //Object mapping (Mapperly, source-generated)
            services.AddSingleton<DctmMapper>();
            //Password hashing (PBKDF2 v2 + legacy verify)
            services.AddSingleton<DCTMRestAPI.Services.PasswordHasher>();
            //Security Headers
            //services.AddCustomHeaders();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Honor X-Forwarded-For / X-Forwarded-Proto from a reverse proxy / IIS so that
            // Request.IsHttps and the client IP are correct behind TLS termination. Must run
            // before the HTTPS redirect and auth so downstream middleware sees the real scheme.
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSecurityHeaders(new HeaderPolicyCollection()
                .AddDefaultSecurityHeaders()
            );



            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Redirect from HTTP to HTTPS (skipped in Development so plain http also works locally)
            if (!env.IsDevelopment())
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-docs";
                c.DocumentTitle = "DCTrackMobile Rest API";

#if DEBUG
                c.SwaggerEndpoint("/swagger/v1.2/swagger.json", "DCTrackMobile REST API V1.2");
#else
                c.SwaggerEndpoint("/dctmrest/swagger/v1.2/swagger.json", "DCTrackMobile REST API v1.2");
#endif

                c.DocExpansion(DocExpansion.None);

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
