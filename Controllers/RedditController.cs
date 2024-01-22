using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyRedditAPI.Models;
using MyRedditAPI.Options;
using MyRedditAPI.Repository;
using Reddit;

namespace MyRedditAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]-[action]")]
    /// <summary>
    /// Sub Reddit Active Statistics
    /// </summary>
    public class RedditController : ControllerBase
    {
        private readonly ILogger<RedditController> _logger;
        private readonly CredentialsOptions credOptions;
        private readonly SubRedditOptions subRedditOptions;
        private readonly IRedditRepository repo;
        private RedditClient reddit = new();

        private Dictionary<string, UserResult> userResult = new();
        private Dictionary<string, PostResult> postResult = new();

        public RedditController(ILogger<RedditController> logger,
            IOptions<CredentialsOptions> credOptions,
            IOptions<SubRedditOptions> subRedditOptions,
            IRedditRepository repo)
        {
            _logger = logger;
            this.repo = repo;
            this.credOptions = credOptions.Value;
            this.subRedditOptions = subRedditOptions.Value;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PostResult))]
        [ProducesResponseType(500)]
        /// <summary>
        /// Top active posts order by score desc ( Top 20)
        /// </summary>
        public async Task<ActionResult<List<PostResult>>> TopPosts()
        {
            try
            {
                var result = await repo.GetPostsResult();

                return Ok(result.ToList());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UserResult))]
        [ProducesResponseType(500)]
        /// <summary>
        /// Top active users order by comments count desc ( Top 20)
        /// </summary>
        public async Task<ActionResult<List<UserResult>>> TopUsers()
        {
            try
            {
                var result = await repo.GetUsersResult();

                return Ok(result.ToList());
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }
    }
}
