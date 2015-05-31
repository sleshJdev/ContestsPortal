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
using ContestsPortal.Domain.DataAccess;
using ContestsPortal.Domain.Models;
using ContestsPortal.WebSite.App_Start;
using ContestsPortal.WebSite.Infrastructure;
using ContestsPortal.WebSite.Infrastructure.ActionAttributes;
using ContestsPortal.WebSite.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;

namespace ContestsPortal.WebSite.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        #region Constructors

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            if (userManager == null) throw new ArgumentNullException("userManager");
            if (signInManager == null) throw new ArgumentNullException("signInManager");

            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        #region Properties

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            set { _signInManager = value; }
        }


        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        #endregion

        #region Action methods

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View("LoginView");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
                return View("LoginView", viewmodel);
            string returnurl;
            UserProfile user;
            if (!string.IsNullOrEmpty(viewmodel.UserName))
            {
                SignInStatus result = await SignInManager.PasswordSignInAsync(viewmodel.UserName, viewmodel.Password, viewmodel.RememberMe,false);
                switch (result)
                {
                    case SignInStatus.Success:
                    {
                        user = await UserManager.FindByNameAsync(viewmodel.UserName);
                        IList<string> roles = await UserManager.GetRolesAsync(user.Id);
                        returnurl = DefineReturnUrl(roles);
                        return RedirectToLocal(returnurl);
                    }
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("InvalidUserCredentials", ValidationResources.InvalidUserCredentials);
                        return View("LoginView", viewmodel);
                }
            }

            user = await UserManager.FindByEmailAsync(viewmodel.Email);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, viewmodel.RememberMe, false);
                IList<string> roles = await UserManager.GetRolesAsync(user.Id);
                returnurl = DefineReturnUrl(roles);
                return RedirectToLocal(returnurl);
            }

            ModelState.AddModelError("InvalidUserCredentials", ValidationResources.InvalidUserCredentials);
            return View("LoginView", viewmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            var list = AuthenticationManager.GetExternalAuthenticationTypes();
            int k = 0;
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl}));
        }

        [AllowAnonymous]
        public ActionResult ExternalCallbackRedirect(string returnUrl)
        {
            return RedirectPermanent("/Account/ExternalLoginCallback");
        }


        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            int k = 0;
            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            SignInStatus result = await SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new {ReturnUrl = returnUrl});
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation",
                        new ExternalLoginConfirmationViewModel {Email = loginInfo.Email});
            }
        }

        // ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
            string returnUrl)
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            ExternalLoginInfo info=null;
            if (ModelState.IsValid)
            {  
                // Get the information about the user from the external login provider
                info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var result = await ConstructNewUserLogin(info, model);
                if (result == IdentityResult.Success)
                {
                    AuthenticationManager.SignIn(new AuthenticationProperties(){IsPersistent = false},info.ExternalIdentity);
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            ViewBag.LoginProvider = info.Login.LoginProvider;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Session["Countries"] == null)
                Session["Countries"] = HttpContext.GetOwinContext().Get<PortalContext>().Countries.ToList();

            ViewBag.Countries = Session["Countries"];
            return View("RegisterView");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = Session["Countries"];
                return View("RegisterView", viewmodel);
            }

            IdentityResult res;
            if (!string.IsNullOrEmpty(viewmodel.Password))
            {
                res = await UserManager.PasswordValidator.ValidateAsync(viewmodel.Password);
                if (!res.Succeeded) AddPasswordRelatedErrors(res, UserManager.PasswordValidator as PasswordValidator);
            }

            string recapthcaresponse = Request.Form["g-recaptcha-response"];
            res = await VerificateRecaptcha(recapthcaresponse);

            if (!res.Succeeded)
            {
                ViewBag.Countries = Session["Countries"];
                AddErrors(res);
                return View("RegisterView", viewmodel);
            }

            var user = new UserProfile
            {
                UserName = viewmodel.UserName,
                Email = viewmodel.Email,
                NickName = viewmodel.NickName,
                FirstName = viewmodel.FirstName,
                LastName = viewmodel.LastName,
                MiddleName = viewmodel.MiddleName,
                AboutYourself = viewmodel.MiddleName,
                IdCountry = viewmodel.CountryId,
                HomePage = viewmodel.HomePage,
                BirthDate = viewmodel.BirthDate,
                PhoneNumber = viewmodel.PhoneNumber,
                RegistrationDate =  DateTime.Now
            };

            res = await UserManager.CreateAsync(user, viewmodel.Password);
            if (!res.Succeeded)
            {
                ViewBag.Countries = Session["Countries"];
                AddErrors(res);
                return View("RegisterView", viewmodel);
            }

            res = await UserManager.AddToRoleAsync(user.Id, Roles.Member);
            Session["Countries"] = null;
            TempData["User"] = user;
            return RedirectToAction("SuccessfulRegistration");
        }

        #endregion

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private string DefineReturnUrl(IList<string> roles)
        {
            if (roles == null) throw new ArgumentNullException();
            string returnUrl = Url.Action("Index", "Home");

            if (roles.Contains(Roles.SuperAdministrator) || roles.Contains(Roles.Administrator))
            {
                returnUrl = Url.Action("Index", "Administrator");
                return returnUrl;
            }

            if (roles.Contains(Roles.Moderator))
            {
                returnUrl = Url.Action("Index", "Moderator");
                return returnUrl;
            }

            if (roles.Contains(Roles.Judge))
            {
                returnUrl = Url.Action("Index", "Judge");
                return returnUrl;
            }

            if (roles.Contains(Roles.Competitor) || roles.Contains(Roles.Member))
            {
                returnUrl = Url.Action("Index", "Member");
                return returnUrl;
            }
            return returnUrl;
        }


        [NoCache]
        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult EntryPartial()
        {
            IPrincipal user = HttpContext.User;
            return PartialView("EntryView", HttpContext.User);
        }

        [AllowAnonymous]
        public ActionResult SuccessfulRegistration()
        {
            var model = TempData["User"] as UserProfile;
            if (model != null) return View("SuccessfulRegistrationView", model);
            return View("Errors/RegistrationAlreadyCompletedError");
        }

        #region Helpers

        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private void AddPasswordRelatedErrors(IdentityResult result, PasswordValidator validator)
        {
            int reqLength = validator.RequiredLength;
            foreach (string error in result.Errors)
            {
                if (error.Contains("Passwords must be at least"))
                    ModelState.AddModelError("Password",
                        string.Format(RussianErrorMessages.PasswordTooShort,
                            reqLength.ToString(CultureInfo.CurrentCulture)));
                else ModelState.AddModelError("Password", ErrorsLocalizer.MapToRussianError[error]);
            }
        }


        private Task<IdentityResult> VerificateRecaptcha(string responsefield)
        {
            return Task<IdentityResult>.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(responsefield))
                {
                    ModelState.AddModelError("Recaptcha", RussianErrorMessages.MissingInputResponse);
                    return IdentityResult.Failed(new[] {RussianErrorMessages.MissingInputResponse});
                }

                string url = String.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    ConfigurationManager.AppSettings["recaptchaprivatekey"], responsefield);

                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.ContentLength = 0;
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                string result;
                using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
                    result = reader.ReadToEnd();
                JToken tokenresult = JObject.Parse(result);
                var success = (bool) tokenresult.SelectToken("success");
                if (!success)
                {
                    List<string> errors = tokenresult.SelectToken("error-codes").ToObject<string[]>().ToList();
                    var rusErrors = new List<string>();
                    if (errors != null && errors.Count > 0)
                    {
                        foreach (string error in errors)
                        {
                            string rusEr = ErrorsLocalizer.MapToRussianError[error];
                            ModelState.AddModelError("Recaptcha", rusEr);
                            rusErrors.Add(rusEr);
                        }
                    }
                    return IdentityResult.Failed(rusErrors.ToArray());
                }
                return IdentityResult.Success;
            });
        }
        
        private async Task<IdentityResult> ConstructNewUserLogin(ExternalLoginInfo loginInfo, ExternalLoginConfirmationViewModel model)
        {
            if (loginInfo == null) throw new ArgumentNullException("loginInfo");
            if (model == null) throw new ArgumentNullException("model");
            IdentityResult result;
            int countryId = 0;
            var userProfile = await UserManager.FindByNameAsync(loginInfo.DefaultUserName);
            if (userProfile == null)
            {
                using (var context = new PortalContext())
                {
                    countryId = context.Countries.Single(x => x.CountryName == "Undefined").CountryId;
                }

                userProfile = new UserProfile
                {
                    UserName = loginInfo.DefaultUserName,
                    Email = model.Email,
                    NickName = model.NickName,
                    IdCountry = countryId,
                    RegistrationDate = DateTime.Now
                };
                var validator = (UserManager.PasswordValidator as PasswordValidator).ClonePasswordValidator();
                // disable password validation
                UserManager.PasswordValidator = PasswordValidatorExtensions.CreateNotValidatingPasswordValidator();
                result = await UserManager.CreateAsync(userProfile, DefaultAuthenticationTypes.ExternalCookie);
                if (!result.Succeeded) return result;
                // restore original password validator
                UserManager.PasswordValidator = validator;

                result = await UserManager.AddToRoleAsync(userProfile.Id, Roles.Member);
                if (!result.Succeeded) return result;
                
                result = await UserManager.AddLoginAsync(userProfile.Id, loginInfo.Login);
                return result;
            }

            result = await UserManager.AddLoginAsync(userProfile.Id, loginInfo.Login);
            return result;
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, int? userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public int? UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties {RedirectUri = RedirectUri};
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId.ToString();
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}