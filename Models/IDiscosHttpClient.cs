using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.Models
{
    public interface IDiscosHttpClient
    {
        Task<string> PerformSearch(SearchType searchType, string query);
        Task<string> ById(SearchType searchType, string id);
    }
}
