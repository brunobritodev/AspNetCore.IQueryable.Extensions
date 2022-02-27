using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RestFulTests.Models
{
    [DebuggerDisplay("{FullName}")]
    public class User
    {
        public int Id { get; set; }
        public Guid CustomId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName}, {FirstName}";
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Username { get; set; }
        public bool Active { get; set; }
        public int Age { get; set; }
        public Ssn SocialNumber { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class Ssn
    {
        public string Identification { get; set; }
    }
}