using AutoMapper;
using System.Reflection;
using FluentValidation.AspNetCore;
using Gentlemen.Infrastructure;
using Gentlemen.Infrastructure.Errors;
using Gentlemen.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentlemen
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddDbContext<GentlemenContext>(config => { config.UseSqlite(_config.GetConnectionString("GentlemenDatabase")); });
            
            services.AddCors();
            services.AddMvc(opt => { opt.EnableEndpointRouting = false; })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
            
            services.AddAutoMapper(GetType().Assembly);
            
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilogLogging();
            
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseCors(builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            
            app.UseAuthentication();
            
            app.UseMvc();

            app.ApplicationServices.GetRequiredService<GentlemenContext>().Database.Migrate();
        }
    }
}