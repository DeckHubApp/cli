using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slidable.Routes;

namespace Slidable
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
                ctx.Response.Redirect("/slidable/0");
                return Task.CompletedTask;
            });
        }

        private void ConfigureRoutes(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var options = SlidableOptions.Bind(Configuration);
            var client =
                new SlidableClient(options, loggerFactory.CreateLogger<SlidableClient>());
            
            app.UseRouter(routes =>
            {
                InternalFilesRouter.Add(routes);
                ThemeRouter.Add(routes);
                ImagesRouter.Add(routes);
                SlidableRouter.Add(routes, client, options, loggerFactory);
                ShotRouter.Add(routes, client, options, loggerFactory);
            });
        }
    }
}