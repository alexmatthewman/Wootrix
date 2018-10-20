using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    public class SegmentArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;

        public SegmentArticlesController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context)
        {
            _context = context;
            _env = env;
            _rootpath = _env.WebRootPath;
            _userManager = userManager;
        }

        // GET: SegmentArticles
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned
            var id = HttpContext.Session.GetInt32("CompanyID");
            var ctx = _context.SegmentArticle.Where(m => m.CompanyID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: SegmentArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(segmentArticle);
        }
        


        // GET: SegmentArticles/Article/5
        public async Task<IActionResult> Article(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(segmentArticle);
        }

        // GET: SegmentArticles/Create
        public IActionResult Create()
        {

            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            SegmentArticleViewModel s = new SegmentArticleViewModel();
            s.Order = 1;
            s.PublishFrom = DateTime.Now.Date;
            s.PublishTill = DateTime.Now.AddYears(10).Date;
            s.CompanyID = _user.companyID;
            s.Author = _user.name;
            s.AllowComments = true;
            //s.ClientLogoImage = _user.photoUrl;
            //var cp = _user.companyID;

            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
           //s.SegmentList = dla.GetArticleSegments(_user.companyID).Select(x => new SelectListItem { Text = x.Value, Value = x.Value }).ToList();



            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {
                s.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }


            return View(s);
        }





        // POST: SegmentArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SegmentArticleViewModel sa)
        {
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var myArticle = new SegmentArticle();
            if (ModelState.IsValid)
            {
                //ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags
                myArticle.CompanyID = _user.companyID;
                myArticle.Order = sa.Order ?? 1;
                myArticle.Title = sa.Title;
                myArticle.PublishFrom = sa.PublishFrom;
                myArticle.PublishTill = sa.PublishTill;
                myArticle.AllowComments = sa.AllowComments;
                myArticle.ArticleContent = sa.ArticleContent;
                myArticle.Author = (sa.Author == null ? _user.name : sa.Author); //if null set to be user name
                myArticle.Tags = sa.Tags;
                myArticle.Segments = string.Join(",", sa.SelectedSegments);
                
                IFormFile img = sa.Image;
                if (img != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads/Articles", _user.companyName + "_" + WebUtility.HtmlEncode(myArticle.Title) + "_" + img.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myArticle.Image = img.FileName;
                }

                IFormFile vid = sa.EmbeddedVideo;
                if (vid != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads/Articles", _user.companyName + "_" + WebUtility.HtmlEncode(myArticle.Title) + "_" + vid.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await vid.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myArticle.EmbeddedVideo = vid.FileName;
                }

                _context.Add(myArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            // s.SegmentList = dla.GetArticleSegments(_user.companyID).Select(x => new SelectListItem { Text = x.Value, Value = x.Value }).ToList();



            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {
                sa.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }
            return View(sa);
        }

        // GET: SegmentArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle.FindAsync(id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            SegmentArticleViewModel s = new SegmentArticleViewModel();
            s.Order = segmentArticle.Order;
            s.ArticleUrl = segmentArticle.ArticleUrl;
            s.Title = segmentArticle.Title;
            //s.Image = segmentArticle.Image; TODO still need to fix this crap
            //s.EmbeddedVideo = segmentArticle.EmbeddedVideo;
            s.ArticleContent = segmentArticle.ArticleContent;
            s.PublishFrom = segmentArticle.PublishFrom;
            s.PublishTill = segmentArticle.PublishTill;
            s.CompanyID = segmentArticle.CompanyID;
            s.Author = (segmentArticle.Author == null || segmentArticle.Author == "" ? _user.name : segmentArticle.Author);
            s.AllowComments = segmentArticle.AllowComments;
            s.Tags = segmentArticle.Tags;

            if (segmentArticle.Segments != null && segmentArticle.Segments != "")
            {
                s.AvailableSegments = segmentArticle.Segments.Split(',').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
            }
                //Add any options not already in the segmentlist
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            var listOfAllSegements = dla.GetArticleSegments(_user.companyID);
            foreach (var seg in listOfAllSegements)
            {
                var match = s.AvailableSegments.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value));

                if (match == null)
                {
                    //if no match (not in the list) then add it
                    s.AvailableSegments.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }
            }

            return View(s);
        }

        // POST: SegmentArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [DisableRequestSizeLimit]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SegmentArticleViewModel sa)
        {
            if (id != sa.ID)
            {
                return NotFound();
            }

            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            var myArticle = new SegmentArticle();

            if (ModelState.IsValid)
            {
                myArticle.ID = sa.ID;
                myArticle.CompanyID = _user.companyID;
                myArticle.Order = sa.Order ?? 1;
                myArticle.Title = sa.Title;
                myArticle.PublishFrom = sa.PublishFrom;
                myArticle.PublishTill = sa.PublishTill;
                myArticle.AllowComments = sa.AllowComments;
                myArticle.ArticleContent = sa.ArticleContent;
                myArticle.Author = sa.Author;
                myArticle.Segments = string.Join(",", sa.SelectedSegments);
                myArticle.Tags = sa.Tags;

                IFormFile img = sa.Image;
                if (img != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads/Articles", _user.companyName + "_" + WebUtility.HtmlEncode(myArticle.Title) + "_" + img.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myArticle.Image = img.FileName;
                }

                IFormFile vid = sa.EmbeddedVideo;
                if (vid != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads/Articles", _user.companyName + "_" + WebUtility.HtmlEncode(myArticle.Title) + "_" + vid.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //async upload for now seems best as they want large files to be uploadable
                        vid.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    myArticle.EmbeddedVideo = vid.FileName;
                }

                try
                {
                    _context.Update(myArticle);
                    await _context.SaveChangesAsync();

                    //update the selected article segments

                    //DatabaseAccessLayer dla = new DatabaseAccessLayer(_context, _user.companyID);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentArticleExists(myArticle.ID))
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
            return View(sa);
        }

        // GET: SegmentArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticle = await _context.SegmentArticle
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(segmentArticle);
        }

        // POST: SegmentArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var segmentArticle = await _context.SegmentArticle.FindAsync(id);
            _context.SegmentArticle.Remove(segmentArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SegmentArticleExists(int id)
        {
            return _context.SegmentArticle.Any(e => e.ID == id);
        }
    }
}
