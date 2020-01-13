namespace RESTFul.Api.Models
{
    public class ClaimViewModel
    {
        public string Type { get; }
        public string Value { get; }
        public int Id { get; set; }
        public ClaimViewModel() { }
        public ClaimViewModel(int id, string type, string value)
        {
            Id = id;
            Type = type;
            Value = value;
        }

    }
}