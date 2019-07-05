using System.Web;
using System.Web.Mvc;

namespace CloudMigrationASP.NET462Auth0
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
