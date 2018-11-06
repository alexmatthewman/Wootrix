using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wootrix.Data;
using Wootrix.Models;
using WootrixV2.Data;


namespace Wootrix.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;

        }

        // Multi-language cookie update script
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                var claim = "";
                if (User.Claims.Count() > 0)
                {
                    var cl = User.Claims.FirstOrDefault(m => m.Value == "Admin" || m.Value == "CompanyAdmin");
                    if (cl != null) { claim = cl.Value; } else
                    {
                        //This is a user
                        var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                        return RedirectToAction("Home", "Company", new { id = user.companyName });
                    }                   
                }

                if (claim == "Admin")
                {
                    //We have a super user

                    var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                    return RedirectToAction("Home", "Company", new { id = user.companyName });
                }
                if (claim == "CompanyAdmin")
                {
                    //Company admin so lets redirect to company home

                    //And lets find the company for the user and redirect to that id

                    var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                    return RedirectToAction("Home", "Company", new { id = user.companyName });
                }
                if (claim == "")
                {
                    //User - they also go to company home
                    var user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                    return RedirectToAction("Home", "Company", new { id = user.companyName });
                }
            }
          
            //If not logged in at all set the session to show the wootrix company so the login pages aren't messed up
         
            
            var company = _context.Company
                .FirstOrDefaultAsync(m => m.CompanyName == "Wootrix").GetAwaiter().GetResult();

            // Saving all this company stuff to the session so the layout isn't dependent on the model
            // Note that it is all non-sensitive stuff
            //byte[] asdf = new byte[8];
            HttpContext.Session.SetInt32("CompanyID", company.ID);
            HttpContext.Session.SetString("CompanyName", company.CompanyName);
            HttpContext.Session.SetString("CompanyTextMain", company.CompanyTextMain);
            HttpContext.Session.SetString("CompanyTextSecondary", company.CompanyTextSecondary);
            HttpContext.Session.SetString("CompanyMainFontColor", company.CompanyMainFontColor);
            HttpContext.Session.SetString("CompanyLogoImage", company.CompanyLogoImage);
            HttpContext.Session.SetString("CompanyFocusImage", company.CompanyFocusImage ?? "");
            HttpContext.Session.SetString("CompanyBackgroundImage", company.CompanyBackgroundImage ?? "");
            HttpContext.Session.SetString("CompanyHighlightColor", company.CompanyHighlightColor);
            HttpContext.Session.SetString("CompanyHeaderFontColor", company.CompanyHeaderFontColor);
            HttpContext.Session.SetString("CompanyHeaderBackgroundColor", company.CompanyHeaderBackgroundColor);
            HttpContext.Session.SetString("CompanyBackgroundColor", company.CompanyBackgroundColor);
            HttpContext.Session.SetInt32("CompanyNumberOfUsers", company.CompanyNumberOfUsers);

            return View();
        }

       




            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
