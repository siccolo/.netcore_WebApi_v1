using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_v1.DataStore
{
    public interface IDataStore<T> //where T : DataModels.IDataEntity
    {
        //Task<IEnumerable<DataModels.IDataEntity>> GetAll();
        //Task<DataModels.IDataEntity> GetOne();
        //Task<IEnumerable<T>> GetAll(string query);
        IAsyncEnumerable<T> GetAll(string query);
        Task<T> GetOne(string query);
    }
}
