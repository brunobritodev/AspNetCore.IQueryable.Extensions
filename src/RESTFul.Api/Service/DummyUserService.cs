using Microsoft.EntityFrameworkCore;
using Polyjuice.Potions;
using RESTFul.Api.Commands;
using RESTFul.Api.Contexts;
using RESTFul.Api.Models;
using RESTFul.Api.Notification;
using RESTFul.Api.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Linq.Enumerable;

namespace RESTFul.Api.Service
{
    public class DummyUserService : IDummyUserService
    {
        private readonly IDomainNotificationMediatorService _domainNotification;
        private readonly RestfulContext _context;
        private static Random _rnd = new Random();

        public DummyUserService(IDomainNotificationMediatorService domainNotification,
            RestfulContext context)
        {
            _domainNotification = domainNotification;
            _context = context;
        }
        private async Task CheckUsers()
        {
            if (_context.Users.Any())
                return;
            var users = Range(1, 200).Select(index => new User
            {
                FirstName = Name.FirstName,
                LastName = Name.LastName,
                Username = Internet.Email(Name.FirstName),
                Gender = Gender.Random,
                Birthday = DateAndTime.Birthday,
                Active = true,
                Claims = GenerateClaims(index + 1)
            }).ToList();

            foreach (var user in users)
            {
                await _context.Users.AddAsync(user);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private IEnumerable<Claim> GenerateClaims(int userId)
        {
            return Range(1, _rnd.Next(1, 7)).Select(i => new Claim(Job.Title, Lorem.Paragraph(), userId)).ToList();
        }

        public IQueryable<User> Query()
        {
            CheckUsers().Wait();
            return _context.Users.AsQueryable();
        }

        public async Task<IEnumerable<User>> All()
        {
            await CheckUsers().ConfigureAwait(false);
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }

        public async Task Save(RegisterUserCommand command)
        {
            var user = command.ToEntity();
            if (CheckIfUserIsValid(user))
                return;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task Update(User user)
        {
            if (user != null && CheckIfUserIsValid(user))
                return;

            var actua = await Find(user.Username).ConfigureAwait(false);
            _context.Users.Remove(actua);
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> Remove(string username)
        {
            var actual = await Find(username).ConfigureAwait(false);
            if (actual != null)
                _context.Users.Remove(actual);
            return await _context.SaveChangesAsync().ConfigureAwait(false);
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

        public Task<User> Find(string username)
        {
            return _context.Users.FirstOrDefaultAsync(f => f.Username == username);

        }
    }
}
