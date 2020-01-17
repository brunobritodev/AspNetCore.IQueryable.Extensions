using Bogus;
using RestFulTests.Models;

namespace RestFulTests.Fakers
{
    public static class UserFaker
    {
        private static Faker _faker = new Faker();
        public static Faker<User> GenerateUserViewModel()
        {
            return new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Gender, f => f.Person.Email)
                .RuleFor(u => u.Birthday, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Id, f => f.Random.Int())
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.Active, f => f.Random.Bool());
        }
    }
}
