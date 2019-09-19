namespace Shared
{
  
    public class RedisCacheRevocatonHandler : IRevocationMessageHandler
    {
        private readonly RedisConfiguation redisConfiguation;

        public RedisCacheRevocatonHandler(RedisConfiguation redisConfiguation)
        {
            this.redisConfiguation = redisConfiguation;
        }
        public void HandleMessage(dynamic message)
        {
            // extract the message properies & add logic to remove from redies cache
        }
    }

    public class InMemoryCacheRevocatonHandler : IRevocationMessageHandler
    {
        private readonly InMemConfiguation inMemConfiguation;

        public InMemoryCacheRevocatonHandler(InMemConfiguation inMemConfiguation)
        {
            this.inMemConfiguation = inMemConfiguation;
        }
        public void HandleMessage(dynamic message)
        {
            // extract the message properies & add logic to remove from redies cache
        }
    }

    public class InMemConfiguation
    {
    }
    // use existing configuration class or create new 
    public class RedisConfiguation
    {
    }
}
