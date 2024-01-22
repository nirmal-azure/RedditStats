namespace MyRedditAPI.Options
{
    public class CredentialsOptions
    {
        public const string Credentials = "Credentials";
        public string AppId { get; set; } = string.Empty;
        public string AppSecret { get; set; } = string.Empty;
        public string AppRefToken { get; set; } = string.Empty;
        public string AppAccessToken { get; set; } = string.Empty;

    }
}
