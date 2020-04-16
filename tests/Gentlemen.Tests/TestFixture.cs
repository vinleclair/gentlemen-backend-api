using System;
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
        private readonly ServiceProvider _provider;

        private readonly IServiceScopeFactory _scopeFactory;

        protected Faker faker = new Faker("en");

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

            var options = new DbContextOptionsBuilder<GentlemenContext>().UseSqlite(connection).Options;

            services.AddSingleton(new GentlemenContext(options));

            startup.ConfigureServices(services);

            _provider = services.BuildServiceProvider();

            DatabaseFixture();

            _scopeFactory = _provider.GetService<IServiceScopeFactory>();
        }

        public void Dispose()
        {
            GetDbContext().Database.EnsureDeleted();
        }

        public GentlemenContext GetDbContext()
        {
            return _provider.GetRequiredService<GentlemenContext>();
        }

        public void DatabaseFixture()
        {
            // TODO Refactor into factory pattern
            GetDbContext().Database.EnsureCreated();
            GetDbContext().Barbers.Add(new Barber {Name = "John Doe", ImagePath = "john_doe.png"});
            GetDbContext().Services.Add(new Service {Name = "Haircut", Duration = 30, Price = 26});
            GetDbContext().SaveChanges();
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