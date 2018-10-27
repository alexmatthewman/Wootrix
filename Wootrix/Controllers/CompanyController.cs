using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private ApplicationUser _user;
        private readonly UserManager<ApplicationUser> _userManager;
        private DatabaseAccessLayer _dla;
        private SignInManager<ApplicationUser> _signInManager;

        public CompanyController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _env = env;
            _rootpath =_env.WebRootPath;
            _userManager = userManager;
            _signInManager = signInManager;
            _dla = new DatabaseAccessLayer(_context);
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            return View(await _context.Company.ToListAsync());
        }


        // GET: Comany Home
        public async Task<IActionResult> Home(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyName == id);

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

            if (company == null)
            {
                return NotFound();
            }

            // Now for users we need to show them articles on the home page so get them in the ViewBag for display
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

            if (_signInManager.IsSignedIn(User))
            {
                ViewBag.Segments = _dla.GetSegmentsList(_user.companyID);
                ViewBag.Articles = _dla.GetArticlesList(_user.companyID);
                ViewBag.CommentUnderReviewCount = _dla.GetArticleReviewCommentCount(_user.companyID);
            }
            return View(company);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyViewModel model)
        {
            //Initialise a new company
            var myCompany = new Company();

            if (ModelState.IsValid)
            {
                //Set the simple fields
                myCompany.CompanyName = model.CompanyName;
                myCompany.CompanyTextMain = model.CompanyTextMain;
                myCompany.CompanyTextSecondary = model.CompanyTextSecondary;
                myCompany.CompanyBackgroundColor = model.CompanyBackgroundColor;
                myCompany.CompanyHeaderBackgroundColor = model.CompanyHeaderBackgroundColor;
                myCompany.CompanyHighlightColor = model.CompanyHighlightColor;
                myCompany.CompanyMainFontColor = model.CompanyMainFontColor;
                myCompany.CompanyHeaderFontColor = model.CompanyHeaderFontColor;
                myCompany.CompanyNumberOfUsers = model.CompanyNumberOfUsers;
                myCompany.CompanyNumberOfPushNotifications = model.CompanyNumberOfPushNotifications;

                IFormFile logo = model.CompanyLogoImage;
                if (logo != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + logo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyLogoImage = logo.FileName;
                }

                IFormFile background = model.CompanyBackgroundImage;
                if (background != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + background.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await background.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyBackgroundImage = background.FileName;
                }

                IFormFile focus = model.CompanyFocusImage;
                if (focus != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + focus.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await focus.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyFocusImage = focus.FileName;
                }

                _context.Add(myCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(myCompany);
        }


        // GET: Company/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Company/Create
        public IActionResult Create()
        {
            return View();
        }

      
        // GET: Company/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
          
            return View(company);
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanyViewModel model)
        {
            //Initialise a new company
            var myCompany = new Company();

            if (ModelState.IsValid)
            {
                myCompany.ID = id;
                //Set the simple fields
                myCompany.CompanyName = model.CompanyName;
                myCompany.CompanyTextMain = model.CompanyTextMain;
                myCompany.CompanyTextSecondary = model.CompanyTextSecondary;
                myCompany.CompanyBackgroundColor = model.CompanyBackgroundColor;
                myCompany.CompanyHeaderBackgroundColor = model.CompanyHeaderBackgroundColor;
                myCompany.CompanyHighlightColor = model.CompanyHighlightColor;
                myCompany.CompanyMainFontColor = model.CompanyMainFontColor;
                myCompany.CompanyHeaderFontColor = model.CompanyHeaderFontColor;
                myCompany.CompanyNumberOfUsers = model.CompanyNumberOfUsers;
                myCompany.CompanyNumberOfPushNotifications = model.CompanyNumberOfPushNotifications;

                IFormFile logo = model.CompanyLogoImage;
                if (logo != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + logo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await logo.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyLogoImage = logo.FileName;
                }

                IFormFile background = model.CompanyBackgroundImage;
                if (background != null)
                {
                    var s = _env.WebRootPath;
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + background.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await background.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyBackgroundImage = background.FileName;
                }

                IFormFile focus = model.CompanyFocusImage;
                if (focus != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", model.CompanyName + "_" + focus.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await focus.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyFocusImage = focus.FileName;
                }

              
                try
                {
                    _context.Update(myCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(myCompany);
        }



        // GET: Company/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.ID == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.ID == id);
        }
    }
}
