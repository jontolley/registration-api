using AutoMapper;
using Registration.API.Entities;
using Registration.API.Models;
using System.Collections.Generic;

namespace Registration.API.CustomDtoMapper
{
    public class AttendeeForUpdateDtoToAttendeeConverter : ITypeConverter<AttendeeForUpdateDto, Attendee>
    {
        public Attendee Convert(AttendeeForUpdateDto attendeeForUpdateDto, Attendee attendee, ResolutionContext context)
        {            
            var shirtSize = new ShirtSize { Size = attendeeForUpdateDto.ShirtSize };

            attendee.SubgroupId = attendeeForUpdateDto.SubgroupId;
            attendee.FirstName = attendeeForUpdateDto.FirstName;
            attendee.LastName = attendeeForUpdateDto.LastName;
            attendee.IsAdult = attendeeForUpdateDto.IsAdult;
            attendee.DateOfBirth = attendeeForUpdateDto.DateOfBirth;
            attendee.Triathlon = attendeeForUpdateDto.Triathlon;
            attendee.ShirtSize = shirtSize;
            
            return attendee;
        }
    }
}
