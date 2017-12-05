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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                 .HasIndex(u => u.SubscriberId)
                 .IsUnique();

            builder.Entity<MeritBadge>()
                 .HasIndex(u => u.Name)
                 .IsUnique();

            builder.Entity<Accommodation>()
                 .HasIndex(u => u.Name)
                 .IsUnique();

            builder.Entity<ShirtSize>()
                 .HasIndex(u => u.Size)
                 .IsUnique();
        }

        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<AttendeeAccommodation> AttendeeAccommodations { get; set; }
        public DbSet<AttendeeMeritBadge> AttendeeMeritBadges { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<MeritBadge> MeritBadges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShirtSize> ShirtSizes { get; set; }
        public DbSet<Subgroup> Subgroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSubgroup> UserSubgroups { get; set; }
    }
}
