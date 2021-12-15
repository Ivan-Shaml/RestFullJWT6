namespace RESTJwt.Data.Contracts
{
    using RESTJwt.GenericContracts;
    using RESTJwt.Models;
    public interface IUserRepository : IGenericRepository<User>
    {
        public User GetByUserName(string username);
        public User GetByUUID(Guid Uuid);
    }
}
