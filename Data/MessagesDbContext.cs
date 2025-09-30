using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ServiceBusListener2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusListener2.Data
{
    public class MessagesDbContext : DbContext
    {
        public MessagesDbContext(DbContextOptions<MessagesDbContext> options) : base(options) { }

        public DbSet<ServiceBusMessageEntity> Messages { get; set; }
    }


    public class MessagesDbContextFactory : IDesignTimeDbContextFactory<MessagesDbContext>
    {
        public MessagesDbContext CreateDbContext(string[] args)
        {
            // Load config from appsettings.json or fallback to hardcoded string
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = config.GetConnectionString("SqlConnection")
                ?? "Server=tcp:sqlserver1989.database.windows.net,1433;Initial Catalog=jaydb;Persist Security Info=False;User ID=jay1989;Password=Admin@123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            var optionsBuilder = new DbContextOptionsBuilder<MessagesDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MessagesDbContext(optionsBuilder.Options);
        }
    }
}
