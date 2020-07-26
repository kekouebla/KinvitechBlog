using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudMigrationASP.NET462SessionCache.App_Start
{
    public class RedisConfig
    {
        static IConnectionMultiplexer _connectionMultiplexer;
        static RedisConfig()
        {
            _connectionMultiplexer = CloudConfig.GetService<IConnectionMultiplexer>()
                ?? throw new ArgumentNullException(nameof(IConnectionMultiplexer));
        }

        public static string GetConnectionString()
        {
            return _connectionMultiplexer.Configuration;
        }
    }
}