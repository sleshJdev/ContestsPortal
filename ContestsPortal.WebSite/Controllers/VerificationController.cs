using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.App_Start;
using ContestsPortal.WebSite.ViewModels.Account;
using Microsoft.AspNet.Identity.Owin;

namespace ContestsPortal.WebSite.Controllers
{    
    public class VerificationController : Controller
    { 
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> VerificateUserName(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
            if (!Request.IsAjaxRequest()) return null;

            var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            bool isfree = await um.FindByNameAsync(username) == null;
            return Json(isfree, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<JsonResult> VerificateUserNickName(string nickname)
        {
            if (string.IsNullOrEmpty(nickname)) throw new ArgumentNullException("nickname");
            if (!Request.IsAjaxRequest()) return null;

            return Task<JsonResult>.Factory.StartNew(() =>
            {
                using (var context = HttpContext.GetOwinContext().Get<PortalContext>())
                {
                    var uppernick = nickname.ToUpper();
                    var isFree = context.Users.SingleOrDefault(x => x.NickName.ToUpper().Equals(uppernick)) == null;
                    return Json(isFree, JsonRequestBehavior.DenyGet);
                }
            });
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> VerificateUserEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
            if (!Request.IsAjaxRequest()) return null;
            var um = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            bool isfree = await um.FindByEmailAsync(email) == null;
            return Json(isfree, JsonRequestBehavior.DenyGet);
        }
    }
}