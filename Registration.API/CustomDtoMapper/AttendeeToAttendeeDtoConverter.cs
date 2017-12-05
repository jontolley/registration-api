﻿using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using System.Collections.Generic;

namespace Registration.API.CustomDtoMapper
{
    public class AttendeeToAttendeeDtoConverter : ITypeConverter<Attendee, AttendeeDto>
    {
        public AttendeeDto Convert(Attendee attendee, AttendeeDto attendeeDto, ResolutionContext context)
        {
            attendeeDto = new AttendeeDto();

            var meritBadges = new List<MeritBadgeDto>();
            var accommodations = new List<AccommodationDto>();

            foreach (var attendeeMeritBadge in attendee.AttendeeMeritBadges)
            {
                meritBadges.Add(new MeritBadgeDto
                {
                    Id = attendeeMeritBadge.MeritBadge.Id,
                    Name = attendeeMeritBadge.MeritBadge.Name
                });
            }

            foreach (var attendeeAccommodation in attendee.AttendeeAccommodations)
            {
                accommodations.Add(new AccommodationDto
                {
                    Id = attendeeAccommodation.Accommodation.Id,
                    Name = attendeeAccommodation.Accommodation.Name
                });
            }

            attendeeDto.Id = attendee.Id;
            attendeeDto.FirstName = attendee.FirstName;
            attendeeDto.LastName = attendee.LastName;
            attendeeDto.IsAdult = attendee.IsAdult;
            attendeeDto.DateOfBirth = attendee.DateOfBirth;
            attendeeDto.Triathlon = attendee.Triathlon;
            attendeeDto.ShirtSize = attendee.ShirtSize?.Size;
            attendeeDto.MeritBadges = meritBadges;
            attendeeDto.Accommodations = accommodations;

            return attendeeDto;
        }
    }
}
