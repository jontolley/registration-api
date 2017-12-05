using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using Registration.API.Services;
using System.Collections.Generic;
using System.Linq;

namespace Registration.API.CustomDtoMapper
{
    public class AttendeeForCreationDtoToAttendeeConverter : ITypeConverter<AttendeeForCreationDto, Attendee>
    {
        public Attendee Convert(AttendeeForCreationDto attendeeForCreationDto, Attendee attendee, ResolutionContext context)
        {
            attendee = new Attendee();

            var attendeeMeritBadges = new List<AttendeeMeritBadge>();
            var attendeeAccommodations = new List<AttendeeAccommodation>();

            if (attendeeForCreationDto.MeritBadges != null)
            {
                foreach (var meritBadge in attendeeForCreationDto.MeritBadges)
                {
                    attendeeMeritBadges.Add(new AttendeeMeritBadge
                    {
                        Attendee = attendee,
                        MeritBadgeId = meritBadge.Id,
                        SortOrder = meritBadge.SortOrder
                    });
                }
            }

            if (attendee.AttendeeAccommodations != null)
            {
                foreach (var attendeeAccommodation in attendee.AttendeeAccommodations)
                {
                    attendeeAccommodations.Add(new AttendeeAccommodation
                    {
                        Attendee = attendee,
                        AccommodationId = attendeeAccommodation.Id
                    });
                }
            }

            // TODO: Get ShirtSize from repo because this will create a duplicate
            var shirtSize = new ShirtSize { Size = attendeeForCreationDto.ShirtSize };

            attendee.FirstName = attendeeForCreationDto.FirstName;
            attendee.LastName = attendeeForCreationDto.LastName;
            attendee.IsAdult = attendeeForCreationDto.IsAdult;
            attendee.DateOfBirth = attendeeForCreationDto.DateOfBirth;
            attendee.Triathlon = attendeeForCreationDto.Triathlon;
            attendee.ShirtSize = shirtSize;

            attendee.AttendeeMeritBadges = attendeeMeritBadges;
            attendee.AttendeeAccommodations = attendeeAccommodations;

            return attendee;
        }
    }
}
