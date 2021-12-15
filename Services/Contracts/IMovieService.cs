namespace RESTJwt.Services.Contracts
{
    using RESTJwt.Models;
    using RESTJwt.Models.DTOs;
    public interface IMovieService
    {
        public Movie Create(MovieDTO movie);
        public Movie Get(int id);
        public IEnumerable<Movie> List();
        public Movie Update(Movie newMovie);
        public bool Delete(int id);
    }
}
