using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCargoMobility.Models;

namespace VCargoMobility.Services
{
    public class CarrierMockDataStore : IDataStoreCarrier<Carrier>
    {

        private List<Carrier> Carriers;
        public CarrierMockDataStore()
        {

            //Carriers.Add(new Carrier()
            //{
            //    id = Guid.NewGuid().ToString(),
            //    CarrierCode = "Air21",
            //    CarrierName = "Air 21"
            //});

            //Carriers.Add(new Carrier()
            //{
            //    id = Guid.NewGuid().ToString(),
            //    CarrierCode = "PHAir",
            //    CarrierName = "Phillipine Air Line"
            //});

            Carriers = new List<Carrier>()
            {
                new Carrier {id =Guid.NewGuid().ToString (), CarrierName="Air 21" },
                new Carrier {id =Guid.NewGuid().ToString (), CarrierName="Philippine Air Line" },
                new Carrier {id =Guid.NewGuid().ToString (), CarrierName="Air Bus." },
                
            };

            //var client = new RestClient("https://18.139.49.140:8090/api/employee");
            //client.Timeout = -1;
            //var request = new RestRequest(Method.GET);

            //request.AddParameter("application/json", "", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

            //if (response.IsSuccessful == true)
            //{
            //    //var data = JsonConvert.DeserializeObject(response.Content)
            //    List<Carrier> result = (List<Carrier>)JsonConvert.DeserializeObject(response.Content, typeof(List<Carrier>));


            //    Carriers = new List<Carrier>();
            //    foreach (Carrier book in result)
            //    {
            //        Carriers.Add(new Carrier()
            //        {
            //            id = Guid.NewGuid().ToString (),
            //            CarrierCode = book.CarrierCode,
            //            CarrierName = book.CarrierName
            //        });
            //    }
            //}

        }

        public Task<bool> AddCarrierAsync(Carrier item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCarrierAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Carrier> GetCarrierAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Carrier>> GetCarrierAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Carriers);
        }

        public Task<bool> UpdateCarrierAsync(Carrier item)
        {
            throw new NotImplementedException();
        }
    }
}
