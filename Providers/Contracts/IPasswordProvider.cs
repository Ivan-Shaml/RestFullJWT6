namespace RESTJwt.Providers.Contracts
{
    public interface IPasswordProvider
    {
        public string HashPassword(string plainTextPassword);

        public bool ValidatePassword(string plainTextPassword, string hashedPassword);
    }
}
