using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace WebAPI_v1 
{
    public interface IApiSettings
    {
        string UrlToCall { get; set; }
    }
    public class ApiSettings:IApiSettings
    {
        public string UrlToCall { get; set; }
        public ApiSettings()
        {
            Console.WriteLine($"ApiSettings");
        }
    }

    public class IInfraApp
    {

    }

    public class InfraApp : IInfraApp
    {
        private IApiSettings _ApiSettings;
        public InfraApp(IOptions<ApiSettings> settings)
        {
            _ApiSettings = (IApiSettings)settings.Value;
            Console.WriteLine($"InfraApp {_ApiSettings.UrlToCall}");
        }
    }
}
