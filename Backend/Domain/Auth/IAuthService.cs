using System.Collections.Generic;
using System.Threading.Tasks;


public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
    string GetUserRoleFromToken(string idToken);
    List<string> GetUserGroupsFromToken(string token);
    Task<string> ExchangeGoogleAccessTokenForCognitoIdToken(string accessToken);
    Task<string> SignUpAsync(string email, string password, string name);
    Task ConfirmSignUpAsync(string email, string confirmationCode);
    
    string GetUserNameFromToken(string token);
}