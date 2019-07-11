using Microsoft.EntityFrameworkCore;

namespace yc_psql_demo.Models
{
    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options) : base(options)
        {
        }
        public DbSet<PostClass> Posts { get; set; }
    }
}
