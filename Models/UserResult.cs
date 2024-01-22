namespace MyRedditAPI.Models
{
    public class UserResult
    {
        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
