using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace DDDSample1.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DDDSample1DbContext>
    {
        public DDDSample1DbContext CreateDbContext(string[] args)
        {
            Env.Load("Confidential/.env");

            var connectionString = Env.GetString("DATABASE_CONNECTION_STRING");

            var optionsBuilder = new DbContextOptionsBuilder<DDDSample1DbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DDDSample1DbContext(optionsBuilder.Options);
        }
    }
}
