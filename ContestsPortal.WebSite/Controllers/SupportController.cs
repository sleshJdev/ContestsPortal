using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Controllers
{
    [AllowAnonymous]
    public class SupportController : AsyncController
    {

        public Task<ActionResult> Index()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }
           

        public Task<ActionResult> Faq()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }

        public Task<ActionResult> UsefulStuff()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }

        // also change history will be here
        public Task<ActionResult> SiteMap()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }

        public Task<ActionResult> Articles()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }
        
         public Task<ActionResult> Feedback()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }

         public Task<ActionResult> References()
         {
             return Task<ActionResult>.Factory.StartNew(() =>
             {
                 return View();
             });
         }

         public Task<ActionResult> WhoWeAre()
         {
             return Task<ActionResult>.Factory.StartNew(() =>
             {
                 return View();
             });
         }

        
        [Authorize(Roles = Roles.Member +","+Roles.Competitor+","+Roles.Moderator +Roles.Administrator+","+Roles.SuperAdministrator)]
        public ActionResult MemberOfferings()
        {
            return View();
        }

    }
}