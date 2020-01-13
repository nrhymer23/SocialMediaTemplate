using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using KSUCorner.Models;

namespace KSUCorner.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            SetDropdownViewData();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            SetDropdownViewData();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    UserRepository.CreateUser(model.UserName, model.FirstName,
                          model.LastName, model.Email,
                          model.Password, model.Gender,
                          model.Month, model.Day, model.Year,
                          model.SecurityQuestion, model.SecurityAnswer);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public void SetDropdownViewData()
        {
            SelectListItem[] months = {
                       new SelectListItem() { Selected = true, Text = "January", Value = "01" },
                       new SelectListItem() { Text = "February", Value = "02" },
                       new SelectListItem() { Text = "March", Value = "03" },
                       new SelectListItem() { Text = "April", Value = "04" },
                       new SelectListItem() { Text = "May", Value = "05" },
                       new SelectListItem() { Text = "June", Value = "06" },
                       new SelectListItem() { Text = "July", Value = "07" },
                       new SelectListItem() { Text = "August", Value = "08" },
                       new SelectListItem() { Text = "September", Value = "09" },
                       new SelectListItem() { Text = "October", Value = "10" },
                       new SelectListItem() { Text = "November", Value = "11" },
                       new SelectListItem() { Text = "December", Value = "12" } };
            ViewBag.Month = months;

            String[] days = { "1", "2", "3", "4", "5", "6", "7", "8",
                              "9", "10", "11", "12", "13", "14", "15", "16",
                              "17", "18", "19", "20", "21", "22", "23", "24",
                              "25", "26", "27", "28", "29", "30", "31" };
            SelectList dayItems = new SelectList(days, "1");
            ViewBag.DayListItems = dayItems;

            String[] years = { "1910", "1911", "1912", "1913", "1914",
                               "1915", "1916", "1917", "1918", "1919",
                               "1920", "1921", "1922", "1923", "1924",
                               "1925", "1926", "1927", "1928", "1929",
                               "1930", "1931", "1932", "1933", "1934",
                               "1935", "1936", "1937", "1938", "1939",
                               "1940", "1941", "1942", "1943", "1944",
                               "1945", "1946", "1947", "1948", "1949",
                               "1950", "1951", "1952", "1953", "1954",
                               "1955", "1956", "1957", "1958", "1959",
                               "1960", "1961", "1962", "1963", "1964",
                               "1965", "1966", "1967", "1968", "1969",
                               "1970", "1971", "1972", "1973", "1974",
                               "1975", "1976", "1977", "1978", "1979",
                               "1980", "1981", "1982", "1983", "1984",
                               "1985", "1986", "1987", "1988", "1989",
                               "1990", "1991", "1992", "1993", "1994",
                               "1995", "1996", "1997", "1998", "1999",
                               "2000", "2001", "2002", "2003", "2004",
                               "2005", "2006", "2007", "2008", "2009", "2010" };
            SelectList yearItems = new SelectList(years, "1990");
            ViewBag.YearListItems = yearItems;

            String[] questions = {
                          "Where did you meet your significant other?",
                          "In what city or town was your first job?",
                          "Who was your favorite childhood friend?",
                          "Who is your favorite cartoon character?",
                          "Who is your favorite author?" };
            SelectList questionItems = new SelectList(questions);
            ViewBag.SecurityQuestion = questionItems;
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }

    public class KSUCornerAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    string username = httpContext.User.Identity.GetUserName();
                    int acctID = -1;
                    using (KSUCornerDBEntities1 db1 = new KSUCornerDBEntities1())
                    {
                        var result = from u in db1.Accounts
                                     where (u.UserName.ToLower() == username.ToLower())
                                     select u;
                        if (result.Count() == 0)
                        {
                            return false;
                        }
                        Account account = result.FirstOrDefault();
                        if (!account.IsActivated)
                        {
                            return false;
                        }
                        if (account.IsLockedOut)
                        {
                            if (account.LastLockedOutDate < DateTime.Now)
                            {
                                account.IsLockedOut = false;
                                db1.SaveChanges();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        acctID = account.AccountID;
                    }
                    using (KSUCornerDBEntities12 db12 = new KSUCornerDBEntities12())
                    {
                        var restrictions = from r1 in db12.RestrictedAccounts
                                           where (r1.AccountID == acctID)
                                           select r1;
                        if (restrictions.Count() > 0)
                        {
                            RestrictedAccount restrict = restrictions.FirstOrDefault();
                            int restrictionValue = restrict.AccessMask;
                            if (restrictionValue > 0)
                            {
                                int index = GetViewIndex(GetView());
                                int mask = (int)Math.Pow(2, index);
                                if ((mask & restrictionValue) != 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetView()
        {
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            if (path == "/" || path == "" || path.ToLower() == "/home/" || path.ToLower() == "/home")
                path = "Index";
            else if (path.Length > 6 &&
                                path.Substring(0, 6).ToLower() == "/home/")
            {
                path = path.Substring(6);
                int n = path.IndexOf('/');
                if (n > -1)
                    path = path.Substring(0, n);
                n = path.IndexOf('?');
                if (n > -1)
                    path = path.Substring(0, n);
            }
            return path;
        }

        public int GetViewIndex(string viewName)
        {
            viewName = viewName.ToLower();
            for (int i = 0; i < HomeController.tabViews.Length; ++i)
                if (viewName == HomeController.tabViews[i].ToLower())
                    return i;

            string viewmatch = "";
            switch (viewName)
            {
                case "acceptinvitation": viewmatch = "Friends"; break;
                case "acceptmembershiprequest": viewmatch = "Groups"; break;
                case "activateaccount": viewmatch = "Admin"; break;
                case "admindeletemessage": viewmatch = "Admin"; break;
                case "adminmessage": viewmatch = "Admin"; break;
                case "adminmessaging": viewmatch = "Admin"; break;
                case "adminopenmessage": viewmatch = "Admin"; break;
                case "adminreplymessage": viewmatch = "Admin"; break;
                case "alreadyfriends": viewmatch = "Friends"; break;
                case "deactivateaccount": viewmatch = "Admin"; break;
                case "deleteblog": viewmatch = "Blogging"; break;
                case "deleteforum": viewmatch = "Forums"; break;
                case "deleteforumcategory": viewmatch = "Forums"; break;
                case "deletegroup": viewmatch = "Groups"; break;
                case "deletemediafile": viewmatch = "MediaGalleries"; break;
                case "deletemessage": viewmatch = "Messaging"; break;
                case "deletereply": viewmatch = "Messaging"; break;
                case "editblog": viewmatch = "Blogging"; break;
                case "editforum": viewmatch = "Forums"; break;
                case "editforumcategory": viewmatch = "Forums"; break;
                case "editgallery": viewmatch = "MediaGalleries"; break;
                case "editgroup": viewmatch = "Groups"; break;
                case "editmediafile": viewmatch = "MediaGalleries"; break;
                case "editprofile": viewmatch = "Profile"; break;
                case "editreply": viewmatch = "Messaging"; break;
                case "filedetails": viewmatch = "MediaGalleries"; break;
                case "groupforum": viewmatch = "Groups"; break;
                case "groupgallery": viewmatch = "Groups"; break;
                case "groupmission": viewmatch = "Groups"; break;
                case "invitationsent": viewmatch = "Friends"; break;
                case "invitefriend": viewmatch = "Friends"; break;
                case "lockoutaccount": viewmatch = "Admin"; break;
                case "membershiprequestsent": viewmatch = "Groups"; break;
                case "newblog": viewmatch = "Blogging"; break;
                case "newforum": viewmatch = "Forums"; break;
                case "newforumcategory": viewmatch = "Forums"; break;
                case "newgallery": viewmatch = "MediaGalleries"; break;
                case "newgroup": viewmatch = "Groups"; break;
                case "newmediafile": viewmatch = "MediaGalleries"; break;
                case "newmessage": viewmatch = "Messaging"; break;
                case "newreply": viewmatch = "Messaging"; break;
                case "notoself": viewmatch = "Friends"; break;
                case "openforum": viewmatch = "Forums"; break;
                case "openforumcategory": viewmatch = "Forums"; break;
                case "opengallery": viewmatch = "MediaGalleries"; break;
                case "openmessage": viewmatch = "Messaging"; break;
                case "openprivateblog": viewmatch = "Blogging"; break;
                case "openpublishedblog": viewmatch = "Blogging"; break;
                case "privateblogs": viewmatch = "Blogging"; break;
                case "publishedblogs": viewmatch = "Blogging"; break;
                case "removefriend": viewmatch = "Friends"; break;
                case "replymessage": viewmatch = "Messaging"; break;
                case "restrictaccount": viewmatch = "Admin"; break;
                case "search": viewmatch = "Messaging"; break;
                case "unlockoutaccount": viewmatch = "Admin"; break;
            }

            viewmatch = viewmatch.ToLower();
            for (int i = 0; i < HomeController.tabViews.Length; ++i)
                if (viewmatch == HomeController.tabViews[i].ToLower())
                    return i;

            return -1;
        }
    }

}