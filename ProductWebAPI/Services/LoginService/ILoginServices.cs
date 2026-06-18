using ProductWebAPI.Model;

namespace ProductWebAPI.Services.LoginService
{
    public interface ILoginServices
    {
        Login VerifyLogin(string username, string password);
    }
}
