using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Data;
using ProductWebAPI.Model;

namespace ProductWebAPI.Services.LoginService
{
    public class LoginServices:ILoginServices
    {
        private readonly Entityclass _context;
        public LoginServices(Entityclass context) => _context = context;

        public Login VerifyLogin(string username, string password)
        {
            return _context.login
                .FromSqlRaw("EXEC VerifyLogin @Username={0}, @Password={1}", username, password)
                .AsEnumerable()
                .FirstOrDefault();
        }
    }
}
