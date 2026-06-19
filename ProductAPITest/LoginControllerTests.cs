using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        //Assert
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials", unauthorized.Value);
    }

    [Fact]
    public void VerifyLogin_ReturnsOk_WithAdminRole()
    {
        var options = new DbContextOptionsBuilder<Entityclass>()
          .UseInMemoryDatabase("TestDb1")
          .Options;

        //using var fakeContext = new Entityclass(options);
        var fakeAdmin = new Login { username = "admin", password = "1234" };
        _mockLogin.Setup(s => s.VerifyLogin("admin", "1234"))
                        .Returns(fakeAdmin);

        _mockToken.Setup(t => t.GenerateToken("admin", "Admin"))
                 .Returns("fake-admin-token");

        var controller = new LoginController(null, _mockToken.Object, _mockLogin.Object);
        var dto = new logindto { Username = "admin", Password = "1234" };

        var result = controller.VerifyLogin(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenObj = okResult.Value.GetType().GetProperty("Token")?.GetValue(okResult.Value);
        Assert.Equal("fake-admin-token", tokenObj);
    }

    [Fact]
    public void VerifyLogin_ReturnsOk_WithUserRole()
    {
        // Arrange: add a non-admin user to in-memory DbContext
        var options = new DbContextOptionsBuilder<Entityclass>()
            .UseInMemoryDatabase("TestDb2")
            .Options;

        var fakeAdmin = new Login { username = "dnyanesh", password = "1234" };
        _mockLogin.Setup(s => s.VerifyLogin("dnyanesh", "1234"))
                        .Returns(fakeAdmin);

        _mockToken.Setup(t => t.GenerateToken("dnyanesh", "User"))
                 .Returns("fake-user-token");

        var controller = new LoginController(null, _mockToken.Object, _mockLogin.Object);
        var dto = new logindto { Username = "dnyanesh", Password = "1234" };

        // Act
        var result = controller.VerifyLogin(dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenObj = okResult.Value.GetType().GetProperty("Token")?.GetValue(okResult.Value);
        Assert.Equal("fake-user-token", tokenObj);
    }


    [Fact]
    public void VerifyLogin_ReturnsServerError_OnException()
    {
        // Arrange
 
        // Fake user returned by login service
        var fakeUser = new Login { username = "john", password = "1234" };
        _mockLogin.Setup(s => s.VerifyLogin("john", "1234"))
                        .Returns(fakeUser);

        // Simulate exception when generating token
        _mockToken.Setup(t => t.GenerateToken("john", "User"))
                 .Throws(new Exception("Token generation failed"));

        var controller = new LoginController(null, _mockToken.Object, _mockLogin.Object);
        var dto = new logindto { Username = "john", Password = "1234" };

        // Act
        var result = controller.VerifyLogin(dto);

        // Assert
        var serverError = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, serverError.StatusCode);
        Assert.Equal("An error occurred while processing your request.", serverError.Value);
    }



}
