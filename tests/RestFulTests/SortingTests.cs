using AspNetCore.IQueryable.Extensions.Sort;
using FluentAssertions;
using RestFulTests.Fakers;
using RestFulTests.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestFulTests
{
    public class SortingTests
    {
        private readonly List<User> _users;

        public SortingTests()
        {
            _users = UserFaker.GenerateUserViewModel().Generate(50);
        }

        [Fact]
        public void ShouldSortByUsername()
        {
            var sortingByFieldName = _users.AsQueryable().Sort("username").Select(s => s.Username).ToList();
            var sortingByOriginal = _users.OrderBy(s => s.Username).Select(s => s.Username).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Should().Be(sortingByOriginal[i]);
            }
        }

        [Fact]
        public void ShouldSortByUsernameDescending()
        {
            var sortingByFieldName = _users.AsQueryable().Sort("-username").Select(s => s.Username).ToList();
            var sortingByOriginal = _users.OrderByDescending(s => s.Username).Select(s => s.Username).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Should().Be(sortingByOriginal[i]);
            }
        }


        [Fact]
        public void ShouldSortByUsernameDescendingThenByFirstname()
        {
            var sortingByFieldName = _users.AsQueryable().Sort("-username, firstname").ToList();
            var sortingByOriginal = _users.OrderByDescending(s => s.Username).ThenBy(s => s.FirstName).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Username.Should().Be(sortingByOriginal[i].Username);
                sortingByFieldName[i].FirstName.Should().Be(sortingByOriginal[i].FirstName);
            }
        }


        [Fact]
        public void ShouldSortFromInterfaceImplementation()
        {
            var sort = new UserSearch()
            {
                Sort = "username"
            };
            var sortingByFieldName = _users.AsQueryable().Sort(sort).ToList();
            var sortingByOriginal = _users.OrderBy(s => s.Username).ThenBy(s => s.FirstName).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Username.Should().Be(sortingByOriginal[i].Username);
                sortingByFieldName[i].FirstName.Should().Be(sortingByOriginal[i].FirstName);
            }
        }



        [Fact]
        public void ShouldNotThrowErrorWhenSortFieldDoesntExist()
        {
            var sort = new UserSearch()
            {
                Sort = "usernameaa"
            };
            var sortingByFieldName = _users.AsQueryable().Sort(sort).ToList();
            var sortingByOriginal = _users.ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Username.Should().Be(sortingByOriginal[i].Username);
                sortingByFieldName[i].FirstName.Should().Be(sortingByOriginal[i].FirstName);
            }
        }

    }
}
