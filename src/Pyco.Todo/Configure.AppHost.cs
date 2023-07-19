using Funq;
using ServiceStack;
using Pyco.Todo.ServiceInterface;

[assembly: HostingStartup(typeof(Pyco.Todo.AppHost))]

namespace Pyco.Todo;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public AppHost() : base("Pyco.Todo", typeof(MyServices).Assembly) {}

    public override void Configure(Container container)
    {
        // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages
        Plugins.Add(new SharpPagesFeature {
            EnableSpaFallback = true
        }); 

        SetConfig(new HostConfig {
            AddRedirectParamsToQueryString = true,
        });
    }
}
