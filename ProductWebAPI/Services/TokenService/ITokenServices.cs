namespace ProductWebAPI.Services.TokenService
{
    public interface ITokenServices
    {
        string GenerateToken(string username, string role);
    }
}
