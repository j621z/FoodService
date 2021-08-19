using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodTruckController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ICacheManager cacheManager;

        public FoodTruckController(ILogger logger, ICacheManager cacheManager)
        {
            this.logger = logger;
            this.cacheManager = cacheManager;
        }

        [HttpGet]
        [Route("api/getalltrucks")]
        public async Task<IActionResult> GetTrucksAsync()
        {
            var result = await this.cacheManager.GetTrucksFromCache().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        [Route("api/gettruck")]
        public async Task<IActionResult> GetTruckAsync([FromQuery] int id)
        {
            if (id <= 0) { throw new ArgumentNullException(nameof(id), "Truck id cannot be null"); };

            return await this.GetTruckAsyncInternal(id).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("api/addtruck")]
        public async Task<IActionResult> AddTruckAsync([FromBodyAttribute] FoodTruck foodTruck)
        {
            // tst with postman or fake here
            // in reality unit test to ensure it's not null
            foodTruck = new FoodTruck()
            {
                TruckId = 123489,
                Address = @"One Microsoft way, redmond",
                Block = 9873,
                TruckName = "JZTruck"
            };
            
            var trucks = await this.cacheManager.GetTrucksFromCache().ConfigureAwait(false);
            trucks.Add(foodTruck);

            this.cacheManager.RemoveTrucksFromCache("foodtrucks");

            try
            { 
                await this.cacheManager.TrucksInCache(trucks).ConfigureAwait(false);

                var x = await GetTruckAsyncInternal(foodTruck.TruckId);
                return Ok(x);
            }
            catch (Exception ex)
            {
                this.logger?.LogError(ex, "Failure adding new truck");
                throw ex;
            }
        }

        private async Task<IActionResult> GetTruckAsyncInternal(int id)
        {
            var trucks = await this.cacheManager.GetTrucksFromCache().ConfigureAwait(false);
            var truck = trucks.Find(x => x.TruckId == id);

            if (truck != null)
            {
                return Ok(truck);
            }
            else
            {
                return Ok("Truck not found");
            }
        }
    }
}
