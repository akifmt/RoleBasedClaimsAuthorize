using System.Web.Mvc;
using System.Data.Entity;

namespace MatRoleClaim.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Your blog page.";

            return View(DbContext.Blogs.Include(x => x.Author));
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