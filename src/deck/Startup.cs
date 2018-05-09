using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Deck.Routes;

namespace Deck
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureRoutes(app, loggerFactory);
            
            app.Run(ctx =>
            {
                ctx.Response.Redirect("/deckhub/0");
                return Task.CompletedTask;
            });
        }

        private void ConfigureRoutes(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var options = DeckHubOptions.Bind(Configuration);
            var client =
                new DeckHubClient(options, loggerFactory.CreateLogger<DeckHubClient>());
            
            app.UseRouter(routes =>
            {
                InternalFilesRouter.Add(routes);
                ThemeRouter.Add(routes);
                ImagesRouter.Add(routes);
                DeckRouter.Add(routes, client, options, loggerFactory);
                ShotRouter.Add(routes, client, options, loggerFactory);
            });
        }
    }
}