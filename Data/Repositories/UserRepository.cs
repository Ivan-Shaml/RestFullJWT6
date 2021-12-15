namespace RESTJwt.Data.Repositories
{
    using RESTJwt.Data.Contracts;
    using RESTJwt.GenericRepositories;
    using RESTJwt.Models;
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public User GetByUserName(string username)
        {
            return this.FirstOrDefault(x=>x.Username == username);
        }

        public User GetByUUID(Guid Uuid)
        { 
            return this.FirstOrDefault(x => x.Uuid == Uuid);
        }
    }
}
