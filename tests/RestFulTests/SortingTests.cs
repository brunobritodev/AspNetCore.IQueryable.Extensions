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
        public void Should_Sort_By_Username()
        {
            var sortingByFieldName = _users.AsQueryable().Sort("username").Select(s => s.Username).ToList();
            var sortingByOriginal = _users.OrderBy(s => s.Username).Select(s => s.Username).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Should().Be(sortingByOriginal[i]);
            }
        }

        [Fact]
        public void Should_Sort_By_Username_Descending()
        {
            var sortingByFieldName = _users.AsQueryable().Sort("-username").Select(s => s.Username).ToList();
            var sortingByOriginal = _users.OrderByDescending(s => s.Username).Select(s => s.Username).ToList();
            for (int i = 0; i < sortingByFieldName.Count(); i++)
            {
                sortingByFieldName[i].Should().Be(sortingByOriginal[i]);
            }
        }


        [Fact]
        public void Should_Sort_By_Username__Descending_Then_By_Firstname()
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
        public void Should_Sort_From_Interface_Implementation()
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
        public void Should_Not_Throw_Error_When_Sort_Field_Doesnt_Exist()
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
