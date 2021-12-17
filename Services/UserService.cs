namespace RESTJwt.Services
{
    using RESTJwt.Data.Contracts;
    using RESTJwt.Models;
    using RESTJwt.Models.DTOs;
    using RESTJwt.Providers.Contracts;
    using RESTJwt.Services.Contracts;
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IPasswordProvider _passwordProvider;
        public UserService(IUserRepository userRepository, IPasswordProvider passwordProvider)
        {
            this._userRepository = userRepository;
            this._passwordProvider = passwordProvider;
        }

        public IEnumerable<string> DoesUserExists(UserRegisterDTO userRegstierDTO)
        {
            bool doesUsernameExist = false;
            bool doesEmailExist = false;

            List<string> resultsList = new List<string>();

            User checkIfUserExists = this._userRepository.FirstOrDefault(x => x.Username == userRegstierDTO.Username);
            
            doesUsernameExist = checkIfUserExists is null ? false : true;

            checkIfUserExists = this._userRepository.FirstOrDefault(x => x.EmailAddress == userRegstierDTO.EmailAddress);

            doesEmailExist = checkIfUserExists is null ? false : true;

            if (doesEmailExist == false && doesUsernameExist == false)
            {
                return null;
            }
            
            if (doesUsernameExist)
            {
                resultsList.Add("Username already in use.");
            }
            
            if (doesEmailExist)
            {
                resultsList.Add("Email already in use.");
            }

            return resultsList;
        }

        public User Create(UserRegisterDTO userRegstierDTO)
        {
            string hashedPassword = this._passwordProvider.HashPassword(userRegstierDTO.Password);

            User newUser = new()
            {
                Username = userRegstierDTO.Username,
                Password = hashedPassword,
                FirstName = userRegstierDTO.FirstName,
                LastName = userRegstierDTO.LastName,
                EmailAddress = userRegstierDTO.EmailAddress,
                Role = "Standard",
            };

            return this._userRepository.Create(newUser);
        }

        public User Get(UserLoginDTO userLoginDTO)
        {
            string hashedPassword = this._passwordProvider.HashPassword(userLoginDTO.Password);

            return this._userRepository.FirstOrDefault(x => x.Username == userLoginDTO.Username && x.Password == hashedPassword);
        }

        public string ChangeRole(string username)
        {
            User user = this._userRepository.FirstOrDefault(x => x.Username == username);

            if (user is not null)
            {
                if (user.Role == "Standard")
                {
                    user.Role = "Administrator";
                }
                else
                {
                    user.Role = "Standard";
                }

                this._userRepository.Update(user);

                return $"Role for user {user.Username} has been changed sucessfully to {user.Role}";
            }

            return null;
        }

        public string ChangeRole(Guid uuid)
        {
            User user = this._userRepository.FirstOrDefault(x => x.Uuid == uuid);

            if (user is not null)
            {
                if (user.Role == "Standard")
                {
                    user.Role = "Administrator";
                }
                else
                {
                    user.Role = "Standard";
                }

                this._userRepository.Update(user);

                return $"Role for user {user.Username} has been changed sucessfully to {user.Role}";
            }

            return null;
        }
    }
}
