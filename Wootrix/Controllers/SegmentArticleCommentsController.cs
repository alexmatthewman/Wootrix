using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SegmentArticleCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;
        

        public SegmentArticleCommentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SegmentArticleComments
        public async Task<IActionResult> Index(int id)
        {
        
            HttpContext.Session.SetInt32("ArticleID", id);
            ViewBag.ArticleID = id;
            var ctx = _context.SegmentArticleComment
                .Where(m => m.SegmentArticleID == id);
            return View(await ctx.ToListAsync());
        }

        // GET: SegmentArticleComments
        public async Task<IActionResult> Admin(int id)
        {
            //Only show comments where the Status is "Review"

            var ctx = _context.SegmentArticleComment
                .Where(m => m.Status == "Review")
                .Where(m => m.CompanyID == id);
                
            return View(await ctx.ToListAsync());
        }

        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }
            segmentArticleComment.Status = "Approved";
            await _context.SaveChangesAsync();
            var ctx = _context.SegmentArticleComment
                .Where(m => m.Status == "Review")
                .Where(m => m.CompanyID == id);
            return RedirectToAction("Admin", new { id = segmentArticleComment.CompanyID });
            // return View(await ctx.ToListAsync());
        }

        public async Task<IActionResult> AdminDelete(int id)
        {
            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);
            _context.SegmentArticleComment.Remove(segmentArticleComment);
            await _context.SaveChangesAsync();
            var ctx = _context.SegmentArticleComment
               .Where(m => m.Status == "Review")
               .Where(m => m.CompanyID == id);
            return RedirectToAction("Admin", new { id = segmentArticleComment.CompanyID });
        }



        // GET: SegmentArticleComments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SegmentArticleComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SegmentArticleComment segmentArticleComment)
        {
            if (ModelState.IsValid)
            {
                var articleID = HttpContext.Session.GetInt32("ArticleID") ?? 0;
                _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                SegmentArticleComment sgc = new SegmentArticleComment();

                sgc.CompanyID = _user.companyID;
                sgc.UserID = _user.Id;
                sgc.UserName = _user.UserName;
                sgc.CreatedDate = DateTime.Now;
                sgc.Status = "Review";
                sgc.Comment = segmentArticleComment.Comment;

                sgc.SegmentArticleID = articleID;
                _context.Add(sgc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = articleID });
            }
            return View(segmentArticleComment);
        }


        // GET: SegmentArticleComments/Create
        public IActionResult ApproveReply(int id)
        {
            HttpContext.Session.SetInt32("CommentToApprove", id);
            var sac = _context.SegmentArticleComment.Find(id);
            ViewBag.OriginalComment = sac.Comment;
            return View();
        }

        // POST: SegmentArticleComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveReply(SegmentArticleComment segmentArticleComment)
        {
            if (ModelState.IsValid)
            {
                var articleID = HttpContext.Session.GetInt32("ArticleID") ?? 0;
                var commentToApproveID = HttpContext.Session.GetInt32("CommentToApprove") ?? 0;
                _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                SegmentArticleComment sgc = new SegmentArticleComment();

                //First of all approve the orignal comment
                var sac = await _context.SegmentArticleComment.FindAsync(commentToApproveID);
                if (sac == null)
                {
                    return NotFound();
                }
                sac.Status = "Approved";

                // Create the new comment but set the ReplyingToCommentID field
                sgc.CompanyID = _user.companyID;
                sgc.UserID = _user.Id;
                sgc.UserName = _user.UserName;
                //set the created date to be just after the original message so it is next
                sgc.CreatedDate = sac.CreatedDate.AddMilliseconds(1);
                sgc.Status = "Approved";
                sgc.Comment = segmentArticleComment.Comment;
                sgc.ReplyingToCommentID = commentToApproveID;

                sgc.SegmentArticleID = sac.SegmentArticleID;
                _context.Add(sgc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Admin", new { id = _user.companyID });

            }
            return View(segmentArticleComment);
        }




        // GET: SegmentArticleComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }
            return View(segmentArticleComment);
        }

        // POST: SegmentArticleComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Comment,CreatedDate")] SegmentArticleComment segmentArticleComment)
        {
            if (id != segmentArticleComment.ID)
            {
                return NotFound();
            }
            var articleID = HttpContext.Session.GetInt32("ArticleID") ?? 0;
            if (ModelState.IsValid)
            {
                try
                {
                    //When we edit the old fields except the ones being edited are wiped. So I needed to 
                    //lookup the old one then overwrite it as neeed
                    var oldComment = _context.SegmentArticleComment.FirstOrDefault(p => p.ID == id);
                    oldComment.Comment = segmentArticleComment.Comment;
                    segmentArticleComment = oldComment;
                    _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                    segmentArticleComment.CompanyID = _user.companyID;
                    segmentArticleComment.UserID = _user.Id;
                    segmentArticleComment.UserName = _user.UserName;
                    segmentArticleComment.EditDate = DateTime.Now;
                    segmentArticleComment.Status = "Review";
                    
                    segmentArticleComment.SegmentArticleID = articleID;
                    _context.Update(segmentArticleComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SegmentArticleCommentExists(segmentArticleComment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = articleID });
            }
            return View(segmentArticleComment);
        }

        // GET: SegmentArticleComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var segmentArticleComment = await _context.SegmentArticleComment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (segmentArticleComment == null)
            {
                return NotFound();
            }

            return View(segmentArticleComment);
        }

        // POST: SegmentArticleComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleID = HttpContext.Session.GetInt32("ArticleID") ?? 0;
            var segmentArticleComment = await _context.SegmentArticleComment.FindAsync(id);

            //So we also need to delete any replies that reference this
            var commentList = _context.SegmentArticleComment.Where(n => n.ReplyingToCommentID == id).ToList();
            foreach (SegmentArticleComment x in commentList)
            {
                _context.SegmentArticleComment.Remove(x);
            }

            _context.SegmentArticleComment.Remove(segmentArticleComment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = articleID });
        }

        private bool SegmentArticleCommentExists(int id)
        {
            return _context.SegmentArticleComment.Any(e => e.ID == id);
        }
    }
}
