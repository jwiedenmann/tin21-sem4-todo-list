using Microsoft.IdentityModel.Tokens;
using Pyco.Todo.Data.Jwt;
using Pyco.Todo.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pyco.Todo.Core.Authorization;

public class JwtUtils : IJwtUtils
{
    private readonly JwtOptions _jwtOptions;

    public JwtUtils(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateJwtToken(User user)
    {
        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
        Claim[] claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username)
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.JwtExpirationMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public IEnumerable<Claim>? ValidateJwtToken(string? token)
    {
        if (token == null) return null;

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

            return jwtToken.Claims;
        }
        catch
        {
            return null;
        }
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpirationMinutes),
            Created = DateTime.UtcNow
        };

        return refreshToken;

        //string getUniqueToken()
        //{
        //    // token is a cryptographically strong random sequence of values
        //    var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        //    // ensure token is unique by checking against db
        //    var tokenIsUnique = !_context.Users.Any(u => u.RefreshTokens.Any(t => t.Token == token));

        //    if (!tokenIsUnique)
        //        return getUniqueToken();

        //    return token;
        //}
    }
}