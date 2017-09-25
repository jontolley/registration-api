using Registration.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Registration.API.Services
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private RegistrationContext _context;
        public RegistrationRepository(RegistrationContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetEvents()
        {
            return _context.Events.ToList();
        }
    }
}
