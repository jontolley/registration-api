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

            builder.Entity<Subgroup>()
                .HasMany(a => a.Attendees)
                .WithOne(u => u.Subgroup)
                .HasForeignKey(a => a.SubgroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attendee>()
                .HasOne(a => a.InsertedBy)
                .WithMany(u => u.InsertedAttendees)
                .HasForeignKey(a => a.InsertedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attendee>()
                .HasOne(a => a.UpdatedBy)
                .WithMany(u => u.UpdatedAttendees)
                .HasForeignKey(a => a.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attendee>()
                .HasOne(a => a.ShirtSize)
                .WithMany(u => u.ShirtSizeAttendees)
                .HasForeignKey(a => a.ShirtSizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attendee>()
                .HasOne(a => a.Attendance)
                .WithMany(u => u.AttendanceAttendees)
                .HasForeignKey(a => a.AttendanceId)
                .OnDelete(DeleteBehavior.Restrict);
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
        public DbSet<Contact> Contacts { get; set; }
    }
}
