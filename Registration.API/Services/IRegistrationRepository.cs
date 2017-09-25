using Registration.API.Entities;
using System.Collections.Generic;

namespace Registration.API.Services
{
    public interface IRegistrationRepository
    {
        IEnumerable<Event> GetEvents();
    }
}
