namespace RESTJwt.Services
{
    using RESTJwt.Data.Contracts;
    using RESTJwt.Models;
    using RESTJwt.Models.DTOs;
    using RESTJwt.Services.Contracts;

    public class MovieService : IMovieService
    {
        private IMovieRepository _movieRepository;
        private IUserRepository _userRepository;
        public MovieService(IMovieRepository movieRepository, IUserRepository userRepository)
        {
            this._movieRepository = movieRepository;
            this._userRepository = userRepository;
        }
        public Movie Create(MovieDTO movie)
        {
            Movie newMovie = new() {
                Rating = movie.Rating,
                Title = movie.Title,
                Description = movie.Description,
            };
            return this._movieRepository.Create(newMovie);
        }

        public bool Delete(int id)
        {
            var movie = this._movieRepository.FindById(id);

            if (movie is null)
            {
                return false;
            }

            this._movieRepository.Delete(movie);
            
            return true;
        }

        public Movie Get(int id)
        {
            return this._movieRepository.FindById(id);
        }

        public IEnumerable<Movie> List()
        {
            return this._movieRepository.GetAll();
        }

        public Movie Update(Movie newMovie)
        {
            var oldMovie = this._movieRepository.FindById(newMovie.Id);

            if (oldMovie is null)
            {
                return null;
            }

            oldMovie.Title = newMovie.Title;
            oldMovie.Description = newMovie.Description;
            oldMovie.Rating = newMovie.Rating;

            return this._movieRepository.Update(oldMovie);
        }
    }
}
