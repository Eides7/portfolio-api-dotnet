using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Portfolio.Infrastructure.Persistence;
using System.Data.Common;

namespace Portfolio.Tests.Integration
{
    public sealed class CustomerWebApplicationFactory : WebApplicationFactory<Program>
    {
        private DbConnection? _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                //Remove the existing DBContext registration
                var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<PortfolioDbContext>));

                if (descriptor is not null)
                    services.Remove(descriptor);

                //Create and open SQLite in-memory connection for test
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                services.AddDbContext<PortfolioDbContext>(options => options.UseSqlite(_connection));

                //Ensure database schema exists
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
                db.Database.EnsureCreated();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _connection?.Dispose();
            }
        }
    }
}
