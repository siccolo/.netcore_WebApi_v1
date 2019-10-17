using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.DataModels
{

    public class Image
    {
        public int height { get; set; }
        public string resource_url { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
        public string uri150 { get; set; }
        public int width { get; set; }
    }

    public class Member
    {
        public bool active { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string resource_url { get; set; }
    }

    public class Artist: DataModels.IDataEntity
    {
        public List<string> namevariations { get; set; }
        public string profile { get; set; }
        public string releases_url { get; set; }
        public string resource_url { get; set; }
        public string uri { get; set; }
        public List<string> urls { get; set; }
        public string data_quality { get; set; }
        public int id { get; set; }
        public List<Image> images { get; set; }
        public List<Member> members { get; set; }
    }
}
