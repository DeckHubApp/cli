using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddMvcCore().AddJsonFormatters();

            var slideableOptions = new SlidableOptions
            {
                Api = Configuration["Api"],
                ApiKey = Configuration["ApiKey"],
                Offline = string.Equals(Configuration["Offline"], "true", StringComparison.OrdinalIgnoreCase),
                Place = Configuration["Place"],
                Presenter = Configuration["Presenter"],
                Slug = Configuration["Slug"]
            };
            Console.WriteLine(slideableOptions.Slug);
            Things.SlidableOptions = slideableOptions;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Things.LoggerFactory = loggerFactory;
            Things.SlidableClient =
                new SlidableClient(Things.SlidableOptions, loggerFactory.CreateLogger<SlidableClient>());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            ConfigureRoutes(app);
            app.Run(ctx =>
            {
                ctx.Response.Redirect("/slidable/0");
                return Task.CompletedTask;
            });
        }

        private void ConfigureRoutes(IApplicationBuilder app)
        {
            app.UseRouter(routes =>
            {
                routes.MapPost("shot/{index}", (req, res, data) => ShotHandler.Handle(req, res, data));
            });
        }
    }
}