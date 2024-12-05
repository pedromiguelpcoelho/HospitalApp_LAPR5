using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using DDDSample1.Domain.Patients;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

public class CognitoAuthService: IAuthService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoProvider;
    private readonly string _clientId;
    private readonly string _userPoolId;

    private readonly IPatientProfileRepository _patientProfileRepository;

    public CognitoAuthService(IAmazonCognitoIdentityProvider cognitoProvider, string clientId, string userPoolId, IPatientProfileRepository patientProfileRepository)
    {
        _cognitoProvider = cognitoProvider;
        _clientId = clientId;
        _userPoolId = userPoolId;
        _patientProfileRepository = patientProfileRepository;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var request = new AdminInitiateAuthRequest
        {
            AuthFlow = AuthFlowType.ADMIN_USER_PASSWORD_AUTH,
            UserPoolId = _userPoolId,
            ClientId = _clientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password }
            }
        };

        var response = await _cognitoProvider.AdminInitiateAuthAsync(request);

        // Retorna o ID Token para o cookie
        return response.AuthenticationResult?.IdToken; 
    }

    public string GetUserRoleFromToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(idToken);

        // Cognito armazena os grupos no campo "cognito:groups"
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "cognito:groups")?.Value;

        return role;
    }

    public List<string> GetUserGroupsFromToken(string token)
    {
        var groups = new List<string>();
        
        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.ReadJwtToken(token);
        
        // Check if the token contains the "cognito:groups" claim
        if (jwtToken.Claims != null)
        {
            // Extract the groups from the claims
            var groupClaims = jwtToken.Claims.Where(c => c.Type == "cognito:groups");
            groups = groupClaims.Select(c => c.Value).ToList();
        }
        
        return groups;
    }

    public async Task<string> ExchangeGoogleAccessTokenForCognitoIdToken(string accessToken)
    {
        // Lógica para trocar o token de acesso do Google pelo token de ID do Cognito
        var request = new InitiateAuthRequest
        {
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            ClientId = _clientId,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", accessToken },
                { "PASSWORD", "google" } // Ou outra lógica de tratamento de token
            }
        };

        var response = await _cognitoProvider.InitiateAuthAsync(request);
        return response.AuthenticationResult?.IdToken;
    }

    public async Task<string> SignUpAsync(string email, string password, string name)
    {
        var signUpRequest = new SignUpRequest
        {
            ClientId = _clientId,
            Username = email,
            Password = password,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email },
                new AttributeType { Name = "name", Value = name }
            }
        };

        try
        {
            bool patientExistsOnSystem = await _patientProfileRepository.ExistsAsync(email);
            if (!patientExistsOnSystem)
            {
                throw new Exception("Patients must make pre-registration on hospital before signing up.");
            }

            // Sign up the user
            var response = await _cognitoProvider.SignUpAsync(signUpRequest);
            
            // Automatically add the user to the "patients" group
            await AddUserToGroupAsync(email, "patients");

            return response.UserSub; // Return the user ID or any other information
        }
        catch (Exception ex)
        {
            throw new Exception($"Error signing up: {ex.Message}");
        }
    }

    public async Task ConfirmSignUpAsync(string email, string confirmationCode)
    {
        var confirmRequest = new ConfirmSignUpRequest
        {
            ClientId = _clientId,
            Username = email,
            ConfirmationCode = confirmationCode
        };

        try
        {
            await _cognitoProvider.ConfirmSignUpAsync(confirmRequest);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error confirming sign up: {ex.Message}");
        }
    }

    private async Task AddUserToGroupAsync(string username, string groupName)
    {
        var addUserToGroupRequest = new AdminAddUserToGroupRequest
        {
            UserPoolId = _userPoolId,
            Username = username,
            GroupName = groupName
        };

        try
        {
            await _cognitoProvider.AdminAddUserToGroupAsync(addUserToGroupRequest);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding user to group: {ex.Message}");
        }
    }

    public string GetUserNameFromToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;
        var name = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
        return name;
    }
}
