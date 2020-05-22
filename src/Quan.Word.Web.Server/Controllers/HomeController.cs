using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Quan.Word.Web.Server
{
    /// <summary>
    /// Manages the standard web server pagess
    /// </summary>
    public class HomeController : Controller
    {
        #region Protected Members

        /// <summary>
        /// The scoped Application context
        /// </summary>
        protected ApplicationDbContext mContext;


        /// <summary>
        /// The manager for handing user creation, deletion, searching, roles etc...
        /// </summary>
        protected UserManager<ApplicationUser> mUserManager;


        /// <summary>
        /// The manager for handing signing in and out for our users
        /// </summary>
        protected SignInManager<ApplicationUser> mSignInManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The injected context</param>
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            mContext = context;
            mUserManager = userManager;
            mSignInManager = signInManager;
        }

        #endregion

        /// <summary>
        /// Basic welcome page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Make sure we have the database
            mContext.Database.EnsureCreated();

            if (!mContext.Settings.Any())
            {
                mContext.Settings.Add(new SettingsDataModel()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = "BackgroundColor",
                    Value = "Red"
                });

                var settingsLocally = mContext.Settings.Local.Count();
                var settingsDatabase = mContext.Settings.Count();

                var firstSettings = mContext.Settings.Local.FirstOrDefault();
                var firstDatabase = mContext.Settings.FirstOrDefault();

                mContext.SaveChanges();

                settingsLocally = mContext.Settings.Local.Count();
                settingsDatabase = mContext.Settings.Count();

                firstSettings = mContext.Settings.Local.FirstOrDefault();
                firstDatabase = mContext.Settings.FirstOrDefault();

            }
            return View();
        }

        /// <summary>
        /// Creates our single user for now
        /// </summary>
        /// <returns></returns>
        [Route("create")]
        public async Task<IActionResult> CreateUserAsync()
        {
            var result = await mUserManager.CreateAsync(new ApplicationUser()
            {
                UserName = "quanljh",
                Email = "quanljh@gmail.com"
            }, "password");

            if (result.Succeeded)
                return Content("User was created", "text/html");

            return Content("User creation failed", "text/html");
        }


        /// <summary>
        /// Private area. No peeking
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("private")]
        public IActionResult Private()
        {
            return Content($"This is a private area. Welcome {HttpContext.User.Identity.Name}", "text/html");
        }


        /// <summary>
        /// Log the user out
        /// </summary>
        /// <returns></returns>
        [Route("logout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return Content("done");
        }

        /// <summary>
        /// An auto-login page for testing
        /// </summary>
        /// <param name="returnUrl">The url to return to if successfully logged in</param>
        /// <returns></returns>
        [Route("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            // Sign out any previous sessions
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // Sign user in with the valid credentials
            var result = await mSignInManager.PasswordSignInAsync("quanljh", "password", true, false);

            // If successful...
            if (result.Succeeded)
            {
                // If we have no return URL...
                if (string.IsNullOrEmpty(returnUrl))
                    // Go to home
                    return RedirectToAction(nameof(Index));

                // Otherwise, go to the return url
                return Redirect(returnUrl);
            }

            return Content("Failed to login", "text/html");
        }


        [Route("test")]
        public SettingsDataModel Test([FromBody] SettingsDataModel model)
        {
            return new SettingsDataModel()
            {
                Id = "some id",
                Name = "Luke",
                Value = "10"
            };
        }
    }
}
