using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using System.Collections.Generic;

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
                var sortOrder = 1;
                foreach (var meritBadge in attendeeForCreationDto.MeritBadges)
                {
                    attendeeMeritBadges.Add(new AttendeeMeritBadge
                    {
                        Attendee = attendee,
                        MeritBadgeId = meritBadge.Id,
                        SortOrder = sortOrder++
                    });
                }
            }

            if (attendeeForCreationDto.Accommodations != null)
            {
                foreach (var accommodation in attendeeForCreationDto.Accommodations)
                {
                    attendeeAccommodations.Add(new AttendeeAccommodation
                    {
                        Attendee = attendee,
                        AccommodationId = accommodation.Id
                    });
                }
            }

            // TODO: Get ShirtSize from repo because this will create a duplicate
            var shirtSize = new ShirtSize { Size = attendeeForCreationDto.ShirtSize };

            attendee.SubgroupId = attendeeForCreationDto.SubgroupId;
            attendee.FirstName = attendeeForCreationDto.FirstName;
            attendee.LastName = attendeeForCreationDto.LastName;
            attendee.IsAdult = attendeeForCreationDto.IsAdult;
            attendee.DateOfBirth = attendeeForCreationDto.DateOfBirth;
            attendee.Triathlon = attendeeForCreationDto.Triathlon;
            attendee.IsWithMinor = attendeeForCreationDto.IsWithMinor;
            attendee.ShirtSize = shirtSize;
            attendee.Attendance = Mapper.Map<Attendance>(attendeeForCreationDto.Attendance);

            attendee.AttendeeMeritBadges = attendeeMeritBadges;
            attendee.AttendeeAccommodations = attendeeAccommodations;

            return attendee;
        }
    }
}
