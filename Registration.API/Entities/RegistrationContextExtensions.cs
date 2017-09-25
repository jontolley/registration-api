using System;
using System.Collections.Generic;
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
            var events = new List<Event>()
            {
                new Event()
                {
                    Name = "Encampment 2018",
                    Description = "The encampment of 2018",
                    Location = "Camp Cowels",
                    StartDate = new DateTime(2018, 8, 6),
                    EndDate = new DateTime(2018, 8, 12)
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
