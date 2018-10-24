using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationUser _user;
        private int _companyID;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly string _rootpath;
        private readonly IHostingEnvironment _env;

        public UsersController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _env = env;
            _rootpath = _env.WebRootPath;
        }


        // GET: Users
        public async Task<IActionResult> Index(string id)
        {

            _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var role = (Roles)Enum.Parse(typeof(Roles), id);
            HttpContext.Session.SetString("ManageType", role.ToString());
            return View(await _context.User
                .Where(m => m.CompanyID == _companyID)
                .Where(n => n.Role == role)
                .ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var r = HttpContext.Session.GetString("ManageType");
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            UserViewModel s = new UserViewModel();
            var cp = _user.companyID;
            s.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            s.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
            s.Role = (Roles)Enum.Parse(typeof(Roles), r);
            s.Departments = dla.GetDepartments(cp);
            s.Genders = dla.GetGenders();
            return View(s);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            var companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            if (ModelState.IsValid)
            {
                var r = HttpContext.Session.GetString("ManageType");
                _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
                User un = new User();
                un.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                un.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
                un.Role = (Roles)Enum.Parse(typeof(Roles), r);
                un.Gender = user.Gender;
                un.EmailAddress = user.EmailAddress;
                un.Name = user.Name;
                un.PhoneNumber = user.PhoneNumber;
                un.WebsiteLanguage = user.WebsiteLanguage;
                un.CreatedOn = DateTime.Now;
                un.CreatedBy = _user.UserName;
               // un.CategoriesID = user.Categories;

                IFormFile avatar = user.Photo;
                if (avatar != null)
                {
                    var filePath = Path.Combine(_rootpath, "images/Uploads", _user.companyName + "_" + avatar.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }
                    //The file has been saved to disk - now save the file name to the DB
                    un.Photo = avatar.FileName;
                }

                //now for the tricky bit - have to do a normal asp.net registration and if a companyadmin or admin set the appropriate claim type

                //Create the user
                if (CreateDotNetUser(user).Succeeded)
                {
                    //Add the claim type - it will be exactly as per the type passed to the Index in the first place
                    var userForClaims = await _userManager.FindByEmailAsync(user.EmailAddress);
                    var ac = await _userManager.GetClaimsAsync(userForClaims);

                    if (ac.Count() <= 0)
                    {
                        await _userManager.AddToRoleAsync(userForClaims, r);
                        await _userManager.AddClaimAsync(userForClaims, new Claim(ClaimTypes.Role, r));
                    }
                }
                //Add the user to the user table as well at the identity tables
                _context.Add(un);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Users", new { id = r });
            }
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            user.Departments = dla.GetDepartments(companyID);
            user.Genders = dla.GetGenders();
            return View(user);
        }

        public IdentityResult CreateDotNetUser(UserViewModel un)
        {
            var user = new ApplicationUser { UserName = un.EmailAddress, Email = un.EmailAddress, companyName = HttpContext.Session.GetString("CompanyName") ?? "", companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0, name = un.Name };
            var result = _userManager.CreateAsync(user, un.Password).GetAwaiter().GetResult();
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = _userManager.GenerateEmailConfirmationTokenAsync(user).GetAwaiter().GetResult();
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                _emailSender.SendEmailAsync(un.EmailAddress, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").GetAwaiter().GetResult();

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return result;
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmailAddress,Name,Role,PhoneNumber,Gender,Photo,WebsiteLanguage")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
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
            return View(user);
        }


        public async Task<IActionResult> DeleteDotNetUser(int id)
        {
            //Get the email address then search for the .net id based on that
            var usr = _context.User.FirstOrDefaultAsync(m => m.ID == id).GetAwaiter().GetResult();
            var user = await _userManager.FindByEmailAsync(usr.EmailAddress);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);

            var r = HttpContext.Session.GetString("ManageType");
            await DeleteDotNetUser(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Users", new { id = r });
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
        }
    }
}
