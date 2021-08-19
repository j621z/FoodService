using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodService
{
    public interface ICacheManager
    {
        Task TrucksInCache(List<FoodTruck> trucks=null);
        Task<List<FoodTruck>> GetTrucksFromCache();
        void RemoveTrucksFromCache(string _key);
    }
}
