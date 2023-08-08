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

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        User? user = await _userRepository.GetAsync(model.Username);
        PasswordHasher passwordHasher = new();

        // validate
        if (user == null ||
            passwordHasher.VerifyHashedPassword(user.Password, model.Password) == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Username or password is incorrect");
        }

        // authentication successful so generate jwt and refresh tokens
        string jwtToken = _jwtUtils.GenerateJwtToken(user);
        RefreshToken refreshToken = _jwtUtils.GenerateRefreshToken();
        await _refreshTokenRepository.SetAsync(refreshToken);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }

    public async Task<AuthenticateResponse> RefreshToken(string token)
    {
        User? user = await _userRepository.GetByTokenAsync(token);
        RefreshToken? refreshToken = user != null ? await _refreshTokenRepository.GetAsync(user.Id) : null;

        if (user is null || (refreshToken?.IsExpired ?? true))
        {
            throw new UnauthorizedException("Invalid token");
        }

        RefreshToken newRefreshToken = _jwtUtils.GenerateRefreshToken();
        string newJwtToken = _jwtUtils.GenerateJwtToken(user);

        // replace token in db
        await _refreshTokenRepository.SetAsync(newRefreshToken);

        return new AuthenticateResponse(user, newJwtToken, newRefreshToken.Token);
    }

    public async Task RevokeToken(string token)
    {
        User? user = await _userRepository.GetByTokenAsync(token);
        RefreshToken? refreshToken = user != null ? await _refreshTokenRepository.GetAsync(user.Id) : null;

        if (refreshToken is null)
        {
            return;
        }

        await _refreshTokenRepository.DeleteAsync(token);
    }
}
