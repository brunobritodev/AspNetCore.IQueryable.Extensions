namespace RestFulTests.Models
{
    public class Claim
    {
        public string Type { get; }
        public string Value { get; }
        public int UserId { get; }
        public int Id { get; set; }
        public Claim() { }
        public Claim(string type, string value, int userId)
        {
            Type = type;
            Value = value;
            UserId = userId;
        }

    }
}