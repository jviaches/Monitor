using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Monitor.Core.Interfaces;
using Monitor.Core.Settings;
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
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();

            var awsCredential = Configuration.GetSection("AWSCredentials");
            services.Configure<Credentials>(awsCredential, binderOptions => binderOptions.BindNonPublicProperties = true);
            services.AddSingleton(provider => provider.GetService<IOptions<Credentials>>().Value);
            services.AddScoped<IResourceService, ResourceService>();

            //services.AddCognitoIdentity();
            var Region = Configuration["AWSCognito:Region"];
            var PoolId = Configuration["AWSCognito:PoolId"];
            var AppClientId = Configuration["AWSCognito:AppClientId"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                            {
                                // get JsonWebKeySet from AWS
                                var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                                // serialize the result
                                var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                                // cast the result to be the type expected by IssuerSigningKeyResolver
                                return (IEnumerable<SecurityKey>)keys;
                            },

                            ValidIssuer = $"https://cognito-idp.{Region}.amazonaws.com/{PoolId}",
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidAudience = AppClientId,
                            ValidateAudience = true
                        };
                    });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Admin", policy => policy.RequireClaim("custom:groupId", new List<string> { "0" }));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
