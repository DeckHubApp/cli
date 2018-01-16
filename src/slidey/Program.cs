using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using slidey.Commands;

namespace slidey
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 1 && args[0].Equals("new", StringComparison.OrdinalIgnoreCase))
            {
                var newCommand = new NewCommand(args);
                await newCommand.Execute();
                return;
            }
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var port = GetPort(args);
            return new WebHostBuilder()
                .UseUrls($"http://localhost:{port}")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((_, config) =>
                {
                    var userProfileConfig =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".slidey",
                            "slidey.json");
                    config.AddJsonFile(userProfileConfig, true)
                        .AddEnvironmentVariables("SLIDEY_")
                        .AddJsonFile("slidey.json", true)
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
                    services.Configure<SlideyOptions>(hostingContext.Configuration);
                    services.AddSingleton<ISlideyClient, SliderClient>();
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

        private static int GetPort(string[] args)
        {
            int pIndex = Array.IndexOf(args, "-p");
            if (pIndex < 0) pIndex = Array.IndexOf(args, "--port");
            if (pIndex > -1 && args.Length > pIndex + 1)
            {
                string str = args[pIndex];
                if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out int port))
                {
                    return port;
                }
            }
            return 5555;
        }

        public static void ReflectionRoots()
        {
            SharpYaml.Serialization.Descriptors.DictionaryDescriptor.GetGenericEnumerable<string, object>(new Dictionary<string,object>());
        }
    }
}
