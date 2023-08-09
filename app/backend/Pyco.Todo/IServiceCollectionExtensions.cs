using Pyco.Todo.Core.Authorization;
using Pyco.Todo.Data.Jwt;
using Pyco.Todo.DataAccess;

namespace Pyco.Todo;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddAuthentication(services, configuration);
        return services;
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOptions = configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>();

        services.AddSingleton(jwtOptions);
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IJwtUtils, JwtUtils>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
