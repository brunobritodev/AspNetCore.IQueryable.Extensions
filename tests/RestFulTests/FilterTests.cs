using AspNetCore.RESTFul.Extensions.Filter;
using FluentAssertions;
using RestFulTests.Fakers;
using RestFulTests.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestFulTests
{
    public class FilterTests
    {
        private readonly List<User> _users;
        public FilterTests()
        {
            _users = UserFaker.GenerateUserViewModel().Generate(50);
        }

        [Fact]
        public void ShouldFilterByHasNameAttribute()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.Last().FirstName
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCount(1);
        }


        [Fact]
        public void ShouldApplyAllfilterBasedInClass()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.Last().FirstName,
                Username = _users.Last().Username
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCount(1);
        }



        [Fact]
        public void ShouldApplyAllFilterInNestedObject()
        {
            var userSearch = new UserSearch()
            {
                Ssn = _users.Last().SocialNumber.Identification
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCount(1);
        }
    }
}
