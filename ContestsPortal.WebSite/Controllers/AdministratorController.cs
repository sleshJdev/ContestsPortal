using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.DataAccess.Providers.Interfaces;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.Infrastructure.ActionAttributes;
using ContestsPortal.WebSite.ViewModels.Administrator;
using Microsoft.AspNet.Identity;

namespace ContestsPortal.WebSite.Controllers
{
    [Authorize(Roles = Roles.SuperAdministrator + "," + Roles.Administrator)]
    public class AdministratorController : Controller
    {
        private readonly IContestsProvider _contestsProvider;

        #region Constructors

        public AdministratorController()
        {
        }

        public AdministratorController(IContestsProvider provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            _contestsProvider = provider;
        }

        #endregion

        // GET: Administrator
        [HttpGet]
        public ActionResult Index()
        {
            return View("IndexView");
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ActiveContests()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsByStateAsync(ContestStates.Active);
            return View("_ActiveContests", contests);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> AwaitingContests()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsByStateAsync(ContestStates.Awaiting);
            return View("_AwaitingContests", contests);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ActiveContestTasks(int contestId)
        {
            if (!HttpContext.Request.IsAjaxRequest()) return null;
            return View();
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> DeleteContest(int contestId)
        {
            if (!HttpContext.Request.IsAjaxRequest()) return null;
            IdentityResult result = await _contestsProvider.DeleteContestAsync(contestId);
            return Json(new {Result = result.Succeeded}, JsonRequestBehavior.DenyGet);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> OpenForRegistrationContests()
        {
            IList<Contest> contests = await _contestsProvider.GetContestsByStateAsync(ContestStates.Registration);
            return View(contests);
        }

        [HttpPost, AjaxOnly]
        public async Task<JsonResult> PassToRegisrationStage(int contestId = 0)
        {
            IdentityResult result =
                await _contestsProvider.SetStateForContestAsync(contestId, ContestStates.Registration);
            return Json(new {Result = result.Succeeded}, JsonRequestBehavior.DenyGet);
        }


        [HttpGet,AjaxOnly]
        public async Task<ActionResult> AddNewContest()
        {
            var contest = new ContestEditorViewModel();
            IList<ContestPriority> list;
            // gonna use ninject later
            using (var context = new PortalContext())
                list = context.Set<ContestPriority>().OrderBy(x => x.ContestPriorityId).ToList();
            ViewBag.ContestPriorities = new SelectList(list, "ContestPriorityId", "ContestPriorityName");
            Session["ContestPriorityId"] = ViewBag.ContestPriorities;
            return View("_AddNewContest", contest);
        }

        [HttpPost, AjaxOnly]
        public async Task<ActionResult> ContestDetails(int contestId)
        {
            // no yet
            return null;
        }


        [HttpPost, AjaxOnly]
        public async Task<ActionResult> AddNewContest(ContestEditorViewModel viewmodel)
        {
            ViewData["ContestPriorityId"] = Session["ContestPriorityId"];
            Session["ContestPriorityId"] = null;
            if (!ModelState.IsValid) return View(viewmodel);
            
            TimeSpan? duration = TimeSpan.Zero;

            var tasks = new List<ContestTask>();

            foreach (TaskEditorViewModel editor in viewmodel.TaskEditors)
            {
                var task = new ContestTask
                {
                    TaskComplexity = editor.TaskComplexity,
                    TaskDuration = editor.TaskDuration,
                    TaskAward = editor.TaskAward,
                    TaskTitle = editor.TaskTitle,
                    TaskComment = editor.TaskComment,
                    TaskContent = editor.TaskContent
                };

                duration = duration.Value.Add(editor.TaskDuration.Value);

                // adding only via id should be checked
                task.Languages =
                    editor.Languages.Select(
                        x => new ProgrammingLanguage {LanguageId = x.LanguageId, LanguageName = x.LanguageName})
                        .ToList();
                tasks.Add(task);
            }

            DateTime contestEnd = viewmodel.ContestBeginning.Value.Add(duration.Value);

            var contest = new Contest
            {
                Tasks = tasks,
                ContestBeginning = viewmodel.ContestBeginning,
                ContestTitle = viewmodel.ContestTitle,
                ContestComment = viewmodel.ContestComment,
                TasksCount = viewmodel.TaskEditors.Count,
                ContestEnd = contestEnd,
                IdContestPriority = viewmodel.ContestPriorityId
            };

            IdentityResult result = await _contestsProvider.CreateContestAsync(contest);
            if (result.Succeeded) return Json(new {Succeeded = result.Succeeded}, JsonRequestBehavior.DenyGet);
            return View("_AddNewContest",viewmodel);
        }

        // this is used in addnewcontest tab
        [HttpGet]
        public async Task<ActionResult> OpenTaskEditor()
        {
            if (HttpContext.Session["Languages"] == null)
            {
                // use Resolver.GetService<> later
                using (var context = new PortalContext())
                    HttpContext.Session["Languages"] = context.Languages.ToList();
            }
            List<ProgrammingLanguageViewModel> langvms =
                (HttpContext.Session["Languages"] as IList<ProgrammingLanguage>)
                    .Select(x => new ProgrammingLanguageViewModel(x))
                    .ToList();

            var model = new TaskEditorViewModel {Languages = langvms};
            return View("_TaskEditor", model);
        }


        // task editing
        [HttpGet, AjaxOnly]
        public async Task<ActionResult> EditArchivedTask(int taskId)
        {
            return View();
        }

        [HttpPost, AjaxOnly]
        public async Task<ActionResult> EditArchivedTask(TaskEditorViewModel viewmodel)
        {
            return View();
        }
    }
}