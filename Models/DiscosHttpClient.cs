using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebAPI_v1.Models
{
    public class DiscosHttpClient: IDiscosHttpClient 
    {
        private readonly HttpClient _HttpClient;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment  _HostingEnvironment;
        //public DiscosHttpClient(HttpClient httpClient) => this.httpClient = httpClient;
        public DiscosHttpClient(HttpClient httpClient, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            this._HttpClient = httpClient;
            this._HostingEnvironment = hostingEnvironment;
        }

        public async Task<string> PerformSearch(SearchType searchType, string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query is empty");
            }

            //https://api.discogs.com/database/ --> "https://api.discogs.com/database/search?q={query}";
            string url = $"{_HttpClient.BaseAddress.ToString().TrimEnd('/')}/database/search?q={query}" 
                    + (( searchType.Equals(SearchType.Artist) || searchType.Equals(SearchType.Title)    )?$"&type={searchType.Value}":"");
            /*
            var response = await this._HttpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
            */
            using (var response = await this._HttpClient.GetAsync(url))
            {
                if (_HostingEnvironment.EnvironmentName == "Development")
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string s = await response.Content.ReadAsStringAsync();
                        return s;
                    }
                    else
                    {
                        var ex = new System.InvalidOperationException(url + " " + response.ReasonPhrase + " " + response.RequestMessage.ToString());
                        throw ex;
                    }
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<string> ById(SearchType searchType, string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id is empty");
            }
            if (!searchType.Equals(SearchType.Artist) && !searchType.Equals(SearchType.Title))
            {
                throw new ArgumentOutOfRangeException("invalid type");
            }
          

            //https://api.discogs.com/ --> "https://api.discogs.com/artists/999433;
            string url = $"{_HttpClient.BaseAddress.ToString().TrimEnd('/')}/{searchType.Value}s/{id}";
            using (var response = await this._HttpClient.GetAsync(url))
            {
                if (_HostingEnvironment.EnvironmentName == "Development")
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string s = await response.Content.ReadAsStringAsync();
                        return s;
                    }
                    else
                    {
                        var ex = new System.InvalidOperationException(url + " " + response.ReasonPhrase + " " + response.RequestMessage.ToString());
                        throw ex;
                    }
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
