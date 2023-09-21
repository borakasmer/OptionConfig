using Microsoft.AspNetCore.Mvc;
using OptionConfig.Redis;
using System.Reflection.Metadata;

namespace OptionConfig.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        IRedisCacheService _redisCacheManager;
        public RedisController(IRedisCacheService redisCacheManager)
        {
            _redisCacheManager = redisCacheManager;
        }

        [HttpGet(Name = "GetFuelPrice")]
        public IEnumerable<FuelData> Get()
        {
            return new List<FuelData>();
        }
        [HttpPost(Name = "InsertFuelPrice")]
        public bool Post(FuelData data)
        {
            string key = $"FuelData{Guid.NewGuid()}";
            _redisCacheManager.Set(key, data);
            return true;
        }
    }
}