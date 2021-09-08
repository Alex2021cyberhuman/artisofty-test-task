using System.Reflection;
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

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDataAccessServices(options => _configuration.Bind("DataAccess", options));

            services.AddAutoMapper(typeof(AccountsProfile).Assembly, Assembly.GetExecutingAssembly());

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
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("v1/swagger.json", "TestBack v1"); });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}