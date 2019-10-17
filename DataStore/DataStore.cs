using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_v1.DataModels;

namespace WebAPI_v1.DataStore
{
    public class DataStore : IDataStore<DataModels.IDataEntity>
    {
        private readonly Models.IDiscosData _BackendData;
        private readonly Microsoft.Extensions.Logging.ILogger<DataStore> _Logger;

        public DataStore(Models.IDiscosData backendData, Microsoft.Extensions.Logging.ILogger<DataStore> logger)
        {
            _BackendData = backendData ?? throw new System.ArgumentNullException("backend is missing");
            _Logger = logger;
        }

        //public Task<IEnumerable<IDataEntity>> GetAll(string query)
        public IAsyncEnumerable<IDataEntity> GetAll(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IDataEntity> GetOne(string query)
        {
            throw new NotImplementedException();
        }
    }
}
