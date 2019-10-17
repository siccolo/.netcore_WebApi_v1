using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace WebAPI_v1.Helpers
{
    public class DefaultHttpClientHandler : System.Net.Http.HttpClientHandler
    {
        public DefaultHttpClientHandler() =>
            this.AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip;
    }
}
