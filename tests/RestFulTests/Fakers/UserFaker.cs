using Bogus;
using Bogus.Extensions.UnitedStates;
using RestFulTests.Models;

namespace RestFulTests.Fakers
{
    public static class UserFaker
    {
        private static Faker _faker = new Faker();
        public static Faker<User> GenerateUserViewModel()
        {
            var claims = GenerateClaims().Generate(10);
            return new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Int())
                .RuleFor(u => u.CustomId, f => f.Random.Guid())
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Gender, f => f.Person.Email)
                .RuleFor(u => u.Birthday, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Username, f => f.Person.UserName)
                .RuleFor(u => u.Active, f => f.Random.Bool())
                .RuleFor(u => u.Age, f => f.Random.Int(1, 85))
                .RuleFor(u => u.SocialNumber, f => new Ssn() { Identification = f.Person.Ssn() })
                .RuleFor(u => u.Claims, claims);
        }

        public static Faker<Claim> GenerateClaims()
        {
            return new Faker<Claim>().CustomInstantiator(f => new Claim(f.Commerce.Department(), f.Finance.Account(), f.Random.Int()));
        }
    }
}
