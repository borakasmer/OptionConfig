namespace OptionConfig.Redis
{
    public class RedisConfig
    {
        public RedisConfig() { }
        public string RedisEndPoint { get; set; }
        public string RedisPort { get; set; }
        public string RedisPassword { get; set; }
        public int RedisExpireTime { get; set; }
        public bool IsSsl { get; set; }
    }
}
