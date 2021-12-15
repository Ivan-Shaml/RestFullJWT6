namespace RESTJwt.Data
{
    using Microsoft.EntityFrameworkCore;
    using RESTJwt.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }

    }
}
