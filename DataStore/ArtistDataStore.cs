using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_v1.DataModels;

namespace WebAPI_v1.DataStore
{
    public class ArtistDataStore: IDataStore<DataModels.Artist>
    {
        private readonly Models.IDiscosData _BackendData;
        private readonly Microsoft.Extensions.Logging.ILogger<ArtistDataStore> _Logger;

        public ArtistDataStore(Models.IDiscosData backendData, Microsoft.Extensions.Logging.ILogger<ArtistDataStore> logger)
        {
            _BackendData = backendData ?? throw new System.ArgumentNullException("backend is missing");
            _Logger = logger;
        }


        //public Task<IEnumerable<Artist>> GetAll(string query)
        public async IAsyncEnumerable<Artist> GetAll(string query)
        {
            var jsonresult =  await _BackendData.Search(Models.SearchType.Artist, query);
            var d = Newtonsoft.Json.JsonConvert.DeserializeObject<SearchResult>(jsonresult);
            var list = d.results;
            foreach(var result in list)
            {
                jsonresult = await _BackendData.ById(Models.SearchType.Artist, result.id.ToString());
                var artist = Newtonsoft.Json.JsonConvert.DeserializeObject<DataModels.Artist>(jsonresult);
                yield return artist;
            }
        }

        public Task<Artist> GetOne(string query)
        {
            throw new NotImplementedException();
        }
    }
}
