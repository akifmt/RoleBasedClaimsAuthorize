using System.Web.Mvc;
using MatRoleClaim.Attributes;

namespace MatRoleClaim
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
