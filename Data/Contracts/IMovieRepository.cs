namespace RESTJwt.Data.Contracts
{
    using RESTJwt.GenericContracts;
    using RESTJwt.Models;
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        public Movie FindById (int id);
        public Movie FindByTitle (string title);

    }
}
