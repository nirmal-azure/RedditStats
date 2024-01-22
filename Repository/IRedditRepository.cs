using MyRedditAPI.Models;

namespace MyRedditAPI.Repository
{
    public interface IRedditRepository
    {
        Task<bool> AddPost(PostResult post);
        Task<List<PostResult>> GetPostsResult();

        Task<bool> AddUser(UserResult user);
        Task<List<UserResult>> GetUsersResult();
    }
}
