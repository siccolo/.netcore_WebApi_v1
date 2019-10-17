using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.Models
{
    public class DiscosData:IDiscosData
    {

        private readonly IDiscosHttpClient _DiscosHttpClient;
        public DiscosData(IDiscosHttpClient puller)
        {
            _DiscosHttpClient = puller;
        }

        public async Task<string> Search(SearchType searchType, string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query is empty");
            }
            return await _DiscosHttpClient.PerformSearch(searchType, query);
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
            return await _DiscosHttpClient.ById(searchType, id);
        }

    }
}
