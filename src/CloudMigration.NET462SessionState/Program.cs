using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;

namespace CloudMigration.NET462SessionState
{
    class Program
    {
        /// <summary>
        /// Represents distributed cache of serialized values
        /// </summary>
        private static IDistributedCache _distributedCache;

        /// <summary>
        /// Represents the abstract multiplexer API that hides away details of multiple servers
        /// </summary>
        private static IConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// Describes functionality that is common to both standalone redis servers and redis clusters
        /// </summary>
        private static IDatabase _redisServer;

        static void Main(string[] args)
        {
            CloudConfig.RegisterConfigurations();

            _distributedCache = CloudConfig.GetService<IDistributedCache>()
                ?? throw new ArgumentNullException(nameof(IDistributedCache));

            _connectionMultiplexer = CloudConfig.GetService<IConnectionMultiplexer>()
                ?? throw new ArgumentNullException(nameof(IConnectionMultiplexer));
            _redisServer = _connectionMultiplexer.GetDatabase();

            SimplePingCommand();
            SimpleGetSessionState();
            SetSessionState();
            GetSessionState();
            GetClientList();
            StoreObjectSessionState();
            RetrieveObjectSessionState();

            _connectionMultiplexer.Dispose();

            SetCache();
            GetCache();
            StoreObjectCache();
            RetrieveObjectCache();

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Execute simple PING command
        /// </summary>
        static void SimplePingCommand()
        {
            var sessionStateCommand = "PING";
            Console.WriteLine("\nSession State command  : " + sessionStateCommand);
            Console.WriteLine("Session State response : " + _redisServer.Execute(sessionStateCommand).ToString());
        }

        /// <summary>
        /// Get simple session state with "Message" key
        /// </summary>
        static void SimpleGetSessionState()
        {
            var sessionStateCommand = "GET Session Message";
            Console.WriteLine("\nSession State command  : " + sessionStateCommand + " or StringGet()");
            Console.WriteLine("Session State response : " + _redisServer.StringGet("Message").ToString());
        }

        /// <summary>
        /// Set session state: put primitive data types into the session with "Message" key
        /// </summary>
        static void SetSessionState()
        {
            var sessionStateCommand = "SET Session Message \"Hello! The session state is working from a .NET console app!\"";
            Console.WriteLine("\nSession State command  : " + sessionStateCommand + " or StringSet()");
            Console.WriteLine("Session State response : " + _redisServer.StringSet("Message", "Hello! The Session State is working from a .NET console app!").ToString());
        }

        /// <summary>
        /// Get session state: demonstrate "SET Message" executed as expected with "Message" key value set
        /// </summary>
        static void GetSessionState()
        {
            var sessionStateCommand = "GET Session Message";
            Console.WriteLine("\nSession State command  : " + sessionStateCommand + " or StringGet()");
            Console.WriteLine("Session State response : " + _redisServer.StringGet("Message").ToString());
        }

        /// <summary>
        /// Execute CLIENT LIST command, useful to see if connection list is growing
        /// </summary>
        static void GetClientList()
        {
            var sessionStateCommand = "CLIENT LIST";
            Console.WriteLine("\nSession State command  : " + sessionStateCommand);
            Console.WriteLine("Session State response : \n" + _redisServer.Execute("CLIENT", "LIST").ToString().Replace("id=", "id="));
        }

        /// <summary>
        /// Store .NET object (NOT serialized) to session
        /// </summary>
        static void StoreObjectSessionState()
        {
            Employee e007 = new Employee("007", "Davide Columbo", 100);
            Console.WriteLine("Session State response from storing Employee .NET object : " +
                _redisServer.StringSet("e007", JsonConvert.SerializeObject(e007)));
        }

        /// <summary>
        /// Retrieve .NET object (NOT serialized) from session
        /// </summary>
        static void RetrieveObjectSessionState()
        {
            Employee e007FromSessionState = JsonConvert.DeserializeObject<Employee>(_redisServer.StringGet("e007"));
            Console.WriteLine("Deserialized Employee .NET object :\n");
            Console.WriteLine("\tEmployee.Name : " + e007FromSessionState.Name);
            Console.WriteLine("\tEmployee.Id   : " + e007FromSessionState.Id);
            Console.WriteLine("\tEmployee.Age  : " + e007FromSessionState.Age + "\n");
        }

        /// <summary>
        /// Set Cache Data
        /// </summary>
        static void SetCache()
        {
            var cacheCommand = "SET Cache Message \"KeyValue via RedisCache\"";
            Console.WriteLine("\nCache command  : " + cacheCommand + " or Set()");
            _distributedCache.Set("RedisCacheKey", Encoding.UTF8.GetBytes("KeyValue via RedisCache"));
        }

        /// <summary>
        /// Get Cache Data
        /// </summary>
        static void GetCache()
        {
            var cacheCommand = "GET Cache Message";
            Console.WriteLine("\nCache command  : " + cacheCommand + " or Get()");
            Console.WriteLine("Cache response : " + Encoding.UTF8.GetString(_distributedCache.Get("RedisCacheKey")));
        }

        /// <summary>
        /// Store .NET object to cache
        /// </summary>
        static void StoreObjectCache()
        {
            Employee e008 = new Employee("008", "Captain America", 200);
            _distributedCache.Set("e008", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e008)));
        }

        /// <summary>
        /// Retrieve .NET object from cache
        /// </summary>
        static void RetrieveObjectCache()
        {
            Employee e008FromCache = JsonConvert.DeserializeObject<Employee>(Encoding.UTF8.GetString(_distributedCache.Get("e008")));
            Console.WriteLine("Deserialized Employee .NET object :\n");
            Console.WriteLine("\tEmployee.Name : " + e008FromCache.Name);
            Console.WriteLine("\tEmployee.Id   : " + e008FromCache.Id);
            Console.WriteLine("\tEmployee.Age  : " + e008FromCache.Age + "\n");
        }
    }
}
