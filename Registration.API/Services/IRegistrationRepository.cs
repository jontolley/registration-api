using Registration.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Registration.API.Services
{
    public interface IRegistrationRepository
    {
        bool EventExists(int eventId);
        IEnumerable<Event> GetEvents();
        Event GetEvent(int eventId);
        void AddEvent(Event event_);
        void DeleteEvent(Event event_);

        bool GroupExists(int groupId);
        IEnumerable<Group> GetGroups();
        Group GetGroup(int groupId, bool includeSubgroups);
        void AddGroup(Group group);
        void DeleteGroup(Group group);

        bool SubgroupExists(int groupId, int subgroupId);
        bool CheckSubgroupPin(int subgroupId, int pin);
        IEnumerable<Subgroup> GetSubgroups(int groupId);
        Subgroup GetSubgroup(int groupId, int subgroupId);
        void AddSubgroup(int groupId, Subgroup subgroup);
        void DeleteSubgroup(Subgroup subgroup);
        
        IEnumerable<Attendee> GetAttendees(int subgroupId);
        Attendee GetAttendee(int subgroupId, int attendeeId);
        void AddAttendee(Attendee attendee);

        bool UserExists(int userId);
        bool UserExists(string subscriberId);
        IEnumerable<User> GetUsers();
        User GetUser(int userId, bool includeRoles = false, bool includeSubgroups = false);
        User GetUser(string subscriberId, bool includeRoles = false, bool includeSubgroups = false);
        void AddUser(User user);
        void DeleteUser(User user);

        Role GetRole(string role);
        void AddRole(User user, Role role);
        void RemoveRole(User user, Role role);
        
        void AddAssignment(UserSubgroup userSubgroup);
        void RemoveAllAssignments(User user);

        IQueryable<ShirtSize> GetShirtSizes();
        IQueryable<MeritBadge> GetMeritBadges();
        IQueryable<Accommodation> GetAccommodations();

        bool Save();
    }
}
