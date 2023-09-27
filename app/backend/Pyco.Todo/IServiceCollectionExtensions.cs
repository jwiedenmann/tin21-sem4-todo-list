using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mqtt.Client.AspNetCore.Services;
using MQTTnet.Client;
using Pyco.Todo.Core.Authorization;
using Pyco.Todo.Core.Mqtt;
using Pyco.Todo.Data.Jwt;
using Pyco.Todo.DataAccess.DataProvider;
using Pyco.Todo.DataAccess.Interfaces;
using Pyco.Todo.DataAccess.Repositories;

namespace Pyco.Todo;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddAuthentication(services, configuration);
        AddMqtt(services, configuration);

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
        services.AddScoped<IListRepository, ListRepository>();
        services.AddSingleton<IListItemRepository, ListItemRepository>();

        services.AddScoped<IListItemDataProvider, ListItemDataProvider>();
        services.AddScoped<IListDataProvider, ListDataProvider>();
    }

    private static void AddMqtt(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MqttHelper>();
        services.AddMqttClientServiceWithConfig(aspOptionBuilder =>
        {
            aspOptionBuilder
                .WithCredentials("user1", "1234")
                //.WithClientId(clientSettinigs.Id)
                //.WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port)
                .WithWebSocketServer(x => x.WithUri("ws://localhost:9001/mqtt"));
        });
    }

    private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<MqttClientOptionsBuilder> configure)
    {
        services.AddSingleton(serviceProvider =>
        {
            var optionBuilder = new MqttClientOptionsBuilder();
            configure(optionBuilder);
            return optionBuilder.Build();
        });
        services.AddSingleton<MqttListItemUpdateService>();
        services.AddSingleton<IHostedService>(serviceProvider =>
        {
            return serviceProvider.GetService<MqttListItemUpdateService>()!;
        });
        return services;
    }
}
