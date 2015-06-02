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
using ContestsPortal.WebSite.ViewModels.Account;

namespace ContestsPortal.WebSite.Controllers
{
    [AllowAnonymous]
    public class HomeController : AsyncController
    {
        private readonly ITaskProvider _archivedTaskProvider;
        private readonly IContestsProvider _contestsProvider;
        private readonly IPostProvider _postProvider;
        private readonly IUsersProvider _usersProvider;
        private readonly ICompetitorProvider _competitorProvider;


        #region Constructors

        public HomeController()
        {
        }

        public HomeController(IContestsProvider contestsProvider, 
            ITaskProvider archivedTaskProvider, 
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
            ContestEditorViewModel viewModel = new ContestEditorViewModel(contest);
            viewModel.TaskEditors = contest.Tasks.Select(x => new TaskEditorViewModel(x)).ToList();
            viewModel.Competitors = contest.Competitors.Select(x => new UserViewModel(x.UserProfile)).ToList();

            Debug.WriteLine("HomeController.ContestsDetails. id: " + contestId + ", " + contest.Tasks.Count + ", " + contest.TasksCount);
            Debug.WriteLine("Tasks: " + contest.Tasks.Count);

            return View("ContestsDetails", viewModel);
        }

        public async Task<ActionResult> ContestsHistory()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsAsync();
            IList<ContestEditorViewModel> contestsViewMode = contests.Select(x => new ContestEditorViewModel(x)).ToList();

            return View("ContestsHistory", contestsViewMode);
        }

        public async Task<ActionResult> ArchivedTaskDetails(int taskId)
        {
            ContestTask task = await _archivedTaskProvider.GetContestTaskAsync(taskId);
            TaskEditorViewModel viewModel = new TaskEditorViewModel(task);
            viewModel.Languages = task.Languages.Select(x => new ProgrammingLanguageViewModel(x)).ToList();

            ViewBag.IsUseLayout = true;

            Debug.WriteLine("HomeController.ArchivedTaskDetails. id: " + taskId);
            
            return View("../Administrator/_TaskEditor", viewModel);   
        }

        public async Task<ActionResult> ArchivedTasks()
        {
            IList<ContestTask> tasks = await _archivedTaskProvider.GetAllContestTasksAsync();
            
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

        private static Competitor CreateaCompetitor(string fullName, string nickName, int ranking, int totalGrade)
        {
            return new Competitor()
            {
                UserProfile = new UserProfile()
                {
                    FullName = fullName,
                    NickName = nickName
                },
                Ranking = ranking,
                TotalGrade = totalGrade
            };
        }

        public async Task<ActionResult> CompetitorsRating()
        {
            Random randomizer = new Random();

            IList<Competitor> competitors = await _competitorProvider.GetAllCompretitorAsync();

            int number = 0;
            competitors.Add(CreateaCompetitor("Константиновпольский Константин Константинович", "Костя Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Алексей", "Алекс Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Петр Константинович", "Петя Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Валера Константинович", "Валера Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Рома Константинович", "Рома Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Маша Константиновна", "Маша Цзю", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Евгений Александрович", "slesh", (number = randomizer.Next(1, 20)), 100 - 3 * number));
            competitors.Add(CreateaCompetitor("Виталий Александрович", "slesh123", (number = randomizer.Next(1, 20)), 100 - 3 * number));

            competitors = competitors.OrderBy(x => x.Ranking).ToList();

            return View(competitors); 
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