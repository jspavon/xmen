using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xmen.api.Filters;
using xmen.api.Models.Abstracts;
using xmen.Api.Swagger;
using xmen.common.Configuration;
using xmen.core.Interfaces;
using xmen.core.Services;
using xmen.Infrastructure.Context;
using xmen.Infrastructure.Repository;
using xmen.Infrastructure.UnitOfWork;

namespace xmen.api
{
    public class Startup
    {


        private IConfiguration Configuration { get; }


        public Startup(Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            #region Register (dependency injection)

            // DataContext of Database.
            services.AddDbContext<IDataContext, CoreContext>();

            services.AddScoped<ILogger, Logger<CoreContext>>();

            // CustomerRepository await UnitofWork parameter ctor explicit
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();

            //Services
            services.AddScoped<IMutantService, MutantService>();


            // Infrastructure                       
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient(typeof(IRepositoryData<>), typeof(RepositoryData<>));
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();

            #endregion

            ConfigureCorsService(ref services);

            services.AddDataProtection();

            services.AddRegistrationSwagger();

            services.AddBrowserDetection();

            services.AddControllers();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<MutantModelValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigureSecurityApp(ref app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.AddRegistrationSwagger();

            app.UseRouting();

            ConfigureCorsApp(ref app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

        /// <summary>
        /// Configure CORS Service.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <remarks>Elkin Vasquez Isenia</remarks>
        private void ConfigureCorsService(ref IServiceCollection services)
        {
            // Enables CORS and httpoptions
            services.AddCors(options =>
            {
                options.AddPolicy(CommonConfiguration.EnableCorsName, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithHeaders(CommonConfiguration.Authorization, CommonConfiguration.Accept, CommonConfiguration.ContentType, CommonConfiguration.Origin);
                    builder.SetIsOriginAllowed((_) => true);
                });
            });
            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
        }

        /// <summary>
        /// Configure Cors App.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <remarks>Elkin Vasquez Isenia</remarks>
        private void ConfigureCorsApp(ref IApplicationBuilder app)
        {
            app.UseCors(CommonConfiguration.EnableCorsName);
        }

        /// <summary>
        /// Configures the security application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <remarks>Elkin Vasquez Isenia</remarks>
        private void ConfigureSecurityApp(ref IApplicationBuilder app)
        {
            app.UseHsts(options => options.MaxAge(365).IncludeSubdomains().Preload());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseCsp(opts =>
            {
                opts.BlockAllMixedContent();
                opts.StyleSources(s => s.Self());
                opts.StyleSources(s => s.UnsafeInline());
                opts.FontSources(s => s.Self());
                opts.FormActions(s => s.Self());
                opts.FrameAncestors(s => s.Self());
                opts.ImageSources(s => s.Self());
            });
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseStaticFiles();
        }

    }
}
