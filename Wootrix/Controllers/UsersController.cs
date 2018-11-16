using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly IOptions<RequestLocalizationOptions> _rlo;

        public UsersController(IOptions<RequestLocalizationOptions> rlo, UserManager<ApplicationUser> userManager, IHostingEnvironment env, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
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
            _rlo = rlo;
        }


        // GET: Users        
        public async Task<IActionResult> Index(string id)
        {

            _companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var role = (Roles)Enum.Parse(typeof(Roles), id);
            HttpContext.Session.SetString("ManageType", role.ToString());
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            List<User> data = new List<User>();

            ViewBag.Companies = dla.GetCompanies();
            if (role.ToString() == "Admin")
            {
                Roles cpa = Roles.CompanyAdmin;
                // just return all CompanyAdmins for all companies
                data = await _context.User
                    .Where(n => n.Role == cpa)
                    .OrderBy(n => n.CompanyName)
                    .ToListAsync();

            }
            else
            {
                data = await _context.User
                   .Where(m => m.CompanyID == _companyID)
                   .Where(n => n.Role == role)
                   .OrderBy(n => n.EmailAddress)
                   .ToListAsync();
                foreach (var item in data)
                {
                    if (!string.IsNullOrWhiteSpace(item.Categories) && item.Categories != "User")
                    {
                        DatabaseAccessLayer dla1 = new DatabaseAccessLayer(_context);

                        item.Categories = dla1.GetCompanyDepartmentName(item.Categories);
                    }
                    //if (!string.IsNullOrWhiteSpace(item.Country))
                    //{
                    //    DatabaseAccessLayer dla2 = new DatabaseAccessLayer(_context);

                    //    item.Country = dla2.GetCountryName(item.Country);
                    //}
                    //if (!string.IsNullOrWhiteSpace(item.State))
                    //{
                    //    DatabaseAccessLayer dla3 = new DatabaseAccessLayer(_context);

                    //    item.State = dla3.GetStateName(item.State);
                    //}

                }
            }
            return View(data);
        }



        // GET: Users/Details/5
        public async Task<IActionResult> BulkUpload()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkUpload(BulkUploadViewModel uf)
        {
            if (ModelState.IsValid)
            {


                _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                CsvReader csv;
                IEnumerable<BulkUploadDataViewModel> records;
                IFormFile file = uf.BulkUploadFile;

                if (file != null && file.Length > 0)
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        try
                        {
                            csv = new CsvReader(reader);
                            records = csv.GetRecords<BulkUploadDataViewModel>();


                            foreach (BulkUploadDataViewModel budvm in records)
                            {
                                //Awesome - create a user for each row
                                User un = new User();
                                un.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
                                un.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
                                un.Role = Roles.User;

                                un.EmailAddress = budvm.EmailAddress;
                                un.Name = budvm.Name;
                                un.PhoneNumber = budvm.PhoneNumber;
                                un.Gender = budvm.Gender;
                                un.InterfaceLanguage = budvm.InterfaceLanguage;
                                un.WebsiteLanguage = budvm.ArticleLanguages;
                                un.Topics = budvm.Topics;
                                un.Groups = budvm.Groups;
                                un.TypeOfUser = budvm.TypeOfUser;
                                un.Country = budvm.Country;
                                un.State = budvm.State;
                                un.City = budvm.City;
                                un.CreatedOn = DateTime.Now;
                                un.CreatedBy = _user.UserName;


                                //now for the tricky bit - have to do a normal asp.net registration and if a companyadmin or admin set the appropriate claim type
                                //Create the user
                                var user = new UserViewModel { EmailAddress = budvm.EmailAddress, CompanyName = HttpContext.Session.GetString("CompanyName") ?? "", CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0, Name = budvm.Name };

                                if (CreateDotNetUser(user, "ChangePassword1!").Succeeded)
                                {
                                    //Add the claim type - it will be exactly as per the type passed to the Index in the first place
                                    var userForClaims = await _userManager.FindByEmailAsync(user.EmailAddress);
                                    var ac = await _userManager.GetClaimsAsync(userForClaims);

                                    if (ac.Count() <= 0)
                                    {
                                        await _userManager.AddToRoleAsync(userForClaims, "User");
                                        await _userManager.AddClaimAsync(userForClaims, new Claim(ClaimTypes.Role, "User"));
                                    }
                                    //Add the user to the user table as well at the identity tables
                                    _context.Add(un);
                                    
                                }
                             
                            }
                            await _context.SaveChangesAsync();
                            ViewBag.Message = "Users successfully created";
                            return RedirectToAction("Index", "Users", new { id = HttpContext.Session.GetString("ManageType") });

                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                            ViewBag.Message = e + System.Environment.NewLine;
                        }
                    }
                }
            }
            ViewBag.Message +="    User creation failed";
            return View(uf);
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
        public IActionResult Create(int? id)
        {
            if (!string.IsNullOrWhiteSpace(id.ToString()))
            {
                //Admin user has selected a companyID so set the current session stuff appropriately
                var companyID = id ?? 0;
                var companyName = _context.Company.FirstOrDefaultAsync(n => n.ID == companyID).GetAwaiter().GetResult().CompanyName;
                HttpContext.Session.SetInt32("CompanyID", companyID);
                HttpContext.Session.SetString("CompanyName", companyName);
            }

            var r = HttpContext.Session.GetString("ManageType");
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            UserViewModel s = new UserViewModel();

            var cp = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            s.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            s.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
            s.Role = (Roles)Enum.Parse(typeof(Roles), r);
            s.Departments = dla.GetDepartments(cp);
            s.Genders = dla.GetGenders();
            s.InterfaceLanguages = dla.GetInterfaceLanguages(cp, _rlo);

            if (r == "User")
            {

                // Add group checkboxes
                var listOfAllGroups = dla.GetListGroups(_user.companyID);
                foreach (var seg in listOfAllGroups)
                {
                    s.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add topic checkboxes
                var listOfAllTopics = dla.GetListTopics(_user.companyID);
                foreach (var seg in listOfAllTopics)
                {
                    s.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add type checkboxes
                var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.companyID);
                foreach (var seg in listOfAllTypeOfUser)
                {
                    s.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add language checkboxes
                var listOfAllLanguages = dla.GetListLanguages(_user.companyID, _rlo);
                foreach (var seg in listOfAllLanguages)
                {
                    s.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Get location dropdown data
                s.Countries = dla.GetCountries();
                s.States = dla.GetNullStatesOrCities();
                s.Cities = dla.GetNullStatesOrCities();
            }
            return View(s);
        }

        public IEnumerable<SelectListItem> GetRegions()
        {
            List<SelectListItem> regions = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return regions;
        }


        [HttpGet]
        public JsonResult GetStates(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode) && countryCode.Length == 2)
            {
                HttpContext.Session.SetString("countryCode", countryCode);
                DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
                IEnumerable<SelectListItem> states = dla.GetStates(countryCode);
                return Json(states);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetCities(string stateCode)
        {
            if (!string.IsNullOrWhiteSpace(stateCode))
            {
                string countryCode = HttpContext.Session.GetString("countryCode");
                DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
                IEnumerable<SelectListItem> cities = dla.GetCities(countryCode, stateCode);
                return Json(cities);
            }
            return null;
        }


        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            var companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            var r = HttpContext.Session.GetString("ManageType");
            if (r == "Admin") r = "CompanyAdmin";

            if (ModelState.IsValid)
            {
                //now for the tricky bit - have to do a normal asp.net registration and if a companyadmin or admin set the appropriate claim type

                //Create the user
                var result = CreateDotNetUser(user);
                if (result.Succeeded)
                {
                    //Add the claim type - it will be exactly as per the type passed to the Index in the first place
                    var userForClaims = await _userManager.FindByEmailAsync(user.EmailAddress);
                    var ac = await _userManager.GetClaimsAsync(userForClaims);

                    if (ac.Count() <= 0)
                    {
                        await _userManager.AddToRoleAsync(userForClaims, r);
                        await _userManager.AddClaimAsync(userForClaims, new Claim(ClaimTypes.Role, r));
                    }

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
                    un.Categories = user.Categories;
                    un.InterfaceLanguage = user.InterfaceLanguage;

                    un.Groups = string.Join("|", user.SelectedGroups);
                    un.Topics = string.Join("|", user.SelectedTopics);
                    un.TypeOfUser = string.Join("|", user.SelectedTypeOfUser);
                    un.WebsiteLanguage = string.Join("|", user.SelectedLanguages);
                    if (user.Country != null && user.Country != "") un.Country = _context.LocationCountries.FirstOrDefault(m => m.country_code == user.Country).country_name;
                    if (user.State != null && user.State != "") un.State = _context.LocationStates.FirstOrDefault(n => n.country_code == user.Country && n.state_code == user.State).state_name;

                    // un.Country = user.Country;
                    // un.State = user.State;
                    un.City = user.City;

                    IFormFile avatar = user.Photo;
                    if (avatar != null)
                    {
                        var filePath = Path.Combine(_rootpath, "images/Uploads/Users", HttpContext.Session.GetString("CompanyName") + "_" + avatar.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await avatar.CopyToAsync(stream);
                        }
                        //The file has been saved to disk - now save the file name to the DB
                        un.Photo = avatar.FileName;
                    }


                    //Add the user to the user table as well at the identity tables
                    _context.Add(un);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Users", new { id = HttpContext.Session.GetString("ManageType") });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                   
                
               
            }
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            user.Departments = dla.GetDepartments(companyID);
            user.Genders = dla.GetGenders();
            user.InterfaceLanguages = dla.GetInterfaceLanguages(companyID, _rlo);
            if (HttpContext.Session.GetString("ManageType") == "User")
            {

                // Add group checkboxes
                var listOfAllGroups = dla.GetListGroups(companyID);
                foreach (var seg in listOfAllGroups)
                {
                    user.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add topic checkboxes
                var listOfAllTopics = dla.GetListTopics(companyID);
                foreach (var seg in listOfAllTopics)
                {
                    user.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add type checkboxes
                var listOfAllTypeOfUser = dla.GetListTypeOfUser(companyID);
                foreach (var seg in listOfAllTypeOfUser)
                {
                    user.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Add language checkboxes
                var listOfAllLanguages = dla.GetListLanguages(companyID, _rlo);
                foreach (var seg in listOfAllLanguages)
                {
                    user.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                }

                // Get location dropdown data
                user.Countries = dla.GetCountries();
                user.States = dla.GetNullStatesOrCities();
                user.Cities = dla.GetNullStatesOrCities();
            }
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
            
            return result;
        }

        public IdentityResult CreateDotNetUser(UserViewModel un, string password)
        {
            var user = new ApplicationUser { UserName = un.EmailAddress, Email = un.EmailAddress, companyName = HttpContext.Session.GetString("CompanyName") ?? "", companyID = HttpContext.Session.GetInt32("CompanyID") ?? 0, name = un.Name };
            var result = _userManager.CreateAsync(user, password).GetAwaiter().GetResult();
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

            var r = HttpContext.Session.GetString("ManageType");

            //If admin user get companyID/Name of the selected user
            if (!string.IsNullOrWhiteSpace(id.ToString()) && r == "Admin")
            {
                //Admin user has selected a companyID so set the current session stuff appropriately
                var companyID = id ?? 0;
                var _user = _context.User.AsNoTracking().Where(n => n.ID == id).SingleAsync().GetAwaiter().GetResult();

                //var companyName = _context.Company.FirstOrDefaultAsync(n => n.ID == companyID).GetAwaiter().GetResult().CompanyName;
                HttpContext.Session.SetInt32("CompanyID", _user.CompanyID);
                HttpContext.Session.SetString("CompanyName", _user.CompanyName);
            }

            var user = await _context.User.FindAsync(id);
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();
            UserViewModel s = new UserViewModel();
            var cp = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            s.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            s.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
            s.Role = user.Role;
            s.Departments = dla.GetDepartments(cp);
            s.Genders = dla.GetGenders();
            s.InterfaceLanguages = dla.GetInterfaceLanguages(cp, _rlo);

            s.EmailAddress = user.EmailAddress;
            s.Name = user.Name;
            //s.ID = user.ID;
            s.PhoneNumber = user.PhoneNumber;

            if (r == "User")
            {
                // Get location dropdown data
                s.Countries = dla.GetCountries();
                s.States = dla.GetNullStatesOrCities();
                s.Cities = dla.GetNullStatesOrCities();

                if (user.Groups != null && user.Groups != "")
                {
                    s.AvailableGroups = user.Groups.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllGroups = dla.GetListGroups(_user.companyID);
                foreach (var seg in listOfAllGroups)
                {
                    if (s.AvailableGroups.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        s.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }


                if (user.Topics != null && user.Topics != "")
                {
                    s.AvailableTopics = user.Topics.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllTopics = dla.GetListTopics(_user.companyID);
                foreach (var seg in listOfAllTopics)
                {
                    if (s.AvailableTopics.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        s.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }

                if (user.TypeOfUser != null && user.TypeOfUser != "")
                {
                    s.AvailableTypeOfUser = user.TypeOfUser.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.companyID);
                foreach (var seg in listOfAllTypeOfUser)
                {
                    if (s.AvailableTypeOfUser.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        s.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }

                if (user.WebsiteLanguage != null && user.WebsiteLanguage != "")
                {
                    s.AvailableLanguages = user.WebsiteLanguage.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllLanguages = dla.GetListLanguages(_user.companyID, _rlo);
                foreach (var seg in listOfAllLanguages)
                {
                    if (s.AvailableLanguages.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        s.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }
            }
            if (user == null)
            {
                return NotFound();
            }
            return View(s);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }
            User un = _context.User.Where(x => x.ID == id).FirstOrDefault();
            var r = HttpContext.Session.GetString("ManageType");

            if (ModelState.IsValid)
            {
                

                if (r == "Admin") r = "CompanyAdmin";

                try
                {
                    _user = _userManager.GetUserAsync(User).GetAwaiter().GetResult();

                    //un.ID = user.ID;
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
                    un.Categories = user.Categories;
                    _user.categories = user.Categories;
                    un.InterfaceLanguage = user.InterfaceLanguage;
                    un.Country = user.Country;
                    un.State = user.State;
                    // The country and state values are codes which aren't too useful - get the text
                    if (user.Country != null && user.Country != "") un.Country = _context.LocationCountries.FirstOrDefault(m => m.country_code == user.Country).country_name;
                    if (user.State != null && user.State != "") un.State = _context.LocationStates.FirstOrDefault(n => n.country_code == user.Country && n.state_code == user.State).state_name;
                    un.City = user.City;

                    un.Groups = string.Join("|", user.SelectedGroups);
                    un.Topics = string.Join("|", user.SelectedTopics);
                    un.TypeOfUser = string.Join("|", user.SelectedTypeOfUser);
                    un.WebsiteLanguage = string.Join("|", user.SelectedLanguages);

                    IFormFile avatar = user.Photo;
                    if (avatar != null)
                    {
                        var filePath = Path.Combine(_rootpath, "images/Uploads/Users", HttpContext.Session.GetString("CompanyName") + "_" + avatar.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await avatar.CopyToAsync(stream);
                        }
                        //The file has been saved to disk - now save the file name to the DB
                        un.Photo = avatar.FileName;
                    }

                    //Update the .net user every time
                    var usr = _context.User.FirstOrDefaultAsync(m => m.ID == id).GetAwaiter().GetResult();
                    var thisUser = await _userManager.FindByEmailAsync(usr.EmailAddress);
                    var result = await _userManager.UpdateAsync(thisUser);
                    var userId = await _userManager.GetUserIdAsync(thisUser);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Unexpected error occurred editing user with ID '{userId}'.");
                    }

                    _context.Update(un);
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
                return RedirectToAction("Index", "Users", new { id = HttpContext.Session.GetString("ManageType") });
            }
          
            DatabaseAccessLayer dla = new DatabaseAccessLayer(_context);
            user.Departments = dla.GetDepartments(HttpContext.Session.GetInt32("CompanyID") ?? 0);
            user.Genders = dla.GetGenders();
            var cp = HttpContext.Session.GetInt32("CompanyID") ?? 0;           
            user.InterfaceLanguages = dla.GetInterfaceLanguages(cp, _rlo);


           
            user.CompanyID = HttpContext.Session.GetInt32("CompanyID") ?? 0;
            user.CompanyName = HttpContext.Session.GetString("CompanyName") ?? "";
            user.Role = un.Role;
            user.Departments = dla.GetDepartments(cp);
            user.Genders = dla.GetGenders();
            user.InterfaceLanguages = dla.GetInterfaceLanguages(cp, _rlo);

            user.EmailAddress = un.EmailAddress;
            user.Name = un.Name;
            //user.ID = user.ID;
            user.PhoneNumber = un.PhoneNumber;

            if (r == "User")
            {
                // Get location dropdown data
                user.Countries = dla.GetCountries();
                user.States = dla.GetNullStatesOrCities();
                user.Cities = dla.GetNullStatesOrCities();

                if (user.Groups != null && user.Groups != "")
                {
                    user.AvailableGroups = user.Groups.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllGroups = dla.GetListGroups(cp);
                foreach (var seg in listOfAllGroups)
                {
                    if (user.AvailableGroups.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        user.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }


                if (user.Topics != null && user.Topics != "")
                {
                    user.AvailableTopics = user.Topics.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllTopics = dla.GetListTopics(cp);
                foreach (var seg in listOfAllTopics)
                {
                    if (user.AvailableTopics.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        user.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }

                if (user.TypeOfUser != null && user.TypeOfUser != "")
                {
                    user.AvailableTypeOfUser = user.TypeOfUser.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllTypeOfUser = dla.GetListTypeOfUser(cp);
                foreach (var seg in listOfAllTypeOfUser)
                {
                    if (user.AvailableTypeOfUser.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        user.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }

                if (user.WebsiteLanguage != null && user.WebsiteLanguage != "")
                {
                    user.AvailableLanguages = user.WebsiteLanguage.Split('|').Select(x => new SelectListItem { Text = x, Value = x, Selected = true }).ToList();
                }
                //Add any options not already in the segmentlist
                var listOfAllLanguages = dla.GetListLanguages(cp, _rlo);
                foreach (var seg in listOfAllLanguages)
                {
                    if (user.AvailableLanguages.FirstOrDefault(stringToCheck => stringToCheck.Value.Contains(seg.Value)) == null)
                    {
                        //if no match (not in the list) then add it
                        user.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
                    }
                }
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
