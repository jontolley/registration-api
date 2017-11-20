using Microsoft.EntityFrameworkCore;
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

        #region Event methods
        public bool EventExists(int eventId)
        {
            return _context.Events.Any(e => e.Id == eventId);
        }

        public IEnumerable<Event> GetEvents()
        {
            return _context.Events.OrderBy(e => e.Name).ToList();
        }

        public Event GetEvent(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.Id == eventId);
        }

        public void AddEvent(Event event_)
        {
            _context.Events.Add(event_);
        }

        public void DeleteEvent(Event event_)
        {
            _context.Events.Remove(event_);
        }
        #endregion Event methods

        #region Group methods
        public bool GroupExists(int groupId)
        {
            return _context.Groups.Any(g => g.Id == groupId);
        }

        public IEnumerable<Group> GetGroups()
        {
            return _context.Groups.OrderBy(g => g.Name).ToList();
        }

        public Group GetGroup(int groupId, bool includeSubgroups)
        {
            if (includeSubgroups)
            {
                return _context.Groups.Include(g => g.Subgroups)
                    .Where(g => g.Id == groupId).FirstOrDefault();
            }

            return _context.Groups.Where(g => g.Id == groupId).FirstOrDefault();
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void DeleteGroup(Group group)
        {
            _context.Groups.Remove(group);
        }
        #endregion Group methods

        #region Subgroup methods
        public bool SubgroupExists(int groupId, int subgroupId)
        {
            return _context.Subgroups.Any(s => s.Id == subgroupId && s.GroupId == groupId);
        }

        public IEnumerable<Subgroup> GetSubgroups(int groupId)
        {
            return _context.Subgroups.Where(s => s.GroupId == groupId).OrderBy(s => s.Name).ToList();
        }

        public Subgroup GetSubgroup(int groupId, int subgroupId)
        {
            return _context.Subgroups.FirstOrDefault(g => g.Id == subgroupId && g.GroupId == groupId);
        }

        public void AddSubgroup(int groupId, Subgroup subgroup)
        {
            var group = GetGroup(groupId, false);
            group.Subgroups.Add(subgroup);
        }

        public void DeleteSubgroup(Subgroup subgroup)
        {
            _context.Subgroups.Remove(subgroup);
        }
        #endregion Subgroup methods

        #region User methods
        public bool UserExists(int userId)
        {
            return _context.Users.Any(e => e.Id == userId);
        }

        public bool UserExists(string subscriberId)
        {
            return _context.Users.Any(e => e.SubscriberId == subscriberId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.OrderBy(e => e.Name).ToList();
        }

        public User GetUser(int userId, bool includeRoles)
        {
            if (includeRoles)
            {
                //return _context.Users.Include(g => g.)
                //    .Where(g => g.Id == userId).FirstOrDefault();
                throw new System.NotImplementedException();
            }

            return _context.Users.Where(g => g.Id == userId).FirstOrDefault();
        }

        public User GetUser(string subscriberId, bool includeRoles)
        {
            if (includeRoles)
            {
                return _context.Users.Include(g => g.UserRoles).ThenInclude(ur => ur.Role).Where(g => g.SubscriberId == subscriberId).FirstOrDefault();
                //throw new System.NotImplementedException();
            }

            return _context.Users.Where(g => g.SubscriberId == subscriberId).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }
        #endregion User methods

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
