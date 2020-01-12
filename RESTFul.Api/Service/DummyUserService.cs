using Polyjuice.Potions;
using RESTFul.Api.Commands;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RESTFul.Api.Service
{
    public class DummyUserService : IDummyUserService
    {
        private readonly IDomainNotificationMediatorService _domainNotification;
        private static List<User> _users;

        public DummyUserService(IDomainNotificationMediatorService domainNotification)
        {
            _domainNotification = domainNotification;
        }
        private static void CheckUsers()
        {
            if (_users == null)
            {
                _users = Enumerable.Range(1, 10).Select(index => new User
                {
                    Id = index + 1,
                    FirstName = Name.FirstName,
                    LastName = Name.LastName,
                    Username =Internet.Email(Name.FirstName),
                    Gender = Gender.Random,
                    Birthday = DateAndTime.Birthday
                }).ToList();
            }
        }

        public IEnumerable<User> All()
        {
            CheckUsers();
            return _users;
        }

        public void Save(RegisterUserCommand command)
        {
            var user = command.ToEntity(_users.Count + 1);
            if (CheckIfUserIsValid(user))
                return;

            _users.Add(user);
        }

        public void Update(User user)
        {
            if (CheckIfUserIsValid(user))
                return;

            var actua = Find(user.Username);
            _users.Remove(actua);
            _users.Add(user);
        }


        private bool CheckIfUserIsValid(User command)
        {
            var valid = true;
            if (string.IsNullOrEmpty(command.FirstName))
            {
                _domainNotification.Notify(new DomainNotification("User", "Invalid firstname"));
                valid = false;
            }

            if (string.IsNullOrEmpty(command.LastName))
            {
                _domainNotification.Notify(new DomainNotification("User", "Invalid firstname"));
                valid = false;
            }

            if (Find(command.Username) != null)
            {
                _domainNotification.Notify(new DomainNotification("User", "Username already exists"));
                valid = false;
            }

            return valid;
        }

        public User Find(string username)
        {
            var user = _users.First(f => f.Username == username);

            return user;
        }
    }
}
