using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Base controller for all Admin area
/// </summary>
[Authorize(Roles = "Admin")]
public abstract class AdminController : Controller { }



namespace WootrixV2.Controllers
{
    
    public class MagazineController : Controller
    {
        // GET: Magazine
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

      
    }
}