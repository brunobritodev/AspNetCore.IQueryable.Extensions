using RESTFul.Api.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace RESTFul.Api.Commands
{
    public class RegisterUserCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Username { get; set; }
        public DateTime? Birthday { get; set; }

        public User ToEntity()
        {
            return new User()
            {
                Birthday = Birthday,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Username = Username,
                CustomId = Guid.NewGuid()
            };
        }
    }
}
