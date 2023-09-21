using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using System.Runtime;
using System.Text;
using System.Xml;
using static ServiceStack.Diagnostics;

namespace OptionConfig.Redis
{
    public class RedisCacheService : IRedisCacheService
    {

        public readonly IOptionsMonitor<RedisConfig> _config;
        //public readonly IOptionsSnapshot<RedisConfig> _config;
        IConfiguration _configuration = null;
        private RedisEndpoint conf = null;

        public RedisCacheService(IOptionsMonitor<RedisConfig> config, IConfiguration configuration)
        {
            _config = config;
            //_configuration = configuration;

            conf = _config.CurrentValue.IsSsl ?
                new RedisEndpoint { Host = _config.CurrentValue.RedisEndPoint, Port = Convert.ToInt32(_config.CurrentValue.RedisPort), Password = _config.CurrentValue.RedisPassword, Ssl = true, SslProtocols = System.Security.Authentication.SslProtocols.Tls12 }
                :
                new RedisEndpoint { Host = _config.CurrentValue.RedisEndPoint, Port = Convert.ToInt32(_config.CurrentValue.RedisPort), Password = _config.CurrentValue.RedisPassword };
            _config.OnChange(Listener);
            //conf = new RedisEndpoint { Host = _config.Value.RedisEndPoint, Port = Convert.ToInt32(_configuration.GetSection("RedisConfig")["RedisPort"]), Password = _config.Value.RedisPassword };
        }

        private void Listener(RedisConfig _config)
        {
            //conf = new RedisEndpoint { Host = _config.RedisEndPoint, Port = Convert.ToInt32(_config.RedisPort), Password = _config.RedisPassword };
            conf = _config.IsSsl ?
                new RedisEndpoint { Host = _config.RedisEndPoint, Port = Convert.ToInt32(_config.RedisPort), Password = _config.RedisPassword, Ssl = true, SslProtocols = System.Security.Authentication.SslProtocols.Tls12 }
                :
                new RedisEndpoint { Host = _config.RedisEndPoint, Port = Convert.ToInt32(_config.RedisPort), Password = _config.RedisPassword };
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    return client.Get<T>(key);
                }
            }
            catch
            {
                throw new RedisNotAvailableException();
                //return default;
            }
        }


        public void Remove(string key)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    client.Remove(key);
                }
            }
            catch
            {
                throw new RedisNotAvailableException();
            }
        }

        public void Set(string key, object data, DateTime time)
        {
            try
            {
                using (IRedisClient client = new RedisClient(conf))
                {
                    var dataSerialize = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                    client.Set(key, Encoding.UTF8.GetBytes(dataSerialize), time);
                }
            }
            catch
            {
                throw new RedisNotAvailableException();
            }
        }

        public void Set(string key, object data)
        {
            Set(key, data, DateTime.Now.AddMinutes(_config.CurrentValue.RedisExpireTime));
        }
    }
}
