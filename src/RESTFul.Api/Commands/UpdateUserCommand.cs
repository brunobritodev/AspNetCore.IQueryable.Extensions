using System;
using System.ComponentModel.DataAnnotations;
using RESTFul.Api.Models;

namespace RESTFul.Api.Commands
{
    public class UpdateUserCommand
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public void Update(User actual)
        {
            actual.FirstName = FirstName;
            actual.LastName = LastName;
            actual.Gender = Gender;
            actual.Birthday = Birthday;

        }
    }
}