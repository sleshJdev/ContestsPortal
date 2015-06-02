using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ContestsPortal.WebSite.App_Start;
using ContestsPortal.WebSite.Infrastructure;
using ContestsPortal.WebSite.Infrastructure.ActionAttributes;
using ContestsPortal.WebSite.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using ContestsPortal.WebSite.ViewModels.Administrator;
using System.Diagnostics;
using ContestsPortal.Domain.Models;
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.DataAccess.Providers.Interfaces;

namespace ContestsPortal.WebSite.Controllers
{
    [Authorize(Roles = Roles.SuperAdministrator + "," + Roles.Administrator)]
    public class AdministratorController : Controller
    {
        private readonly IContestsProvider _contestsProvider;
        private readonly IUsersProvider _usersProvider;
        private readonly IProgrammingLanguageProvider _programmingLanguageProvider;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        #region Constructors

        public AdministratorController()
        {
        }

        public AdministratorController(IContestsProvider provider, IUsersProvider usersProvider, IProgrammingLanguageProvider programmingLanguageProvider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            if (usersProvider == null) throw new ArgumentNullException("usersProvider");
            if (programmingLanguageProvider == null) throw new ArgumentNullException("programmingLanguageProvider");

            _contestsProvider = provider;
            _usersProvider = usersProvider;
            _programmingLanguageProvider = programmingLanguageProvider;
        }

        #endregion

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> AddLanguage()
        {
            Debug.WriteLine("AdministratorController.AddLanguage");
            
            return View("_ProgrammingLanguageEdit");
        }

        [HttpPost, AjaxOnly]
        public async Task<ActionResult> EditLanguage(ProgrammingLanguageViewModel viewmodel)
        {
            Debug.WriteLine("AdministratorController.EditLanguage(ProgrammingLanguageViewModel viewmodel)");

            ProgrammingLanguage newLanguage = new ProgrammingLanguage()
            {
                LanguageId = viewmodel.LanguageId,
                LanguageName = viewmodel.LanguageName
            };

            IdentityResult result = await _programmingLanguageProvider.AddProgrammingLanguageAsync(newLanguage);
            if (result.Succeeded) return Json(new { Succeeded = result.Succeeded }, JsonRequestBehavior.DenyGet);

            return RedirectToAction("ProgrammingLanguages");
        }

        [HttpGet]
        public async Task<ActionResult> DeleteLanguage(int languageId)
        {
            Debug.WriteLine("AdministratorController.EditLanguage(int languageId)");

            IdentityResult result = await _programmingLanguageProvider.DeleteContestAsync(languageId);
            if (result.Succeeded) return Json(new { Succeeded = result.Succeeded }, JsonRequestBehavior.DenyGet);

            return RedirectToAction("ProgrammingLanguages");
        }

        [HttpGet]
        public async Task<ActionResult> EditLanguage(int languageId)
        {
            Debug.WriteLine("AdministratorController.EditLanguage(int languageId)");

            ProgrammingLanguage language = await _programmingLanguageProvider.GetProgrammingLanguageAsync(languageId);
            ProgrammingLanguageViewModel languageViewModel = new ProgrammingLanguageViewModel(language);

            return View("_ProgrammingLanguageEdit", languageViewModel);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> ProgrammingLanguages()
        {
            IList<ProgrammingLanguage> languages = await _programmingLanguageProvider.GetAllProgrammingLanguagesAsync();
            IList<ProgrammingLanguageViewModel> languagesViewModel = languages.Select(x => new ProgrammingLanguageViewModel(x)).ToList();

            return View("_ProgrammingLanguages", languagesViewModel);
        }

        private UserViewModel ConvertUserProfileToUserViewModel(UserProfile user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                NickName = user.NickName 
            };
        } 

        [HttpPost, AjaxOnly]
        public async Task<ActionResult> EditUser(UserViewModel viewmodel)
        {
            var user = await _usersProvider.GetUser(Convert.ToInt32(viewmodel.Id));
            
            user.NickName = viewmodel.NickName;
            user.PasswordHash = viewmodel.Password;

            IdentityResult res = await UserManager.UpdateAsync(user);
            if (!res.Succeeded)
            {
                return View("_UserEdit", viewmodel);
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<ActionResult> EditUser(int userId)
        {
            UserProfile user = await _usersProvider.GetUser(userId);
            UserViewModel userViewModel = ConvertUserProfileToUserViewModel(user);

            return View("_UserEdit", userViewModel);
        }

        [HttpGet, AjaxOnly]
        public async Task<ActionResult> Users()
        {
            if (!HttpContext.Request.IsAjaxRequest()) return null;

            IList<UserProfile> users = (await _usersProvider.GetAllUsers());
            IList<UserViewModel> usersViewModel = new List<UserViewModel>(users.Count);
            foreach (UserProfile user in users)
            {
                UserViewModel userViewModel = ConvertUserProfileToUserViewModel(user);
                usersViewModel.Add(userViewModel);
            }

            return View("_Users", usersViewModel);
        }       

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
            return Json(new { Result = result.Succeeded }, JsonRequestBehavior.DenyGet);
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
            return Json(new { Result = result.Succeeded }, JsonRequestBehavior.DenyGet);
        }


        [HttpGet, AjaxOnly]
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
        public async Task<ActionResult> ContesEdit(int contestId)
        {
            Contest contest = await _contestsProvider.GetContest(contestId);
            
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
                        x => new ProgrammingLanguage { LanguageId = x.LanguageId, LanguageName = x.LanguageName })
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
            if (result.Succeeded) return Json(new { Succeeded = result.Succeeded }, JsonRequestBehavior.DenyGet);
            return View("_AddNewContest", viewmodel);
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

            var model = new TaskEditorViewModel { Languages = langvms };
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