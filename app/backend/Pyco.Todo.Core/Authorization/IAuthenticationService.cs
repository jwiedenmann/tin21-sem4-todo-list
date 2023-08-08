using Pyco.Todo.Data.ViewModels;

namespace Pyco.Todo.Core.Authorization
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<AuthenticateResponse> RefreshToken(string token);
        Task RevokeToken(string token);
    }
}