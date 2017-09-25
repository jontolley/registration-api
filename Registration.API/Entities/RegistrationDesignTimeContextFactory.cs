using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Registration.API.Entities
{
    public class RegistrationDesignTimeContextFactory : IDesignTimeDbContextFactory<RegistrationContext>
    {
        public RegistrationContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RegistrationContext>();
            var connectionString = Startup.Configuration["connectionStrings:registrationDBConnectionString"];
            builder.UseSqlServer(connectionString);
            return new RegistrationContext(builder.Options);
        }
    }
}
