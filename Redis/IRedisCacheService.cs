namespace OptionConfig.Redis
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);
        void Set(string key, object data);
        void Set(string key, object data, DateTime time);
        void Remove(string key);
        void Clear();
    }
}
