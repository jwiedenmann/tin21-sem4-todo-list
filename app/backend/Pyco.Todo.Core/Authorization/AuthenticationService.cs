using Microsoft.AspNet.Identity;
using Pyco.Todo.Core.Exception;
using Pyco.Todo.Data.Models;
using Pyco.Todo.Data.ViewModels;
using Pyco.Todo.DataAccess;

namespace Pyco.Todo.Core.Authorization;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtUtils _jwtUtils;

    public AuthenticationService(
        IJwtUtils jwtUtils,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshToken)
    {
        _jwtUtils = jwtUtils;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshToken;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        User? user = _userRepository.Get(model.Username);
        PasswordHasher passwordHasher = new();

        // validate
        if (user == null ||
            passwordHasher.VerifyHashedPassword(user.Password, model.Password) == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Username or password is incorrect");
        }

        // authentication successful so generate jwt and refresh tokens
        string jwtToken = _jwtUtils.GenerateJwtToken(user);
        RefreshToken refreshToken = _jwtUtils.GenerateRefreshToken(user.Id);
        _refreshTokenRepository.Set(refreshToken);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }

    public AuthenticateResponse RefreshToken(string token)
    {
        User? user = _userRepository.GetByToken(token);
        RefreshToken? refreshToken = user != null ? _refreshTokenRepository.Get(user.Id) : null;

        if (user is null || (refreshToken?.IsExpired ?? true))
        {
            throw new UnauthorizedException("Invalid token");
        }

        RefreshToken newRefreshToken = _jwtUtils.GenerateRefreshToken(user.Id);
        string newJwtToken = _jwtUtils.GenerateJwtToken(user);

        // replace token in db
        _refreshTokenRepository.Set(newRefreshToken);

        return new AuthenticateResponse(user, newJwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token)
    {
        User? user = _userRepository.GetByToken(token);
        RefreshToken? refreshToken = user != null ? _refreshTokenRepository.Get(user.Id) : null;

        if (refreshToken is null)
        {
            return;
        }

        _refreshTokenRepository.Delete(token);
    }
}
