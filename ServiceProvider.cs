using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FoodService
{
    public interface IServiceProvider
    {
        Task<List<FoodTruck>> LoadData();
    }
    public class ServiceProvider : IServiceProvider
    {
        public ServiceProvider()
        {
        }

        public async Task<List<FoodTruck>> LoadData()
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(@"https://data.sfgov.org/");

                HttpResponseMessage response = await httpClient.GetAsync(@"resource/rqzj-sfat.json?$limit=25").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var resultList = new List<FoodTruck>();
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    resultList = JsonConvert.DeserializeObject<List<FoodTruck>>(result);

                    return resultList;
                }

                throw new Exception("No data found");
            }
            catch (Exception ex)
            {
                // telemetry logging exception here
                throw ex;
            }
        }
    }
}
