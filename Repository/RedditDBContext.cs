using Microsoft.EntityFrameworkCore;
using MyRedditAPI.Models;

namespace MyRedditAPI.Repository
{
    public class RedditDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "RedditDB");
        }

        public DbSet<PostResult> PostResults { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
    }
}
