using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace shtik
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((_, config) =>
                {
                    var userProfileConfig =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".shtik",
                            "shtik.json");
                    config.AddJsonFile(userProfileConfig, true)
                        .AddEnvironmentVariables("SHTIK_")
                        .AddJsonFile("shtik.json", true)
                        .AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.SetMinimumLevel(LogLevel.Information)
                            .AddConsole()
                            .AddDebug();
                    }
                    else
                    {
                        logging.SetMinimumLevel(LogLevel.Warning).AddConsole();
                    }
                })
                .UseDefaultServiceProvider((hostingContext, options) =>
                {
                    options.ValidateScopes = hostingContext.HostingEnvironment.IsDevelopment();
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.Configure<ShtikOptions>(hostingContext.Configuration);
                    services.AddSingleton<IShtikClient, ShtikClient>();
                    services.AddRouting();
                })
                .Configure(app =>
                {
                    app.UseRouter(Routes.Router);
                    app.Run(ctx =>
                    {
                        ctx.Response.Redirect("/0");
                        return Task.CompletedTask;
                    });
                })
                .Build();
        }
    }
}
