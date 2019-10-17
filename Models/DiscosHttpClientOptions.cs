using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.Models
{
    public class DiscosHttpClientOptions
    {
        public Uri BaseAddress { get; set; } = new System.Uri("https://api.discogs.com/database/");
        public string Key { get; set; } = "";
        public string Secret { get; set; } = "";

        public string Authorization
        {
            get
            {
                return  $"Discogs key={Key}, secret={Secret}";
            }
        }

    }
}
