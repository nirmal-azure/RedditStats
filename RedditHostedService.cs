
using Microsoft.Extensions.Options;
using MyRedditAPI.Models;
using MyRedditAPI.Options;
using MyRedditAPI.Repository;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using System.Text.RegularExpressions;

namespace MyRedditAPI
{
    public class RedditHostedService : BackgroundService
    {        
        private readonly ILogger<RedditHostedService> _logger;
        //store to DB
        private readonly IRedditRepository repo;
        private readonly CredentialsOptions _credOptions;
        private readonly SubRedditOptions _subRedditOptions;
        private static RedditClient reddit = new();

        //Store to dictionary
        private Dictionary<string, UserResult> userResult = new();
        private Dictionary<string, PostResult> postResult = new();
        
        private static Subreddit? subreddit;

        public RedditHostedService(
            ILogger<RedditHostedService> logger, IOptions<CredentialsOptions> credOptions,
            IOptions<SubRedditOptions> subRedditOptions, IRedditRepository repo)
        {
            _logger = logger;
            this.repo = repo;
            this._credOptions = credOptions.Value;
            this._subRedditOptions = subRedditOptions.Value;
            Initialize();
        }

        private void Initialize()
        {
            reddit = new RedditClient(_credOptions.AppId, _credOptions.AppRefToken, _credOptions.AppSecret,
             _credOptions.AppAccessToken);

            subreddit = reddit.Subreddit(_subRedditOptions.SubReddit.FirstOrDefault());

            subreddit.Comments.GetNew();  
            subreddit.Comments.MonitorNew();
            subreddit.Comments.NewUpdated += OnNewCommentsHandler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Yield to display swaggerpage
            await Task.Yield();

            while (!stoppingToken.IsCancellationRequested)
            {
                Task.Delay(1000, stoppingToken).Wait();
            }
            return;
        }

        private void OnNewCommentsHandler(object? sender, CommentsUpdateEventArgs e)
        {
            foreach (Comment comment in e.Added)
            {
                if (!userResult.ContainsKey(comment.Author))
                {
                    var newUser = new UserResult { Author = comment.Author, Count = 1 };
                    userResult.Add(comment.Author, newUser);
                }
                else
                {
                    userResult[comment.Author].Count += userResult[comment.Author].Count;
                }
                repo.AddUser(userResult[comment.Author]);

                
                if (!string.IsNullOrEmpty(FromPermalink(comment.Permalink)))
                { 
                    var post = reddit.Post(FromPermalink(comment.Permalink)).About();

                    if (post != null)
                    {
                        if (!postResult.ContainsKey(post.Author))
                        {
                            var newPost = new PostResult { Title = post.Title, Score = post.Score.ToString() };
                            postResult.Add(post.Author, newPost);                            
                        }
                        if (postResult[post.Author].Score != post.Score.ToString())
                        {
                            postResult[post.Author].Score = post.Score.ToString();
                        }
                        repo.AddPost(postResult[post.Author]);
                    }
                }
            }
        }

        private static string FromPermalink(string permalink)
        {
            // Get the ID from the permalink, then preface it with "t3_" to convert it to a Reddit fullname.  --Kris
            Match match = Regex.Match(permalink, @"\/comments\/([a-z0-9]+)\/");

            string postFullname = "t3_" + (match != null && match.Groups != null && match.Groups.Count >= 2
                ? match.Groups[1].Value
                : "");
            if (postFullname.Equals("t3_"))
            {
                //throw new Exception("Unable to extract ID from permalink.");
                return string.Empty;
            }
            // Retrieve the post and return the result.  --Kris
            return postFullname;            
        }
    }
}
