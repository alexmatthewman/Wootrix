using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class CompanySegmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;
        private int _companyID;

        public CompanySegmentsController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context)
        {
            _context = context;
            _env = env;
            _rootpath = _env.WebRootPath;
           _userManager = userManager;
            
        }

        // GET: CompanySegments
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned
            _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var ctx = _context.CompanySegment.Where(m => m.CompanyID == _companyID);
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            return View(await ctx.ToListAsync());
        }

        // GET: CompanySegments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companySegment == null)
            {
                return NotFound();
            }

            return View(companySegment);
        }

        // GET: CompanySegments/Create
        public IActionResult Create()
        {
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            CompanySegmentViewModel s = new CompanySegmentViewModel();
            s.Order = 1;
            s.PublishDate = DateTime.Now.Date;
            s.FinishDate = DateTime.Now.AddYears(10).Date;
            s.StandardColor = HttpContext.Session.GetString("CompanyHeaderBackgroundColor");
            s.ThemeColor = HttpContext.Session.GetString("CompanyHighlightColor");
            s.ClientName = _user.name;
            //s.ClientLogoImage = _user.photoUrl;
            var cp = _user.companyID;
            
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context, cp);               
            s.Departments = dla.GetDepartments();
            return View(s);
        }

        // POST: CompanySegments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanySegmentViewModel cps)
        {
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            //Initialise a new companysegment
            var mySegment = new CompanySegment();
            if (ModelState.IsValid)
            {
                //ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags
                mySegment.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                mySegment.Order = cps.Order ?? 1;
                mySegment.Title = cps.Title;
                mySegment.PublishDate = cps.PublishDate;
                mySegment.FinishDate = cps.FinishDate;
                mySegment.ClientName = cps.ClientName;
                mySegment.ThemeColor = cps.ThemeColor;
                mySegment.StandardColor = cps.StandardColor;
                mySegment.Draft = DateTime.Now > cps.PublishDate ? false : true;
                mySegment.Department = cps.Department;

                IFormFile coverImage = cps.CoverImage;
                if (coverImage != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", _user.companyName + "_" + coverImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    mySegment.CoverImage = coverImage.FileName;
                }

                IFormFile coverImageMB = cps.CoverImageMobileFriendly;
                if (coverImageMB != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", _user.companyName + "_" + coverImageMB.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImageMB.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    mySegment.CoverImageMobileFriendly = coverImageMB.FileName;
                }

                IFormFile cli = cps.ClientLogoImage;
                if (cli != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", _user.companyName + "_" + cli.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cli.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    mySegment.CoverImageMobileFriendly = cli.FileName;
                }

                _context.Add(mySegment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mySegment);
        }

        // GET: CompanySegments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment.FindAsync(id);
            if (companySegment == null)
            {
                return NotFound();
            }
            return View(companySegment);
        }

        // POST: CompanySegments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags")] CompanySegment companySegment)
        {
            if (id != companySegment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    companySegment.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                    _context.Update(companySegment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanySegmentExists(companySegment.ID))
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
            return View(companySegment);
        }

        // GET: CompanySegments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companySegment = await _context.CompanySegment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (companySegment == null)
            {
                return NotFound();
            }

            return View(companySegment);
        }

        // POST: CompanySegments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companySegment = await _context.CompanySegment.FindAsync(id);
            _context.CompanySegment.Remove(companySegment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanySegmentExists(int id)
        {
            return _context.CompanySegment.Any(e => e.ID == id);
        }
    }
}
