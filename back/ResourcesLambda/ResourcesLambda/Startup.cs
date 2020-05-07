using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Monitor.Core.Settings;
using Monitor.Infra;
using Monitor.Infra.Entities;
using Monitor.Infra.Interfaces.Repository;
using Monitor.Infra.Repositories;
using Monitor.Infra.Services;
using Newtonsoft.Json;
using ResourcesLambda.Services;

namespace ResourcesLambda
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("GlobalCorPolicy",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://monitor-stage.s3-website.us-east-2.amazonaws.com")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var DBHostName = Environment.GetEnvironmentVariable("DBHostName");
            var DBName = Environment.GetEnvironmentVariable("DBName");
            var DBUserName = Environment.GetEnvironmentVariable("DBUserName");
            var DBPassword = Environment.GetEnvironmentVariable("DBPassword");
            var DBPort = Environment.GetEnvironmentVariable("DBPort");

            services.AddDbContext<AppDbContext>(options =>
            {
                if (string.IsNullOrEmpty(DBHostName) || string.IsNullOrEmpty(DBName) ||
                    string.IsNullOrEmpty(DBUserName) || string.IsNullOrEmpty(DBPassword) || string.IsNullOrEmpty(DBPort))
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Monitor.Infra"));
                else                    
                    options.UseNpgsql($"Host={DBHostName};Port={DBPort};Username={DBUserName};Password={DBPassword};Database={DBName};", b => b.MigrationsAssembly("Monitor.Infra"));
            });

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();

            var awsCredential = Configuration.GetSection("AWSCredentials");
            services.Configure<Credentials>(awsCredential, binderOptions => binderOptions.BindNonPublicProperties = true);
            services.AddSingleton(provider => provider.GetService<IOptions<Credentials>>().Value);

            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IResourceHistoryRepository, ResourceHistoryRepository>();
            services.AddScoped<IUserActionRepository, UserActionRepository>();

            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IResourceHistoryService, ResourceHistoryService>();
            services.AddScoped<IUserActionService, UserActionService>();

            //services.AddCognitoIdentity();
            //var RegionStaging = Configuration["AWSCognito-staging:Region"];
            //var PoolIdStaging = Configuration["AWSCognito-production:PoolId"];
            //var AppClientIdStaging = Configuration["AWSCognito-staging:AppClientId"];

            //var RegionProduction = Configuration["AWSCognito-production:Region"];
            //var PoolIdProduction = Configuration["AWSCognito-production:PoolId"];
            //var AppClientIdProduction = Configuration["AWSCognito-production:AppClientId"];

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
            //                {
            //                    // get JsonWebKeySet from AWS
            //                    var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
            //                    // serialize the result
            //                    var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
            //                    // cast the result to be the type expected by IssuerSigningKeyResolver
            //                    return (IEnumerable<SecurityKey>)keys;
            //                },

            //                ValidIssuers = new List<string>
            //                {
            //                    $"https://cognito-idp.{RegionStaging}.amazonaws.com/{PoolIdStaging}",
            //                    $"https://cognito-idp.{RegionProduction}.amazonaws.com/{PoolIdProduction}",
            //                },
            //                ValidateIssuerSigningKey = true,
            //                ValidateIssuer = true,
            //                ValidateLifetime = true,
            //                ValidAudiences = new List<string>
            //                {
            //                    AppClientIdStaging,
            //                    AppClientIdProduction
            //                },
            //                ValidateAudience = true
            //            };
            //        });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin", policy => policy.RequireClaim("custom:groupId", new List<string> { "0" }));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("GlobalCorPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
