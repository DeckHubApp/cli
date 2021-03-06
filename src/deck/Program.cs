﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Deck.Commands;

namespace Deck
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("new", StringComparison.OrdinalIgnoreCase))
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
            return WebHost.CreateDefaultBuilder(args)
                 .UseUrls($"http://localhost:{port}")
                 .ConfigureAppConfiguration((_, config) =>
                 {
                     var userProfileConfig =
                         Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".deckhub",
                             "deckhub.json");
                     config.AddJsonFile(userProfileConfig, true)
                         .AddEnvironmentVariables("DECKHUB_")
                         .AddJsonFile("deckhub.json", true)
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
                         LogLevel logLevel = LogLevel.Error;
                         if (args.Any(a => a.StartsWith("--debug", StringComparison.OrdinalIgnoreCase)))
                         {
                             logLevel = LogLevel.Debug;
                         }
                         else if (args.Any(a => a.StartsWith("--verbose", StringComparison.OrdinalIgnoreCase)))
                         {
                             logLevel = LogLevel.Information;
                         }

                         logging.SetMinimumLevel(logLevel).AddConsole();
                     }
                 })
                 .UseStartup<Startup>()
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

        // Need to create the generic type in code for AoT compilation
        // ReSharper disable once UnusedMember.Global
        public static void ReflectionRoots()
        {
            SharpYaml.Serialization.Descriptors.DictionaryDescriptor.GetGenericEnumerable<string, object>(
                new Dictionary<string, object>());
        }
    }
}