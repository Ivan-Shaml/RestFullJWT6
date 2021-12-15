namespace RESTJwt.Services.Contracts
{
    using RESTJwt.Models;
    using RESTJwt.Models.DTOs;

    public interface IUserService
    {
        public User Get(UserLoginDTO userLogin);

        public User Create(UserRegisterDTO userRegstierDTO);
        public IEnumerable<string> DoesUserExists(UserRegisterDTO userRegstierDTO);

        public string ChangeRole(string username);
        public string ChangeRole(Guid uuid);
    }
}
