using System;
using System.Linq;

namespace Registration.API.Entities
{
    public static class RegistrationContextExtensions
    {
        public static void EnsureSeedDataForContext(this RegistrationContext context)
        {
            if (context.Events.Any())
            {
                return;
            }

            // init seed data
            var seedEvent = new Event
            {
                Name = "Encampment 2018",
                Description = "The encampment of 2018",
                Location = "Camp Cowels",
                StartDate = new DateTime(2018, 8, 6),
                EndDate = new DateTime(2018, 8, 12)
            };

            var group = new Group
            {
                Name = "Spokane East Stake"
            };

            var subgroup = new Subgroup
            {
                Name = "Bowdish Ward",
                Group = group
            };

            context.Events.Add(seedEvent);
            context.Groups.Add(group);
            context.Subgroups.Add(subgroup);

            context.SaveChanges();
        }
    }
}
