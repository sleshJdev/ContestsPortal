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
using System;
using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using System.Diagnostics;

namespace ContestsPortal.WebSite.Controllers
{    
    [AllowAnonymous]
    public class HomeController : AsyncController
    {
        private readonly IArchivedTaskProvider _archivedTaskProvider;
        private readonly IContestsProvider _contestsProvider;
        private readonly IPostProvider _postProvider;

        #region Constructors

        public HomeController()
        {
        }

        public HomeController(IContestsProvider contestsProvider, IArchivedTaskProvider archivedTaskProvider, IPostProvider postProvider)
        {
            if (contestsProvider == null) throw new ArgumentNullException("contestsProvider");
            if (archivedTaskProvider == null) throw new ArgumentNullException("archivedTaskProvider");
            if (postProvider == null) throw new ArgumentNullException("postProvider");

            _contestsProvider = contestsProvider;
            _archivedTaskProvider = archivedTaskProvider;
            _postProvider = postProvider;
        }

        #endregion Constructors

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

        public async Task<ActionResult> ContestsDetails(int contestId)
        {
            Contest contest = await _contestsProvider.GetContest(contestId);

            Debug.WriteLine("HomeController.ContestsDetails. id: " + contestId + ", " + contest.Tasks.Count);

            return View("ContestsDetails", contest);
        }

        public async Task<ActionResult> ContestsHistory()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsAsync();

            return View("ContestsHistory", contests);
        }

        public async Task<ActionResult> ArchivedTaskDetails(int taskId)
        {
            ArchivedTask task = await _archivedTaskProvider.GetArchivedTaskAsync(taskId);
            Debug.WriteLine("HomeController.ArchivedTaskDetails. id: " + taskId);

            return View("ArchivedTaskDetails", task);   
        }
        
        public async Task<ActionResult> ArchivedTasks()
        {
            IList<ArchivedTask> tasks = await _archivedTaskProvider.GetAllArchivedTasksAsync();
            Debug.WriteLine("HomeController.ArchivedTasks Count: " + tasks.Count);
            tasks = new[]
            {
                new ArchivedTask(){TaskTitle = "ArchivedTask1", TaskContent = "ArchivedTask1 content", TaskComplexity = 5},
                new ArchivedTask(){TaskTitle = "ArchivedTask2", TaskContent = "ArchivedTask2 content", TaskComplexity = 10},
                new ArchivedTask(){TaskTitle = "ArchivedTask3", TaskContent = "ArchivedTask3 content", TaskComplexity = 15},
                new ArchivedTask(){TaskTitle = "ArchivedTask4", TaskContent = "ArchivedTask4 content", TaskComplexity = 20},
            };

            return View("ArchivedTasks", tasks);           
        }

        [NoCache]
        [ChildActionOnly]
        public async Task<PartialViewResult> MainMenu()
        {
            var list = await DefineMainMenuRefs();
            return PartialView("MenuView", list);
        }

        [NoCache]
        [ChildActionOnly]
        public async Task<PartialViewResult> Posts()
        {
            IList<Post> posts = await _postProvider.GetAllPosts();
            posts = new[]
            {
              new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
              new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
              new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now},
              new Post(){PostContent = "Олимпиада bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb", PublicationDate = DateTime.Now}
            };

            return PartialView("_Posts", posts);
        }

        public async Task<ActionResult> PostDetails(int postId)
        {
            Post post = await _postProvider.GetPost(postId);

            return View("_PostDetails", post);
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


        private Task<IList<MenuItem>> DefineMainMenuRefs()
        {
            return Task.Factory.StartNew<IList<MenuItem>>(() => {
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
                        {
                            list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.SuperAdministrator)
                                                      || x.MinimalAccessibleRole.Equals(Roles.Administrator)
                                                      || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                            return list;
                        }


                        if (User.IsInRole(Roles.Administrator))
                        {
                            list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Administrator)
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
                        {
                            list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Judge)
                                                      || x.MinimalAccessibleRole.Equals(Roles.Member)
                                                      || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                            return list;
                        }

                        if (User.IsInRole(Roles.Competitor))
                        {
                            list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Competitor)
                                                     || x.MinimalAccessibleRole.Equals(Roles.Member)
                                                     || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                            return list;
                        }

                        if (User.IsInRole(Roles.Member))
                        {
                            list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Member)
                                                      || x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                            return list;
                        }
                    }
                    else list = list.Where(x => x.MinimalAccessibleRole.Equals(Roles.Guest)).ToList();
                    return list;
                }
            });
        }

        #endregion
    }
}