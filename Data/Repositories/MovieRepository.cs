namespace RESTJwt.Data.Repositories
{
    using RESTJwt.Data.Contracts;
    using RESTJwt.GenericRepositories;
    using RESTJwt.Models;
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }

        public Movie FindById(int id)
        {
            return this.FirstOrDefault(x => x.Id == id);
        }

        public Movie FindByTitle(string title)
        {
            return this.FirstOrDefault(x => x.Title == title);
        }
    }
}
