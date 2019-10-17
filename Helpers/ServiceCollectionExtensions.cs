using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//
namespace WebAPI_v1.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
                                                                                this IServiceCollection services
                                                                                , IConfiguration configuration
                                                                                , string configurationSectionName)
                                                        where TClient : class
                                                        where TImplementation : class, TClient
                                                        where TClientOptions : Models.DiscosHttpClientOptions, new() =>
        services
            .Configure<TClientOptions>(configuration.GetSection(configurationSectionName))
            //.AddTransient<CorrelationIdDelegatingHandler>()
            .AddTransient<UserAgentDelegatingHandler>()
            .AddHttpClient<TClient, TImplementation>()
            .ConfigureHttpClient(
                (sp, options) =>
                {
                    var httpClientOptions = sp
                        .GetRequiredService<IOptions<TClientOptions>>()
                        .Value;
                    options.BaseAddress = httpClientOptions.BaseAddress;

                    options.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", httpClientOptions.Authorization);
                    //options.Timeout = httpClientOptions.Timeout;
                })
            .ConfigurePrimaryHttpMessageHandler(x => new Helpers.DefaultHttpClientHandler())
            //.AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
            //.AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
            //.AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            .AddHttpMessageHandler<UserAgentDelegatingHandler>()
            .Services;

        public static IServiceCollection AddDiscogsServices(this IServiceCollection services, IConfiguration configuration)
        {
           var s= services
                .AddTransient(typeof(Models.IDiscosData), typeof(Models.DiscosData))
                .AddTransient(typeof(DataStore.IDataStore<DataModels.Artist>), typeof(DataStore.ArtistDataStore))
            ;

            return s;
        }
      
       
    }
}
