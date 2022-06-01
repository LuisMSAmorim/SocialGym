using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SocialGym.DAL.Context;

public class SocialGymDbContextFactory
{
    public SocialGymDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory()
            + "/../SocialGym.API/appsettings.json").Build();

        var builder = new DbContextOptionsBuilder<SocialGymDbContext>();
        var connectionString = configuration.GetConnectionString("DatabaseConnection");

        builder.UseSqlServer(connectionString);

        return new SocialGymDbContext(builder.Options);
    }
}
