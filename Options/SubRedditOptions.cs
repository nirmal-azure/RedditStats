namespace MyRedditAPI.Options
{    public class SubRedditOptions
    {
        public const string Credentials = "SubReddit";
        public List<string> SubReddit { get; set; } = new();
    }
}
