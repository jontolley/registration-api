using Registration.API.Entities;
using System.Collections.Generic;

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
        IEnumerable<Subgroup> GetSubgroups(int groupId);
        Subgroup GetSubgroup(int groupId, int subgroupId);
        void AddSubgroup(int groupId, Subgroup subgroup);
        void DeleteSubgroup(Subgroup subgroup);

        bool UserExists(int userId);
        bool UserExists(string subscriberId);
        IEnumerable<User> GetUsers();
        User GetUser(int userId, bool includeRoles);
        User GetUser(string subscriberId, bool includeRoles);
        void AddUser(User user);
        void DeleteUser(User user);

        bool Save();
    }
}
