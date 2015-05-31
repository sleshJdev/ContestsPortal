using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContestsPortal.WebSite.App_Start;
using Microsoft.AspNet.Identity.Owin;

namespace ContestsPortal.WebSite.Controllers
{
    [Authorize(Roles = Roles.Member)]
    public class MemberController : Controller
    {
        // GET: Member
        public async Task<ActionResult> Index()
        {
            var um =  HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await  um.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(user);
        }

        [AllowAnonymous]
        public Task<ActionResult> Forum()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                return View();
            });
        }
    }
}