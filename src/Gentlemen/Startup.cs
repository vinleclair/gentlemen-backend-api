using System.Configuration;
using System.Reflection;
using FluentValidation.AspNetCore;
using Gentlemen.Infrastructure;
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
            services.AddCors();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<GentlemenContext>(config => { config.UseSqlite(_config.GetConnectionString("GentlemenDatabase")); });
            services.AddMvc(opt => { opt.EnableEndpointRouting = false; })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //TODO bug: if I dont use this response is undefined, needs to be changed to something secure
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                return next.Invoke();
            });
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
            app.UseCors(builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            
            app.ApplicationServices.GetRequiredService<GentlemenContext>().Database.Migrate();

            loggerFactory.AddSerilogLogging();
        }
    }
}