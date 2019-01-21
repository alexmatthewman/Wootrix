using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;
using Microsoft.AspNetCore.Authorization;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class CompanySegmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _env;
        private readonly string _rootpath;
        private readonly UserManager<ApplicationUser> _userManager;
        private WootrixV2.Models.User _user;
        private WootrixV2.Models.Company _cpy;
        private int _companyID;
        private DataAccessLayer _dla;

        public CompanySegmentsController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context)
        {
            _context = context;
            _env = env;
            _rootpath = _env.WebRootPath;
            _userManager = userManager;
            _dla = new DataAccessLayer(_context);


            
        }

        #region Article Ordering
        public async Task<IActionResult> ChangeArticleOrder(string id)
        {
            string[] orderArray = id.Split("|");
            bool success = int.TryParse(orderArray[1].ToString(), out int articleID);
            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var article = await _context.SegmentArticle.FindAsync(articleID);
            int whereItCurrentlyIs = article.Order ?? 0;
            success = int.TryParse(orderArray[0].ToString(), out int whereItIsMovingTo);

            var segID = HttpContext.Session.GetInt32("SegmentID") ?? 0;
            CompanySegment cs = await _context.CompanySegment.FirstOrDefaultAsync(m => m.ID == segID && m.CompanyID == _companyID);

            var segmentArticle = _context.SegmentArticle
                .Where(n => n.CompanyID == cs.CompanyID)
                .Where(m => m.Segments.Contains(cs.Title));

            // So for each segment with an order greater than the order we need to increment the order number
            if (whereItCurrentlyIs < whereItIsMovingTo)
            {
                foreach (var art in segmentArticle.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) <= whereItIsMovingTo) && (m.Order ?? 0) > whereItCurrentlyIs))
                {
                    var updatedSegmentsAndOrders = "";
                    //Split the segments into a list, grab their order and increment it then save the change
                    var segments = art.Segments;
                    var segmentsList = art.Segments.Split("|");
                    foreach (string segmentTitleAndOrder in segmentsList)
                    {
                        var ender = "";
                        // If this isn't the last title, add a delimited
                        if (segmentsList.Last() != segmentTitleAndOrder) ender = "|";

                        if (segmentTitleAndOrder.Contains(cs.Title))
                        {
                            //Get the order and increment
                            var titleAndOrder = segmentTitleAndOrder.Split("/");
                            int ord;
                            bool success2 = int.TryParse(titleAndOrder[1], out ord);
                            ord--;
                            updatedSegmentsAndOrders += titleAndOrder[0] + "/" + ord + ender;

                        }
                        else
                        {
                            // just add it unchanged 
                            updatedSegmentsAndOrders += segmentTitleAndOrder + ender;
                        }
                    }
                    art.Segments = updatedSegmentsAndOrders;
                    art.Order--;
                    _context.Update(art);
                }
            }
            else
            {
                foreach (var art in segmentArticle.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) >= whereItIsMovingTo) && (m.Order ?? 0) < whereItCurrentlyIs))
                {
                    var updatedSegmentsAndOrders = "";
                    //Split the segments into a list, grab their order and increment it then save the change
                    var segments = art.Segments;
                    var segmentsList = art.Segments.Split("|");
                    foreach (string segmentTitleAndOrder in segmentsList)
                    {
                        var ender = "";
                        // If this isn't the last title, add a delimited
                        if (segmentsList.Last() != segmentTitleAndOrder) ender = "|";

                        if (segmentTitleAndOrder.Contains(cs.Title))
                        {
                            //Get the order and increment
                            var titleAndOrder = segmentTitleAndOrder.Split("/");
                            int ord;
                            bool success2 = int.TryParse(titleAndOrder[1], out ord);
                            ord++;
                            updatedSegmentsAndOrders += titleAndOrder[0] + "/" + ord + ender;

                        }
                        else
                        {
                            // just add it unchanged 
                            updatedSegmentsAndOrders += segmentTitleAndOrder + ender;
                        }
                    }
                    art.Segments = updatedSegmentsAndOrders;
                    art.Order++;
                    _context.Update(art);
                }
            }
            // Update the order of the original Article as well
            article.Order = whereItIsMovingTo;

            var uso = "";
            var segList = article.Segments.Split("|");
            foreach (string sto in segList)
            {
                var ender = "";
                // If this isn't the last title, add a delimited
                if (segList.Last() != sto) ender = "|";
                if (sto.Contains(cs.Title))
                {
                    //Get the order and increment
                    var to = sto.Split("/");
                    uso += to[0] + "/" + whereItIsMovingTo + ender;
                }
                else
                {
                    // just add it unchanged 
                    uso += sto + ender;
                }
            }
            article.Segments = uso;


            _context.Update(article);
            _context.SaveChanges();

            return RedirectToAction("Details", "CompanySegments", new { id = segID });
        }


        public async Task<IActionResult> ChangeOrder(string id)
        {
            var orderArray = id.Split("|");
            var order = orderArray[0].ToString();
            int segmentID;
            bool success = Int32.TryParse(orderArray[1].ToString(), out segmentID);
            _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var segment = await _context.CompanySegment.FindAsync(segmentID);
            int whereItCurrentlyIs = segment.Order ?? 0;
            success = Int32.TryParse(order, out int whereItIsMovingTo);

            // So for each segment with an order greater than the order we need to increment the order number
            if (whereItCurrentlyIs < whereItIsMovingTo)
            {
                foreach (var seg in _context.CompanySegment.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) <= whereItIsMovingTo) && (m.Order ?? 0) > whereItCurrentlyIs))
                {
                    seg.Order--;
                    _context.Update(seg);
                }
            }
            else
            {
                foreach (var seg in _context.CompanySegment.Where(m => m.CompanyID == _companyID && ((m.Order ?? 0) >= whereItIsMovingTo) && (m.Order ?? 0) < whereItCurrentlyIs))
                {
                    seg.Order++;
                    _context.Update(seg);
                }
            }
            // Update the order of the segmentID to be as passed
            segment.Order = whereItIsMovingTo;
            _context.Update(segment);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Decrement everything else
        public void InsertAtOrder1()
        {
            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            foreach (var seg in _context.CompanySegment.Where(m => m.CompanyID == _companyID))
            {
                seg.Order++;
                _context.Update(seg);
            }
            _context.SaveChanges();
        }

        // Decrement everything else
        public void InsertAtOrder1(int oldOrder)
        {
            // If the magazine was at position 5 we only need to decrement the order
            // of magazines 1-4
            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            foreach (var seg in _context.CompanySegment.Where(m => m.CompanyID == _companyID))
            {
                if (seg.Order < oldOrder)
                {
                    seg.Order++;
                    _context.Update(seg);
                }
            }
            _context.SaveChanges();
        }

        // Decrement everything below it
        public void DeleteSegmentAndUpdateOthersOrder(int deletedSegmentOrder)
        {
            var _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            foreach (var seg in _context.CompanySegment.Where(m => m.CompanyID == _companyID && m.Order > deletedSegmentOrder))
            {
                seg.Order--;
                _context.Update(seg);
            }
            _context.SaveChanges();
        }

        #endregion

        // GET: CompanySegments
        public async Task<IActionResult> UserSegmentSearch(string SearchString)
        {
            DataAccessLayer dla = new DataAccessLayer(_context);
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
            var segments = dla.GetSegmentsList(_cpy.ID, _user, SearchString, "");
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";
            return View(segments);
        }


        // GET: CompanySegments
        public async Task<IActionResult> Index()
        {
            //Get the company name out the session and use it as a filter for the groups returned

            // We also need to filter on department.
            // So if a segment is set to only be Editable by Company Admins of Department IT, 
            // then if the current Company Admin is in Department Marketing they wont see it.
            // Note that if the segment doesn't have a department set, everyone sees it



            // Get current user department
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

            var department = _user.Categories; //bad naming for the old DB i know
            List<CompanySegment> ctx;

            // If the user doesn't have a department don't filter on it
            if (!string.IsNullOrWhiteSpace(department))
            {
                ctx = await _context.CompanySegment
                  .Where(m => m.CompanyID == _cpy.ID)
                  .Where(m => m.Department == department)
                  .OrderBy(m => m.Order)
                  .ToListAsync();
            }
            else
            {
                ctx = await _context.CompanySegment
                .Where(m => m.CompanyID == _cpy.ID)
                .OrderBy(m => m.Order)
                .ToListAsync();
            }
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";
            return View(ctx);
        }

        // GET: SegmentArticles/Articlelist/id of segment
        public async Task<IActionResult> ArticleList(int id)
        {

            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
            HttpContext.Session.SetInt32("SegmentListID", id);
            

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
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";

            DataAccessLayer dla = new DataAccessLayer(_context);
            // Now for users we need to show them articles on the home page so get them in the ViewBag for display   
            //Also add the Segment to the Viewbag so we can get the Image
            CompanySegment cs = await _context.CompanySegment.FirstOrDefaultAsync(m => m.ID == id && m.CompanyID == _cpy.ID);
            var segmentArticle = dla.GetArticlesListBasedOnThisUsersFilters(_user, "", cs).OrderBy(m =>m.Order);
            if (segmentArticle == null)
            {
                return NotFound();
            }

            ViewBag.SegmentCoverImage = cs.CoverImage ?? "";
            ViewBag.SegmentTitle = cs.Title ?? "";
            ViewBag.CompanyName = _user.CompanyName ?? "";

            return View(segmentArticle);
        }

        // GET: SegmentArticles/Articlelist/id of segment
        public async Task<IActionResult> UserArticleSearch(string searchString)
        {
            if (searchString == null)
            {
                return NotFound();
            }
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
            var segmentid = HttpContext.Session.GetInt32("SegmentListID");

            DataAccessLayer dla = new DataAccessLayer(_context);

            //Also add the Segment to the Viewbag so we can get the Image
            CompanySegment cs = await _context.CompanySegment.FirstOrDefaultAsync(m => m.ID == segmentid && m.CompanyID == _cpy.ID);

            var segmentArticle = dla.GetArticlesListBasedOnThisUsersFilters(_user, "", cs);
            ViewBag.Segment = cs;
            if (segmentArticle == null)
            {
                return NotFound();
            }
            ViewBag.UploadsLocation = "https://s3-us-west-2.amazonaws.com/wootrixv2uploadfiles/images/Uploads/";
            return View(segmentArticle);
        }

        // GET: CompanySegments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

            HttpContext.Session.SetInt32("SegmentID", id);
            CompanySegment cs = await _context.CompanySegment.FirstOrDefaultAsync(m => m.ID == id && m.CompanyID == _cpy.ID);

            var segmentArticle = _context.SegmentArticle
                .Where(n => n.CompanyID == _cpy.ID)
                .Where(m => m.Segments.Contains(cs.Title));

            // OK so now we are going to set the article Order field to be as show in the Segments so its easier to work with. 
            // We will need to save the segments back again if there is a change

            foreach (SegmentArticle item in segmentArticle)
            {
                //Split the segments into a list, grab their order and increment it then save the change
                var segments = item.Segments;
                var segmentsList = item.Segments.Split("|");
                foreach (string segmentTitleAndOrder in segmentsList)
                {
                    if (segmentTitleAndOrder.Contains(cs.Title))
                    {
                        //Get the order and increment
                        var titleAndOrder = segmentTitleAndOrder.Split("/");
                        bool success = int.TryParse(titleAndOrder[1], out int ord);

                        // Set the article Order
                        item.Order = ord;
                    }
                }
                _context.Update(item);
            }

            _context.SaveChanges();
            segmentArticle = _context.SegmentArticle
                .Where(n => n.CompanyID == cs.CompanyID)
                .Where(m => m.Segments.Contains(cs.Title))
                .OrderBy(p => p.Order);

            //Also add the Segment to the Viewbag so we can get the Image
            ViewBag.Segment = cs;

            if (segmentArticle == null)
            {
                return NotFound();
            }

            return View(await segmentArticle.ToListAsync());
        }

        // GET: CompanySegments/Create
        public IActionResult Create()
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
            CompanySegmentViewModel s = new CompanySegmentViewModel();
            s.Order = 1;
            s.PublishDate = DateTime.Now.Date;
            s.FinishDate = DateTime.Now.AddYears(10).Date;
            s.StandardColor = HttpContext.Session.GetString("CompanyHeaderBackgroundColor");
            s.ThemeColor = HttpContext.Session.GetString("CompanyHighlightColor");
            s.ClientName = _user.Name;
            //s.ClientLogoImage = _user.photoUrl;
            var cp = _user.CompanyID;

            DataAccessLayer dla = new DataAccessLayer(_context);
            s.Departments = dla.GetDepartments(cp);
            return View(s);
        }

        // POST: CompanySegments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanySegmentViewModel cps)
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

            var companyID = _cpy.ID;
            //Initialise a new companysegment
            var mySegment = new CompanySegment();
            if (ModelState.IsValid)
            {
                //ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags
                mySegment.CompanyID = companyID;
                mySegment.Order = cps.Order ?? 1;
                mySegment.Title = cps.Title;
                mySegment.PublishDate = cps.PublishDate;
                mySegment.FinishDate = cps.FinishDate;
                mySegment.ClientName = cps.ClientName;
                mySegment.ThemeColor = cps.ThemeColor;
                mySegment.StandardColor = cps.StandardColor;
                mySegment.Draft = DateTime.Now > cps.PublishDate ? false : true;
                mySegment.Department = cps.Department;
                mySegment.Tags = cps.Tags;
                mySegment.ClientName = cps.ClientName ?? _user.Name;

                IFormFile coverImage = cps.CoverImage;
                if (coverImage != null)
                {
                    await _dla.UploadFileToS3(coverImage, _user.CompanyName + "_" + coverImage.FileName, "images/Uploads");
                    mySegment.CoverImage = coverImage.FileName;
                }

                IFormFile coverImageMB = cps.CoverImageMobileFriendly;
                if (coverImageMB != null)
                {
                    await _dla.UploadFileToS3(coverImageMB, _user.CompanyName + "_" + coverImageMB.FileName, "images/Uploads");
                    //var filePath = Path.Combine(_rootpath, "images/Uploads", _user.companyName + "_" + coverImageMB.FileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await coverImageMB.CopyToAsync(stream);
                    //}
                    //The file has been saved to disk - now save the file name to the DB
                    mySegment.CoverImageMobileFriendly = coverImageMB.FileName;
                }

                IFormFile cli = cps.ClientLogoImage;
                if (cli != null)
                {
                    await _dla.UploadFileToS3(cli, _user.CompanyName + "_" + cli.FileName, "images/Uploads");
                    //The file has been saved to disk - now save the file name to the DB
                    mySegment.CoverImageMobileFriendly = cli.FileName;
                }
                InsertAtOrder1();
                _context.Add(mySegment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            DataAccessLayer dla = new DataAccessLayer(_context);
            cps.Departments = dla.GetDepartments(companyID);
            return View(cps);
        }

        // GET: CompanySegments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

            var companySegment = await _context.CompanySegment.FindAsync(id);
            if (companySegment == null)
            {
                return NotFound();
            }

            CompanySegmentViewModel s = new CompanySegmentViewModel();

            s.Order = companySegment.Order;
            s.Title = companySegment.Title;
            s.PublishDate = companySegment.PublishDate;
            s.FinishDate = companySegment.FinishDate;
            s.StandardColor = companySegment.StandardColor;
            s.ThemeColor = companySegment.ThemeColor;
            s.ClientName = (companySegment.ClientName ?? _user.Name);
            // s.ClientLogoImage = FormFileHelper.PhysicalToIFormFile(new FileInfo(companySegment.ClientLogoImage));
            s.Department = companySegment.Department;
            s.Tags = companySegment.Tags;

            DataAccessLayer dla = new DataAccessLayer(_context);
            s.Departments = dla.GetDepartments(_user.CompanyID);
            return View(s);
        }

        // POST: CompanySegments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompanySegmentViewModel cps)
        {
            if (id != cps.ID)
            {
                return NotFound();
            }
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

            //Initialise a new companysegment
            CompanySegment mySegment = await _context.CompanySegment.FindAsync(id);

            //Have to check the Segment title isn't already used - it gets weird otherwise
            var existingSeg = _context.SegmentArticle.FirstOrDefault(n => n.Title == cps.Title && n.CompanyID == cps.CompanyID);
            if (existingSeg == null)
            {
                if (ModelState.IsValid)
                {
                    //ID,Order,Title,CoverImage,CoverImageMobileFriendly,PublishDate,FinishDate,ClientName,ClientLogoImage,ThemeColor,StandardColor,Draft,Department,Tags
                    mySegment.CompanyID = _user.CompanyID;


                    mySegment.PublishDate = cps.PublishDate;
                    mySegment.FinishDate = cps.FinishDate;
                    mySegment.ClientName = cps.ClientName;
                    mySegment.ThemeColor = cps.ThemeColor;
                    mySegment.StandardColor = cps.StandardColor;
                    mySegment.Draft = DateTime.Now > cps.PublishDate ? false : true;
                    mySegment.Department = cps.Department;
                    mySegment.Tags = cps.Tags;

                    IFormFile coverImage = cps.CoverImage;
                    if (coverImage != null)
                    {
                        await _dla.UploadFileToS3(coverImage, _user.CompanyName + "_" + coverImage.FileName, "images/Uploads");

                        mySegment.CoverImage = coverImage.FileName;
                    }

                    IFormFile coverImageMB = cps.CoverImageMobileFriendly;
                    if (coverImageMB != null)
                    {
                        await _dla.UploadFileToS3(coverImageMB, _user.CompanyName + "_" + coverImageMB.FileName, "images/Uploads");
                        //The file has been saved to disk - now save the file name to the DB
                        mySegment.CoverImageMobileFriendly = coverImageMB.FileName;
                    }

                    IFormFile cli = cps.ClientLogoImage;
                    if (cli != null)
                    {
                        await _dla.UploadFileToS3(cli, _user.CompanyName + "_" + cli.FileName, "images/Uploads");
                        //The file has been saved to disk - now save the file name to the DB
                        mySegment.CoverImageMobileFriendly = cli.FileName;
                    }


                    try
                    {
                        // Done later to avoid ordering failures if the image upload fails.
                        if (mySegment.Order != 1)
                        {
                            // We only need to decrement articles above it (lower order)
                            InsertAtOrder1(mySegment.Order ?? 1);
                        }
                        mySegment.Order = 1;

                        // Ok but what about the connected articles? We need to update every article segments field and change the old val to the new

                        // If the title changed
                        if (mySegment.Title != cps.Title)
                        {
                            foreach (var art in _context.SegmentArticle.Where(m => m.CompanyID == _user.CompanyID))
                            {
                                if (art.Segments.Contains(mySegment.Title))
                                {
                                    var oldSegments = art.Segments;
                                    var newSegments = oldSegments.Replace(mySegment.Title, cps.Title);
                                    art.Segments = newSegments;
                                    _context.Update(art);
                                }
                            }
                        }

                        mySegment.Title = cps.Title;

                        _context.Update(mySegment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CompanySegmentExists(cps.ID))
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
            }
            else
            {
                // Article title already exists
                ModelState.AddModelError(string.Empty, "Magazine Title already exists - please choose something unique");
            }

            return RedirectToAction("Edit", new { id = cps.ID });
        }

        // GET: CompanySegments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
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
            //Update order of other segments
            DeleteSegmentAndUpdateOthersOrder(companySegment.Order ?? 1);
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
