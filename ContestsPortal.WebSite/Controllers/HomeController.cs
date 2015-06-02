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
using Microsoft.AspNet.Identity;
using ContestsPortal.WebSite.ViewModels.Administrator;

namespace ContestsPortal.WebSite.Controllers
{
    [AllowAnonymous]
    public class HomeController : AsyncController
    {
        private readonly IArchivedTaskProvider _archivedTaskProvider;
        private readonly IContestsProvider _contestsProvider;
        private readonly IPostProvider _postProvider;
        private readonly IUsersProvider _usersProvider;
        private readonly ICompetitorProvider _competitorProvider;


        #region Constructors

        public HomeController()
        {
        }

        public HomeController(IContestsProvider contestsProvider, 
            IArchivedTaskProvider archivedTaskProvider, 
            IPostProvider postProvider, 
            IUsersProvider usersProvider, 
            ICompetitorProvider competitorProvider)
        {
            if (contestsProvider == null) throw new ArgumentNullException("contestsProvider");
            if (archivedTaskProvider == null) throw new ArgumentNullException("archivedTaskProvider");
            if (postProvider == null) throw new ArgumentNullException("postProvider");
            if (usersProvider == null) throw new ArgumentNullException("usersProvider");
            if (competitorProvider == null) throw new ArgumentNullException("competitorProvider");
            
            _contestsProvider = contestsProvider;
            _archivedTaskProvider = archivedTaskProvider;
            _postProvider = postProvider;
            _usersProvider = usersProvider;
            _competitorProvider = competitorProvider;
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

        //[Authorize]
        [HttpGet]
        [HttpPost, AjaxOnly]
        public async Task<ActionResult> TakePart(int contestId)
        {
            UserProfile user = await _usersProvider.GetByLogin(HttpContext.User.Identity.Name);
            Contest contest = await _contestsProvider.GetContest(contestId);

            Competitor competitor = new Competitor()
            {
                Contest = contest,
                IdContest = contest.ContestId,
                UserProfile = user,
                IdProfile = user.Id
            };

            IdentityResult result = await _competitorProvider.AddCompretitor(competitor);
            if (result.Succeeded) return Json(new { Succeeded = result.Succeeded }, JsonRequestBehavior.DenyGet);

            return RedirectToAction("ContestsHistory");
        }

        public async Task<ActionResult> ContestsDetails(int contestId)
        {
            Contest contest = await _contestsProvider.GetContest(contestId);
            
            Debug.WriteLine("HomeController.ContestsDetails. id: " + contestId + ", " + contest.Tasks.Count + ", " + contest.TasksCount);
            Debug.WriteLine("Tasks: " + contest.Tasks.Count);

            return View("ContestsDetails", contest);
        }

        public async Task<ActionResult> ContestsHistory()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsAsync();
            IList<ContestEditorViewModel> contestsViewMode = contests.Select(x => new ContestEditorViewModel(x)).ToList();

            return View("ContestsHistory", contestsViewMode);
        }

        public async Task<ActionResult> ArchivedTaskDetails(int taskId)
        {
            ArchivedTask task = await _archivedTaskProvider.GetArchivedTaskAsync(taskId);
            Debug.WriteLine("HomeController.ArchivedTaskDetails. id: " + taskId);
            return View("ArchivedTaskDetails", new ArchivedTask() { TaskTitle = "ArchivedTask1", TaskContent = "ArchivedTask1 content", TaskComplexity = 5 });   
        }

        public async Task<ActionResult> ArchivedTasks()
        {
            IList<ArchivedTask> tasks = await _archivedTaskProvider.GetAllArchivedTasksAsync();
           
            //stub
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
        public ActionResult MainMenu()
        {
            var list = DefineMainMenuRefs();

            return View("MenuView", list);
        }
                
        public ActionResult Posts()
        {
            IList<Post> posts = _postProvider.GetAllPosts();

            return View("PostsView", posts);
        }

        public ActionResult PostDetails(int postId)
        {
            Post post =  _postProvider.GetPost(postId);

            return View("PostDetails", post);
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
        }

        #endregion
    }
}