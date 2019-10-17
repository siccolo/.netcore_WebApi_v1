using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistController : ControllerBase
    {
        private readonly Microsoft.Extensions.Logging.ILogger<ArtistController> _Logger;
        private readonly DataStore.IDataStore<DataModels.Artist> _DataStore;

        public ArtistController(DataStore.IDataStore<DataModels.Artist> dataStore, Microsoft.Extensions.Logging.ILogger<ArtistController> logger)
        {
            _Logger = logger;
            _DataStore = dataStore ?? throw new System.ArgumentNullException("datastore is missing");
        }

        [HttpGet]
        public IAsyncEnumerable<DataModels.Artist> Search(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentNullException("query is empty");
            }
            return _DataStore.GetAll(query);
        }
    }
}