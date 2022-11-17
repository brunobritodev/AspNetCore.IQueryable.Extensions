using AspNetCore.IQueryable.Extensions.Filter;
using FluentAssertions;
using RestFulTests.Fakers;
using RestFulTests.Models;
using System;
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
        public void Should_Filter_By_Has_Name_Attribute()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.Last().FirstName
            };

            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public void Should_Filter_By_Guid()
        {
            var userSearch = new UserSearch()
            {
                CustomId = _users.Last().CustomId
            };

            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }


        [Fact]
        public void Should_Apply_Allfilter_Based_In_Class()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.Last().FirstName,
                Username = _users.Last().Username
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public void Should_Apply_Allfilter_For_Two_Fields_For_Same_Attribute()
        {
            var userSearch = new UserSearch()
            {
                OlderThan = 18
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }


        [Fact]
        public void Should_Apply_Or_In_Filter()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.First().FirstName,
                Ssn = _users.First().SocialNumber.Identification,
                Username = _users.Last().Username
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }



        [Fact]
        public void Should_Apply_Exclusive_Or_In_Filter()
        {
            var userSearch = new UserSearch()
            {
                Name = _users.First().FirstName,
                Username = _users.Last().Username
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public void Should_Apply_All_Filter_In_Nested_Object()
        {
            var userSearch = new UserSearch()
            {
                Ssn = _users.Last().SocialNumber.Identification
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCountGreaterOrEqualTo(1);
        }


        [Fact]
        public void Should_Find_User_Id_From_Array()
        {
            var ids = _users.Take(2).Select(s => s.Id);

            var userSearch = new UserSearch()
            {
                Id = ids
            };
            var sortingByFieldName = _users.AsQueryable().Filter(userSearch);
            sortingByFieldName.Should().HaveCount(2);
        }

        [Fact]
        public void Should_Throw_Exception_When_Is_Array_And_Operator_Isnt_Contains()
        {
            var ids = _users.Take(2).Select(s => s.Id);

            var userSearch = new WrongOperator()
            {
                Id = ids
            };
            Action act = () => _users.AsQueryable().Filter(userSearch);


            act.Should().Throw<ArgumentException>();


        }
    }

}
