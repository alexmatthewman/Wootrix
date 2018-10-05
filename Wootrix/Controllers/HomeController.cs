using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wootrix.Models;
using WootrixV2.Data;

namespace Wootrix.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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

                    }                   
                }

                if (claim == "Admin")
                {
                    //We have a super user
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
