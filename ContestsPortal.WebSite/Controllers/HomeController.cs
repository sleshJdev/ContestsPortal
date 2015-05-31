using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.Infrastructure.ActionAttributes;
using Microsoft.Owin;
using System.Web.Security;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System;

namespace ContestsPortal.WebSite.Controllers
{    
    [AllowAnonymous]
    public class HomeController : AsyncController
    {
        public Task<ActionResult> Index()
        {
            return Task<ActionResult>.Factory.StartNew(() =>
            {
                using (var context = new PortalContext())
                {
                    IOwinContext owin = HttpContext.GetOwinContext();
                    context.Database.Initialize(true);
                }
                return View();
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Task<ActionResult> ContestsHistory()
        {
            return Task<ActionResult>.Factory.StartNew(() => { return View(); });
        }


        public Task<ActionResult> ArchivedTasks()
        {
            return Task<ActionResult>.Factory.StartNew(() => { return View(); }, CancellationToken.None,
                TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        [NoCache]
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            var list = DefineMainMenuRefs();
            return View("MenuView", list);
        }


        public Task<ActionResult> CompetitorsRating()
        {
            return Task<ActionResult>.Factory.StartNew(() => { return View(); });
        }

        public Task<ActionResult> ContestResult(int? contestId)
        {
            return Task<ActionResult>.Factory.StartNew(() => { return View(); }, CancellationToken.None,
                TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }


        public Task<ActionResult> SomeMethod()
        {

            return null;
        }


        #region Stuff


        private IList<MenuItem> DefineMainMenuRefs()
        {
            List<MenuItem> list;
            using (var context = new PortalContext())
            {
                list = context.Set<MenuItem>()
                    .Include("SubItems")
                    .Where(x => x.IdParentMenuItem == null && x.IdMenuItemCategory == 1)
                    .OrderBy(x => x.OrderNumber)
                    .ToList();
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole(Roles.SuperAdministrator))
                    {list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.SuperAdministrator)
                                               || x.MinimalAccessibleRole.Equals(Roles.Administrator)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                    return list;
                    }
                    

                    if (User.IsInRole(Roles.Administrator))
                    { list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Administrator)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                        return list;
                    }

                    if (User.IsInRole(Roles.Moderator))
                    {
                        list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Moderator)
                                               || x.MinimalAccessibleRole.Equals(Roles.Member)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                        return list;
                    }

                    if (User.IsInRole(Roles.Judge))
                    {list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Judge)
                                               || x.MinimalAccessibleRole.Equals(Roles.Member)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                    return list;
                    }

                    if (User.IsInRole(Roles.Competitor))
                    { list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Competitor)
                                               || x.MinimalAccessibleRole.Equals(Roles.Member)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                        return list;
                    }

                    if (User.IsInRole(Roles.Member))
                    {list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Member)
                                               || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                        return list;
                    }
                }
                else list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                return list;
            }
        }

        #endregion
    }
}