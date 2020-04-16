using System;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Gentlemen.Domain;
using Gentlemen.Infrastructure;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gentlemen.Tests
{
    public class TestFixture : IDisposable
    {
        static readonly IConfiguration Config;

        protected Faker faker = new Faker("en");
        
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ServiceProvider _provider;

        static TestFixture()
        {
            Config = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();
        }

        public TestFixture()
        {
            var startup = new Startup(Config);
            var services = new ServiceCollection();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            
            var option = new DbContextOptionsBuilder<GentlemenContext>().UseSqlite(connection).Options;
            
            services.AddSingleton(new GentlemenContext(option));

            startup.ConfigureServices(services);

            _provider = services.BuildServiceProvider();
            DatabaseFixture();
            _scopeFactory = _provider.GetService<IServiceScopeFactory>();
        }

        public GentlemenContext GetDbContext()
        {
            return _provider.GetRequiredService<GentlemenContext>();
        }

        public void DatabaseFixture()
        {
            GetDbContext().Database.EnsureCreated();
            GetDbContext().Barbers.Add(new Barber { Name = "John Doe", ImagePath = "john_doe.png" });
            GetDbContext().Services.Add(new Service { Name = "Haircut", Duration = 30, Price = 26 });
            GetDbContext().SaveChanges();
        }
        public void Dispose()
        {
            GetDbContext().Database.EnsureDeleted();
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                await action(scope.ServiceProvider);
            }
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                return await action(scope.ServiceProvider);
            }
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task ExecuteDbContextAsync(Func<GentlemenContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<GentlemenContext>()));
        }

        public Task<T> ExecuteDbContextAsync<T>(Func<GentlemenContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<GentlemenContext>()));
        }

        public Task InsertAsync(params object[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }
    }
}