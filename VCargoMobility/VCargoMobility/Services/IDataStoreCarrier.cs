using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VCargoMobility.Services
{
 public   interface IDataStoreCarrier<T>
    {
        Task<bool> AddCarrierAsync(T item);
        Task<bool> UpdateCarrierAsync(T item);
        Task<bool> DeleteCarrierAsync(string id);
        Task<T> GetCarrierAsync(string id);
        Task<IEnumerable<T>> GetCarrierAsync(bool forceRefresh = false);

    }
}
