using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
//
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
//
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//
using Microsoft.AspNetCore.Http;
//
using WebAPI_v1.Helpers;
//

namespace WebAPI_v1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecksUI()
                    .AddHealthChecks()
                    .AddCheck<Helpers.HealthCheck>("api")
                    /*
                        install-package AspNetcore.HealthChecks.Publisher.*
                        .AddApplicationInsightsPublisher()
                        .AddUrlGroup(new Uri("http://httpbin.org/status/200"))
                    */
                    ;

            //  httpclient factory:
            /*
            services.AddHttpClient("discogs.com", c =>
            {
                c.BaseAddress = new Uri("https://api.discogs.com/database/");
                c.DefaultRequestHeaders.Add("Authorization",  $"Discogs key={Key}, secret={Secret}");
                c.DefaultRequestHeaders.Add("User-Agent", "WebApi-for-Discogs");
            });
            */
            services.AddHttpClient<Models.IDiscosHttpClient, Models.DiscosHttpClient, Models.DiscosHttpClientOptions>(this.Configuration, "Discogs.com");
        
            //discogs related 
            services.AddDiscogsServices(this.Configuration);

            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.AddErrorHandler();
            }
            //

            loggerFactory.AddLog4Net();

            //  health checks
            /*
            app
                .UseHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthCheckResponse
                })
                .UseHealthChecks("/healthui", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
                })
                //.UseHealthChecksUI(setup => { setup.ApiPath = "/health"; setup.UIPath = "/healthui"; })
                ;
                */
            //

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecksUI();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = HealthCheckResponse
                });
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/healthui"; // this is ui path in your browser
                    setup.ApiPath = "/health-ui-api"; // the UI ( spa app )  use this path to get information from the store ( this is NOT the healthz path, is internal ui api )
                });
                endpoints.MapControllers();
            });

            #region swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger/ui";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI(v1)");
            });
            #endregion
        }

        #region HealthCheck output
        private static Task HealthCheckResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));
            return httpContext.Response.WriteAsync(     json.ToString(Formatting.Indented)  );
        }
        #endregion
    }
}
