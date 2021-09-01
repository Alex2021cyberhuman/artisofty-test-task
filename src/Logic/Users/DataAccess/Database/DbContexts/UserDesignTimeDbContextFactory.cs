using Logic.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Logic.Users.DataAccess.Database
{
    public class UserDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            var options = new DatabaseOptions();
            configuration.Bind(options);

            var builder = new DbContextOptionsBuilder<UsersDbContext>()
                .ConfigureProviderType(options)
                .ConfigureDebugging(options);


            var context = new UsersDbContext(builder.Options);
            return context;
        }
    }
}