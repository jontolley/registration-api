using Microsoft.EntityFrameworkCore;

namespace Registration.API.Entities
{
    public class RegistrationContext : DbContext
    {
        public RegistrationContext(DbContextOptions<RegistrationContext> options)
           : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Event> Events { get; set; }
    }
}
