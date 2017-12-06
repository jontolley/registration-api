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

        public bool CheckSubgroupPin(int subgroupId, int pin)
        {
            return _context.Subgroups.Any(s => s.Id == subgroupId && s.PinNumber == pin);
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

        public User GetUser(int userId, bool includeRoles = false, bool includeSubgroups = false)
        {
            if (includeRoles && includeSubgroups)
            {
                throw new System.NotImplementedException();
            }
            else if (includeRoles)
            {
                return _context.Users.Include(g => g.UserRoles).ThenInclude(ur => ur.Role).Where(g => g.Id == userId).FirstOrDefault();
            }
            else if (includeSubgroups)
            {
                return _context.Users.Include(g => g.UserSubgroups).ThenInclude(us => us.Subgroup).Where(g => g.Id == userId).FirstOrDefault();
            }

            return _context.Users.Where(g => g.Id == userId).FirstOrDefault();
        }

        public User GetUser(string subscriberId, bool includeRoles = false, bool includeSubgroups = false)
        {
            if (includeRoles && includeSubgroups)
            {
                throw new System.NotImplementedException();
            }
            else if (includeRoles)
            {
                return _context.Users.Include(g => g.UserRoles).ThenInclude(ur => ur.Role).Where(g => g.SubscriberId == subscriberId).FirstOrDefault();
            }
            else if (includeSubgroups)
            {
                return _context.Users.Include(g => g.UserSubgroups).ThenInclude(us => us.Subgroup).Where(g => g.SubscriberId == subscriberId).FirstOrDefault();
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

        #region Role methods
        public Role GetRole(string role)
        {
            return _context.Roles.FirstOrDefault(r => r.Name == role);
        }

        public void AddRole(User user, Role role)
        {
            var roleAlreadyAssigned = _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
            if (roleAlreadyAssigned) return;

            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
        }

        public void RemoveRole(User user, Role role)
        {
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

            if (userRole == null) return;
            
            _context.UserRoles.Remove(userRole);
        }
        #endregion Role methods

        #region Assignment methods
        public void AddAssignment(UserSubgroup userSubgroup)
        {
            _context.UserSubgroups.Add(userSubgroup);
        }

        public void RemoveAllAssignments(User user)
        {
            var allAssignments = _context.UserSubgroups.Where(us => us.UserId == user.Id);
            foreach (var assignment in allAssignments)
            {
                _context.UserSubgroups.Remove(assignment);
            }            
        }
        #endregion Assignment methods

        #region Attendee methods
        public IEnumerable<Attendee> GetAttendees(int subgroupId)
        {
            return _context.Attendees
                .Include(a => a.ShirtSize)
                .Include(a => a.AttendeeMeritBadges).ThenInclude(ab => ab.MeritBadge)
                .Include(a => a.AttendeeAccommodations).ThenInclude(aa => aa.Accommodation)
                .Where(a => a.SubgroupId == subgroupId);
        }

        public Attendee GetAttendee(int subgroupId, int attendeeId)
        {
            return _context.Attendees
                .Include(a => a.ShirtSize)
                .Include(a => a.AttendeeMeritBadges).ThenInclude(ab => ab.MeritBadge)
                .Include(a => a.AttendeeAccommodations).ThenInclude(aa => aa.Accommodation)
                .FirstOrDefault(a => a.SubgroupId == subgroupId && a.Id == attendeeId);
        }

        public void AddAttendee(Attendee attendee)
        {
            attendee.SubgroupId = attendee.SubgroupId == 0 ? attendee.Subgroup.Id : attendee.SubgroupId;
            attendee.Subgroup = null;

            if (attendee.ShirtSize != null)
            {
                var shirtSize = _context.ShirtSizes.FirstOrDefault(s => s.Size == attendee.ShirtSize.Size);
                attendee.ShirtSize = shirtSize;
            }

            _context.Attendees.Add(attendee);
        }

        public void RemoveAllMeritBadges(Attendee attendee)
        {
            var allAttendeeMeritBadges = _context.AttendeeMeritBadges.Where(amb => amb.AttendeeId == attendee.Id);
            foreach (var attendeeMeritBadge in allAttendeeMeritBadges)
            {
                _context.AttendeeMeritBadges.Remove(attendeeMeritBadge);
            }
        }

        public void RemoveAllAccommodations(Attendee attendee)
        {
            var allAttendeeAccommodations = _context.AttendeeAccommodations.Where(aa => aa.AttendeeId == attendee.Id);
            foreach (var attendeeAccommodation in allAttendeeAccommodations)
            {
                _context.AttendeeAccommodations.Remove(attendeeAccommodation);
            }
        }
        #endregion Attendee methods

        #region Support methods
        public IQueryable<ShirtSize> GetShirtSizes()
        {
            return _context.ShirtSizes;
        }

        public IQueryable<MeritBadge> GetMeritBadges()
        {
            return _context.MeritBadges;
        }

        public IQueryable<Accommodation> GetAccommodations()
        {
            return _context.Accommodations;
        }
        #endregion Support methods

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
