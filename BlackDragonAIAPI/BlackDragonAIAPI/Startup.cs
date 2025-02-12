using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Discord;
using BlackDragonAIAPI.Models;
using BlackDragonAIAPI.Models.Validation;
using BlackDragonAIAPI.StorageHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlackDragonAIAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public const string CorsPolicy = "AllowAny";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AuthConfig()
            {
                Secret = this.Configuration.GetSection("AuthConfig").GetValue<string>("Secret")
            });

            services.AddOptions<DiscordConfig>()
                .Bind(this.Configuration.GetSection("DiscordConfig"));
            //            var mongodbClient = new MongoClient(this.Configuration.GetConnectionString("MongoDb"));

            services.AddDbContext<BLBDatabaseContext>(options => 
                options.UseMySql(this.Configuration.GetConnectionString("MySql"), 
                    new MySqlServerVersion(new Version(5, 5, 60))));
            // add connection stuff because life
//            services.AddSingleton<IMongoClient>(mongodbClient);
            services.AddScoped<ICommandService, MySqlCommandService>();
            services.AddScoped<IUserService, MySqlUserService>();
            services.AddScoped<ITimedMessageService, MySqlTimedMessageService>();
            services.AddScoped<IWebhookSubscriberService, MySqlWebhookSubscriberService>();
            services.AddScoped<IDeathCountsService, MySqlDeathCountsService>();
            services.AddScoped<IStreamPlanningService, MysqlStreamPlanningService>();
            services.AddScoped<CommandValidator>();
            services.AddScoped<UserValidator>();
            services.AddScoped<TimedMessageValidator>();
            services.AddScoped<WebhookManager>();
            services.AddSingleton<IDiscordManager, DiscordManager>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(CorsPolicy);
//            app.UseAuthorization();

            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
