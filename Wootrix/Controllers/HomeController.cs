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
                    claim = User.Claims.FirstOrDefault(m => m.Value == "Admin" || m.Value == "CompanyAdmin").Value;
                }

                if (claim == "Admin")
                {
                    //We have a super user
                }
                if (claim == "CompanyAdmin")
                {
                    //Company admin so lets redirect to company admin home
                    
                    //And lets find the company for the user and redirect to that id
                    var user =  _userManager.FindByEmailAsync("wootrixCompanyAdmin@wootrix.com").GetAwaiter().GetResult();
                  
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
