using Pyco.Todo.Data.ViewModels;

namespace Pyco.Todo.Core.Authorization
{
    public interface IAuthenticationService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        AuthenticateResponse RefreshToken(string token);
        void RevokeToken(string token);
    }
}