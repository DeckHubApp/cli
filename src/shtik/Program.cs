using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var configRoot = LoadConfig(args);
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.Configure<ShtikOptions>(configRoot);
                    services.AddRouting();
                })
                .Configure(app =>
                    {
                        app.UseRouter(Routes.Router);
                        app.Run(ctx =>
                        {
                            ctx.Response.Redirect("/1");
                            return Task.CompletedTask;
                        });
                    })
                    .Build();
        }

        private static IConfigurationRoot LoadConfig(string[] args)
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables("SHTIK_")
                .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "shtik.json"))
                .AddCommandLine(args)
                .Build();
        }
    }
}
