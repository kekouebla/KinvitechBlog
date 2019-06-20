using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudMigration.NET462SessionState
{
    /// <summary>
    /// 9 - Serves the connection string to Redis Session State Provider mentinoned in web.config/app.config
    /// </summary>
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
