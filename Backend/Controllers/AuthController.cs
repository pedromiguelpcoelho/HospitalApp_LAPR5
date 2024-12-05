using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DDDNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _cognitoAuthService;

    public AuthController(ILogger<AuthController> logger, IAuthService cognitoAuthService)
    {
        _logger = logger;
        _cognitoAuthService = cognitoAuthService;
    }

    [HttpPost("login-username")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var token = await _cognitoAuthService.LoginAsync(model.Email, model.Password);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Falha na autenticação");
            }

            var role = _cognitoAuthService.GetUserRoleFromToken(token);
            if (role == null)
            {
                return BadRequest("User role is missing.");
            }

            var name = _cognitoAuthService.GetUserNameFromToken(token);

            return Ok(new { token, role, name });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return BadRequest("Falha na autenticação: " + ex.Message);
        }
    }


    [HttpPost("login-google")]
    public async Task<IActionResult> LoginWithGoogle([FromBody] GoogleLoginModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.AccessToken))
        {
            return BadRequest("Token de acesso do Google é obrigatório.");
        }

        try
        {
            // Trocar o token de acesso do Google pelo token de ID do Cognito
            var cognitoIdToken = await _cognitoAuthService.ExchangeGoogleAccessTokenForCognitoIdToken(model.AccessToken);

            if (string.IsNullOrEmpty(cognitoIdToken))
            {
                return Unauthorized("Falha na autenticação do Google");
            }

            // Criar as claims (dados da sessão do usuário)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cognitoIdToken),  // Pode ser email ou uid
                new Claim("JWT", cognitoIdToken)  // Armazena o token no claim
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok("Login com Google bem-sucedido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login com Google");
            return BadRequest("Erro durante a autenticação: " + ex.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("Logout realizado com sucesso");
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Name))
        {
            return BadRequest("Email, password, e Nome são obrigatórios.");
        }

        try
        {
            var userId = await _cognitoAuthService.SignUpAsync(model.Email, model.Password, model.Name);
            return Ok(new { Message = "SignUp realizado com sucesso", UserId = userId });
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}");
        }
    }

    [HttpPost("confirm-signup")]
    public async Task<IActionResult> ConfirmSignUp([FromBody] ConfirmSignUpModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.ConfirmationCode))
        {
            return BadRequest("Email e código de confirmação são obrigatórios.");
        }

        try
        {
            await _cognitoAuthService.ConfirmSignUpAsync(model.Email, model.ConfirmationCode);
            return Ok("Conta confirmada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao confirmar conta: " + ex.Message);
            return BadRequest($"Erro ao confirmar conta, verifique o código de confirmação");
        }
    }

}
