using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockPlatform.Api.Settings;
using StockPlatform.Data.Interfaces;
using StockPlatform.Data.Models;
using StockPlatform.Data.Repositories;
using StockPlatform.Domain.Interfaces;
using StockPlatform.Domain.Services;
using StockPlatform.Infrastructure.Mapping.Profiles;

namespace StockPlatform.Api
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
            var connection = Configuration["DbConnectionString"];
            services.AddDbContext<StockComparisonDbContext>(options => options.UseSqlServer(connection));

            services.AddControllers();//.AddNewtonsoftJson();

            services.AddAutoMapper(new[] { typeof(StockProfile) });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.WithOrigins("https://localhost:44348");
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            });

            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IStockHistoricalDataRepository, StockHistoricalDataRepository>();
            services.AddScoped<IStockHistoricalDataService, StockHistoricalDataService>();

            services.AddScoped<IStockRetriever, StockRetriever>();
            services.AddScoped<IStockPerformanceCalculator, StockPerformanceCalculator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("SiteCorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
