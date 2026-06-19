using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ProductWebAPI.Controllers; // adjust namespace
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Model;
using ProductWebAPI.Services.LoginService;
using ProductWebAPI.Services.TokenService;


// adjust namespace for logindto, login entity
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

public class LoginControllerTests
{


    private readonly IConfiguration _config;
    private readonly Mock<ITokenServices> _mockToken;
    private readonly Mock<ILoginServices> _mockLogin;

    public LoginControllerTests()
    {
        // In-memory configuration for JWT
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "testkey" },
            { "Jwt:Issuer", "testissuer" }
        };
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _mockToken = new Mock<ITokenServices>();
        _mockLogin = new Mock<ILoginServices>();

    }

    [Fact]
    public void VerifyLogin_ReturnsUnauthorized_WhenResultIsNull()
    {
        var options = new DbContextOptionsBuilder<Entityclass>()
            .UseInMemoryDatabase("TestDb1")
            .Options;

        using var fakeContext = new Entityclass(options);

        var controller = new LoginController(fakeContext, _mockToken.Object, _mockLogin.Object);
        var dto = new logindto { Username = "wrong", Password = "wrong" };

        var result = controller.VerifyLogin(dto);

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials", unauthorized.Value);
    }

}
