using System;
using System.Reflection;
using FluentValidation.AspNetCore;
using Gentlemen.Infrastructure;
using Gentlemen.Infrastructure.Errors;
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
        public const string DEFAULT_DATABASE_CONNECTIONSTRING = "Filename=gentlemen.db";
        public const string DEFAULT_DATABASE_PROVIDER = "sqlite";

        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DBContextTransactionPipelineBehavior<,>));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.AddCors();
            
            var connectionString = _config.GetValue<string>("ASPNETCORE_Gentlemen_ConnectionString") ??
                                   DEFAULT_DATABASE_CONNECTIONSTRING;
            var databaseProvider = _config.GetValue<string>("ASPNETCORE_Gentlemen_DatabaseProvider");
            if (string.IsNullOrWhiteSpace(databaseProvider))
                databaseProvider = DEFAULT_DATABASE_PROVIDER;

            services.AddDbContext<GentlemenContext>(options =>
            {
                if (databaseProvider.ToLower().Trim().Equals("sqlite"))
                    options.UseSqlite(connectionString);
                else if (databaseProvider.ToLower().Trim().Equals("sqlserver"))
                {
                    options.UseSqlServer(connectionString);
                }
                else
                    throw new Exception("Database provider unknown. Please check configuration");
            });

            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                    opt.EnableEndpointRouting = false;
                })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.IgnoreNullValues = true; })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });
        }

        public void Configure(IApplicationBuilder app,  ILoggerFactory loggerFactory)
        {
            //TODO Works for now but needs to be changed to something secure
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                return next.Invoke();
            });
            
            loggerFactory.AddSerilogLogging();
            
            app.UseMvc();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            
            app.ApplicationServices.GetRequiredService<GentlemenContext>().Database.EnsureCreated();
        }
    }
}