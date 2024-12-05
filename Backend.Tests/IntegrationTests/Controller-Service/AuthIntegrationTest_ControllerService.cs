using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using DDDNetCore;
using Microsoft.AspNetCore.Mvc;

public class AuthControllerTest
{
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly Mock<IAuthService> _cognitoAuthServiceMock;
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _loggerMock = new Mock<ILogger<AuthController>>();
        _cognitoAuthServiceMock = new Mock<IAuthService>();
        _authController = new AuthController(_loggerMock.Object, _cognitoAuthServiceMock.Object);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginModel = new LoginModel { Email = "test@example.com", Password = "invalid_password" };
        _cognitoAuthServiceMock.Setup(service => service.LoginAsync(loginModel.Email, loginModel.Password))
                               .ReturnsAsync((string)null);

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenUserNotConfirmed()
    {
        // Arrange
        var loginModel = new LoginModel { Email = "test@example.com", Password = "Test@2024" };
        _cognitoAuthServiceMock.Setup(service => service.LoginAsync(loginModel.Email, loginModel.Password))
                               .ThrowsAsync(new Amazon.CognitoIdentityProvider.Model.UserNotConfirmedException("User not confirmed"));

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenUserRoleIsMissing()
    {
        // Arrange
        var loginModel = new LoginModel { Email = "test@example.com", Password = "Test@2024" };
        _cognitoAuthServiceMock.Setup(service => service.LoginAsync(loginModel.Email, loginModel.Password))
                               .ReturnsAsync("valid_token");
        _cognitoAuthServiceMock.Setup(service => service.GetUserRoleFromToken("valid_token"))
                               .Returns((string)null);

        // Act
        var result = await _authController.Login(loginModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}