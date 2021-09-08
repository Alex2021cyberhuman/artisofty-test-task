using System.Reflection;
using Api.Authorization.Configuration;
using FluentValidation;
using Logic.Accounts.Mapping;
using Logic.Accounts.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            var mvcBuilder = services.AddControllersWithViews()
                .AddViewLocalization();

            if (_environment.IsDevelopment())
                mvcBuilder.AddRazorRuntimeCompilation();

            services.AddDataAccessServices(options =>
                _configuration.Bind("DataAccess", options));

            services.AddAutoMapper(typeof(AccountsProfile).Assembly,
                Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssemblies(new[]
            {
                typeof(RegisterRequestValidator).Assembly
            });

            services.AddCustomAuthorization(_configuration);

            services.AddCustomSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", "TestBack v1");
            });

            app.UseRouting();

            app.UseCors(options =>
                options.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "MyArea",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}