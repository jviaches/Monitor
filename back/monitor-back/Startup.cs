using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using monitor_core.Settings;
using monitor_infra;
using monitor_infra.Repositories;
using monitor_infra.Repositories.Interfaces;
using monitor_infra.Services;
using monitor_infra.Services.Interfaces;

namespace monitor_back
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("GlobalCorPolicy",
                builder =>
                {
                    builder
                           //.WithOrigins("http://localhost:4200", "https://d3mbies4lx0e41.cloudfront.net")
                           .AllowAnyHeader()
                           .SetIsOriginAllowed(origin => true)
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });

            services.AddControllers();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("monitor-infra"));
            });

            // configure strongly typed settings objects
            var tokenSection = Configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(tokenSection);

            // configure jwt authentication
            var appSettings = tokenSection.Get<TokenSettings>();
            services.AddSingleton<TokenSettings>(appSettings);

            var encryptionSection = Configuration.GetSection("EncryptionSettings");
            services.Configure<EncryptionSettings>(encryptionSection);
            var encryptionSettings = encryptionSection.Get<EncryptionSettings>();
            services.AddSingleton<EncryptionSettings>(encryptionSettings);


            var key = Encoding.ASCII.GetBytes(appSettings.SigningKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add new services and repositories
            // AddScoped adds your service to the service container using the scoped lifecycle. 
            // This means that a new instance of the TodoItemService class will be created during each web request. 

            // Repositories
            services.AddScoped<ICommunicationChanelRepository, CommunicationChanelRepository>();
            services.AddScoped<IMonitorHistoryRepository, MonitorHistoryRepository>();
            services.AddScoped<IMonitorItemRepository, MonitorItemRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IUserActionRepository, UserActionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserActionService, UserActionService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IMonitorItemService, MonitorItemService>();

            // Infra Services
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("GlobalCorPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}