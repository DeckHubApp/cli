using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
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
    }
}
