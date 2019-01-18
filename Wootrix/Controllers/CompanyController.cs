using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private WootrixV2.Models.User _user;
        private WootrixV2.Models.Company _cpy;
        private readonly UserManager<ApplicationUser> _userManager;
        private DataAccessLayer _dla;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<CompanyController> _companyLocalizer;

        public CompanyController(IStringLocalizer<CompanyController> companyLocalizer, UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _env = env;
            _rootpath = _env.WebRootPath;
            _userManager = userManager;
            _signInManager = signInManager;
            _dla = new DataAccessLayer(_context);
            _companyLocalizer = companyLocalizer;
                                       
        }

        // GET: Company
        public async Task<IActionResult> Index()
        {
            
            return View(await _context.Company.ToListAsync());
        }

        // GET: Comany Home
        [AllowAnonymous]
        public async Task<IActionResult> Home(string id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";


            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.CompanyName == id);    

            if (company == null)
            {
                return NotFound();
            }

            // Now for users we need to show them articles on the home page so get them in the ViewBag for display
            

            if (_signInManager.IsSignedIn(User))
            {
                _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
                _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
                HttpContext.Session.SetInt32("CompanyID", _cpy.ID);
                HttpContext.Session.SetString("CompanyName", _cpy.CompanyName);
                HttpContext.Session.SetString("CompanyTextMain", _cpy.CompanyTextMain);
                HttpContext.Session.SetString("CompanyTextSecondary", _cpy.CompanyTextSecondary);
                HttpContext.Session.SetString("CompanyMainFontColor", _cpy.CompanyMainFontColor);
                HttpContext.Session.SetString("CompanyLogoImage", _cpy.CompanyLogoImage);
                HttpContext.Session.SetString("CompanyFocusImage", _cpy.CompanyFocusImage ?? "");
                HttpContext.Session.SetString("CompanyBackgroundImage", _cpy.CompanyBackgroundImage ?? "");
                HttpContext.Session.SetString("CompanyHighlightColor", _cpy.CompanyHighlightColor);
                HttpContext.Session.SetString("CompanyHeaderFontColor", _cpy.CompanyHeaderFontColor);
                HttpContext.Session.SetString("CompanyHeaderBackgroundColor", _cpy.CompanyHeaderBackgroundColor);
                HttpContext.Session.SetString("CompanyBackgroundColor", _cpy.CompanyBackgroundColor);
                HttpContext.Session.SetInt32("CompanyNumberOfUsers", _cpy.CompanyNumberOfUsers);

                
                ViewBag.User = _user;
                ViewBag.CommentUnderReviewCount = _dla.GetArticleReviewCommentCount(_user.CompanyID);
                if (_user.Role == Roles.User)
                {
                    System.Console.WriteLine("***********Getting Segments************");
                    ViewBag.Segments = _dla.GetSegmentsList(_user.CompanyID, _user, "", "");   
                }
            }
            // Saving all this company stuff to the session so the layout isn't dependent on the model
            // Note that it is all non-sensitive stuff            
           
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
                    await _dla.UploadFileToS3(logo, model.CompanyName + "_" + logo.FileName, "images/Uploads");
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyLogoImage = logo.FileName;
                }

                IFormFile background = model.CompanyBackgroundImage;
                if (background != null)
                {
                    await _dla.UploadFileToS3(background, model.CompanyName + "_" + background.FileName, "images/Uploads");
                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyBackgroundImage = background.FileName;
                }

                IFormFile focus = model.CompanyFocusImage;
                if (focus != null)
                {
                    await _dla.UploadFileToS3(focus, model.CompanyName + "_" + focus.FileName, "images/Uploads");
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
                    await _dla.UploadFileToS3(logo, model.CompanyName + "_" + logo.FileName, "images/Uploads");

                    //The file has been saved to AWS - now save the file name to the DB
                    myCompany.CompanyLogoImage = logo.FileName;
                }

                IFormFile background = model.CompanyBackgroundImage;
                if (background != null)
                {
                    await _dla.UploadFileToS3(background, model.CompanyName + "_" + background.FileName, "images/Uploads");

                    //The file has been saved to disk - now save the file name to the DB
                    myCompany.CompanyBackgroundImage = background.FileName;
                }

                IFormFile focus = model.CompanyFocusImage;
                if (focus != null)
                {
                    await _dla.UploadFileToS3(focus, model.CompanyName + "_" + focus.FileName, "images/Uploads");
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
