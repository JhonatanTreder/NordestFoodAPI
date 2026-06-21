namespace NordesteFoodAPI.Modules.Auth.Domain.Contracts
{
    public interface IPasswordHasher
    {
        string Hash(string passowrd);
        bool Verify(string password, string passwordHash);
    }
}
