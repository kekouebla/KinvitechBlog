using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudMigrationASP.NET462SessionCache.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 10 - Represents distributed cache of serialized values
        /// </summary>
        private IDistributedCache _distributedCache;

        /// <summary>
        /// 11 - Represents the abstract multiplexer API that hides away details of multiple servers
        /// </summary>
        private IConnectionMultiplexer _connectionMultiplexer;
        private Lazy<ConnectionMultiplexer> _lazyConnection;

        /// <summary>
        /// 12 - Describes functionality that is common to both standalone redis servers and redis clusters
        /// </summary>
        private IDatabase _redisServer;

        /// <summary>
        /// 13 - Ges Redis client connection
        /// </summary>
        public HomeController()
        {
            _distributedCache = CloudConfig.GetService<IDistributedCache>()
                ?? throw new ArgumentNullException(nameof(IDistributedCache));

            _connectionMultiplexer = CloudConfig.GetService<IConnectionMultiplexer>()
                ?? throw new ArgumentNullException(nameof(IConnectionMultiplexer));
            _redisServer = _connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// 14 - Supports Redis action that runs some commands against the new session or cache
        /// </summary>
        /// <returns></returns>
        public ActionResult RedisSessionCache()
        {
            ViewBag.Message = "A simple example with Session or Cache for Redis on ASP.NET.";

            // Integrate the session or cache provider with existing ASP.NET app with web.config instead of appsettings.json
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                if (ConfigurationManager.AppSettings["CacheConnection"] != null)
                {
                    string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
                    return ConnectionMultiplexer.Connect(cacheConnection);
                }
                return null;
            });

            // Connection refers to a property that returns a ConnectionMultiplexer
            // as shown in the previous example.
            if (_lazyConnection.Value != null)
            {
                _redisServer = _lazyConnection.Value.GetDatabase();
            }            

            // Perform cache operations using the cache object...

            // Simple PING command
            ViewBag.command1 = "PING";
            ViewBag.command1Result = _redisServer.Execute(ViewBag.command1).ToString();

            // Simple get and put of integral data types into the cache
            ViewBag.command2 = "GET Message";
            ViewBag.command2Result = _redisServer.StringGet("Message").ToString();

            ViewBag.command3 = "SET Message \"Hello! The cache is working from ASP.NET!\"";
            ViewBag.command3Result = _redisServer.StringSet("Message", "Hello! The cache is working from ASP.NET!").ToString();

            // Demonstrate "SET Message" executed as expected...
            ViewBag.command4 = "GET Message";
            ViewBag.command4Result = _redisServer.StringGet("Message").ToString();

            // Get the client list, useful to see if connection list is growing...
            ViewBag.command5 = "CLIENT LIST";
            ViewBag.command5Result = _redisServer.Execute("CLIENT", "LIST").ToString().Replace(" id=", "\rid=");

            //if (_lazyConnection.Value != null)
            //{
            //    _lazyConnection.Value.Dispose();
            //}
            
            //_connectionMultiplexer.Dispose();

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}