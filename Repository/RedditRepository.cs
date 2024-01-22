using Microsoft.EntityFrameworkCore;
using MyRedditAPI.Models;

namespace MyRedditAPI.Repository
{
    public class RedditRepository : IRedditRepository
    {
        public async Task<bool> AddPost(PostResult post)
        {
            using (var context = new RedditDBContext())
            {
                var dbPost = context.PostResults.FirstOrDefaultAsync(p => p.Title == post.Title);
                if (dbPost.Result == null)
                    context.PostResults.Add(post);
                else
                {
                    var updatedPost = dbPost.Result;
                    if (updatedPost != null)
                    {
                        updatedPost.Score= post.Score;
                        context.PostResults.Update(updatedPost);
                    }
                }

                await context.SaveChangesAsync();
            }           
            return true;
        }

        public async Task<bool> AddUser(UserResult user)
        {
            using (var context = new RedditDBContext())
            {
                var dbUser = context.UserResults.FirstOrDefaultAsync(u => u.Author == user.Author);
                if (dbUser.Result == null)
                    context.UserResults.Add(user);
                else
                {
                    var updatedUser = dbUser.Result;
                    if (updatedUser != null)
                    {
                        updatedUser.Count++;
                        context.UserResults.Update(updatedUser);
                    }
                }

                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<PostResult>> GetPostsResult()
        {
            using (var context = new RedditDBContext())
            {
                return await context.PostResults.OrderByDescending(p => Int64.Parse(p.Score)).Take(20).ToListAsync();
            }
        }

        public async Task<List<UserResult>> GetUsersResult()
        {
            using (var context = new RedditDBContext())
            {
                return await context.UserResults.OrderByDescending(u => u.Count).Take(20).ToListAsync();
            }
        }
    }
}
