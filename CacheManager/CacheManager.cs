using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace FoodService
{
    public class CacheManager : ICacheManager
    {
        private static string _key = "foodtrucks";
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private IServiceProvider serviceProvider;
        
        public CacheManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task TrucksInCache(List<FoodTruck> trucks = null)
        {
            List<FoodTruck> itemsToAdd = trucks ?? await this.serviceProvider.LoadData().ConfigureAwait(false);

            var cacheItemPolicy = new CacheItemPolicy()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(60)
            };
            
            _cache.Add(_key, itemsToAdd, cacheItemPolicy);
        }
       
        public async Task<List<FoodTruck>> GetTrucksFromCache()
        {
            if (!_cache.Contains(_key))
            {
                await TrucksInCache().ConfigureAwait(false);
            }

            return _cache.Get(_key) as List<FoodTruck>;
        }

        public void RemoveTrucksFromCache(string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                _cache.Dispose();
            }
            else
            {
                _cache.Remove(_key);
            }
        }

        public Task TrucksInCache()
        {
            throw new NotImplementedException();
        }
    }
}

