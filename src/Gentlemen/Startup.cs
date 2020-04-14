using System.Reflection;
using FluentValidation.AspNetCore;
using Gentlemen.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
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

            services.AddDbContext<GentlemenContext>(options =>
                options.UseSqlite(_config.GetValue<string>("ASPNETCORE_Gentlemen_ConnectionString")));

            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            })
            .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
            .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
        }

        public void Configure(IApplicationBuilder app,  ILoggerFactory loggerFactory)
        {
            //TODO bug: if I dont use this response is undefined, needs to be changed to something secure
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                return next.Invoke();
            });
            
            loggerFactory.AddSerilogLogging();
            
            app.UseMvc();

            app.UseCors(builder =>
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:8080"));

            app.ApplicationServices.GetRequiredService<GentlemenContext>().Database.EnsureCreated();
        }
    }
}