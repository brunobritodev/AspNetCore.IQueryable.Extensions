using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Pagination;
using FluentAssertions;
using RestFulTests.Fakers;
using RestFulTests.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestFulTests
{
    public class PagingTests
    {
        private readonly List<User> _users;
        public PagingTests()
        {
            _users = UserFaker.GenerateUserViewModel().Generate(50);
        }

        [Fact]
        public void Should_Paging()
        {
            var sortingByFieldName = _users.AsQueryable().Paginate(5, 0);
            sortingByFieldName.Should().HaveCount(5);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(-166)]
        public void Should_Get_Default_Paging_When_Limit_Is_Zero_Or_Negative(int limit)
        {
            var sortingByFieldName = _users.AsQueryable().Paginate(limit, 0);
            sortingByFieldName.Should().HaveCount(10);
        }

        [Fact]
        public void Should_Paging_From_Interface_Implementation()
        {
            var paginate = new UserSearch()
            {
                Limit = 5,
                Offset = 0
            };
            var sortingByFieldName = _users.AsQueryable().Paginate(paginate);

            sortingByFieldName.Should().HaveCount(5);
        }

        [Fact]
        public void Should_Limit_Not_Bigger_Than_Atrribute_Max()
        {
            var paginate = new PagingMax()
            {
                Limit = 20,
                Offset = 0
            };
            var sortingByFieldName = _users.AsQueryable().Paginate(paginate);
            sortingByFieldName.Should().HaveCount(5);
        }

        [Fact]
        public void Should_Respect_Attribute_Max_When_Limit_Is_Not_Set()
        {
            var paginate = new PagingMax()
            {
                Offset = 0
            };
            var sortingByFieldName = _users.AsQueryable().Apply(paginate);
            sortingByFieldName.Should().HaveCount(5);
        }

        [Fact]
        public void Should_Ignore_Attribute_Max_When_Valid_Limit_Is_Given()
        {
            var paginate = new PagingMax()
            {
                Offset = 0,
                Limit = 2
            };
            var sortingByFieldName = _users.AsQueryable().Apply(paginate);
            sortingByFieldName.Should().HaveCount(2);
        }
        
        [Fact]
        public void Should_Return_Alldata_When_Limit_Is_Not_Specified()
        {
            var paginate = new SinglePaging();
            var pagingData = _users.AsQueryable().Paginate(paginate);
            pagingData.Should().HaveCount(50);
        }


        [Fact]
        public void Should_Return_Partial_Data_When_Offset_Specified()
        {
            var paginate = new SinglePaging() { Offset = 10 };
            var pagingData = _users.AsQueryable().Paginate(paginate);
            pagingData.Should().HaveCount(40);
        }

        [Fact]
        public void Should_Return_Partial_Data_When_Limit_Specified()
        {
            var paginate = new SinglePaging() { Limit = 15 };
            var pagingData = _users.AsQueryable().Paginate(paginate);
            pagingData.Should().HaveCount(15);
        }
    }
}
