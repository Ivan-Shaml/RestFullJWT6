namespace RESTJwt.Providers
{
    using RESTJwt.Providers.Contracts;
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordProvider : IPasswordProvider
    {
        public string HashPassword(string plainTextPassword)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);

            byte[] hashedPasswordBytes = SHA256.HashData(passwordBytes);

            string hashedPassword = Convert.ToBase64String(hashedPasswordBytes);

            return hashedPassword;
        }

        public bool ValidatePassword(string plainTextPassword, string hashedPassword)
        {
            string userInputPassword = this.HashPassword(plainTextPassword);

            if (hashedPassword == userInputPassword)
            {
                return true;
            }
            
            return false;
        }
    }
}
