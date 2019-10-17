using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.Models
{
    public interface IDiscosData
    {
        Task<string> Search(SearchType searchType, string query);
        Task<string> ById(SearchType searchType, string id);
    }

    public   class SearchType
    {
        private SearchType(string value) { Value = value; }

        public string Value { get; set; }

        public static SearchType Artist { get { return new SearchType("artist"); } }
        public static SearchType Title { get { return new SearchType("title"); } }
        public static SearchType Query { get { return new SearchType("query"); } }

        public override bool Equals(object obj)
        {
            return Value == ((SearchType)obj).Value;
        }
    }
}
