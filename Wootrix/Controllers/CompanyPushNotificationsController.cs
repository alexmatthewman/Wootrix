using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wootrix.Data;
using WootrixV2.Data;
using WootrixV2.Models;

namespace WootrixV2.Controllers
{
    [Authorize]
    public class CompanyPushNotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private WootrixV2.Models.User _user;
        private readonly IOptions<RequestLocalizationOptions> _rlo;
        private WootrixV2.Models.Company _cpy;
        private DataAccessLayer _dla;

        public CompanyPushNotificationsController(IOptions<RequestLocalizationOptions> rlo, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _dla = new DataAccessLayer(_context);
            _rlo = rlo;
        }

        // GET: CompanyPushNotifications
        public async Task<IActionResult> Index()
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            var usrNotifications = _dla.GetNotificationsMatchingUserFilters(_user);
            int numberOfNotifications = usrNotifications.Count;
            HttpContext.Session.SetInt32("NumberOfNotifications", numberOfNotifications);

            return View(await _context.CompanyPushNotification.Where(m => m.CompanyID == _user.CompanyID).OrderByDescending(m => m.SentAt).ToListAsync());
        }

        
        public async Task<IActionResult> Notifications()
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            var notifications = _dla.GetNotificationsMatchingUserFilters(_user);

            _user.LastViewedNotificationsOn = System.DateTime.Now;
            _context.Update(_user);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32("NumberOfNotifications", 0);
            return View(notifications);
        }

        // GET: CompanyPushNotifications/Create
        public IActionResult Create()
        {
            _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
            _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();
            CompanyPushNotificationViewModel s = new CompanyPushNotificationViewModel();
            
            s.SentAt = DateTime.Now;            
            s.CompanyID = _user.CompanyID;
            s.UserID = _user.ID;
            s.SenderName = _user.Name;     

            DataAccessLayer dla = new DataAccessLayer(_context);
            string deptID = _user.Categories;           

            // Add group checkboxes
            var listOfAllGroups = dla.GetListGroups(_user.CompanyID);
            foreach (var seg in listOfAllGroups)
            {
                s.AvailableGroups.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add topic checkboxes
            var listOfAllTopics = dla.GetListTopics(_user.CompanyID);
            foreach (var seg in listOfAllTopics)
            {
                s.AvailableTopics.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add type checkboxes
            var listOfAllTypeOfUser = dla.GetListTypeOfUser(_user.CompanyID);
            foreach (var seg in listOfAllTypeOfUser)
            {
                s.AvailableTypeOfUser.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Add language checkboxes
            var listOfAllLanguages = dla.GetListLanguages(_user.CompanyID, _rlo);
            foreach (var seg in listOfAllLanguages)
            {
                s.AvailableLanguages.Add(new SelectListItem { Text = seg.Value, Value = seg.Value });
            }

            // Get location dropdown data
            s.Countries = dla.GetCountries();
            s.States = dla.GetNullStatesOrCities();
            s.Cities = dla.GetNullStatesOrCities();
            return View(s);
        }

        // POST: CompanyPushNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyPushNotificationViewModel cpnVM)
        {
            if (ModelState.IsValid)
            {
                var cpn = new CompanyPushNotification();
                _user = _context.User.FirstOrDefault(p => p.EmailAddress == _userManager.GetUserAsync(User).GetAwaiter().GetResult().Email);
                _cpy = _context.Company.FirstOrDefaultAsync(m => m.ID == _user.CompanyID).GetAwaiter().GetResult();

                //MessageTitle,MessageBody,MessageType,SentAt,SenderName,Languages,Groups,Topics,TypeOfUser,Country,State,City
                cpn.MessageTitle = cpnVM.MessageTitle;
                cpn.MessageType = cpnVM.MessageType;
                cpn.MessageBody = cpnVM.MessageBody;
                
                cpn.SenderName = _user.Name;
                cpn.SentAt = DateTime.Now;
                cpn.CompanyID = _user.CompanyID;
                cpn.UserID = _user.ID;               

                cpn.Languages = string.Join("|", cpnVM.SelectedLanguages);
                cpn.Groups = string.Join("|", cpnVM.SelectedGroups);
                cpn.Topics = string.Join("|", cpnVM.SelectedTopics);
                cpn.TypeOfUser = string.Join("|", cpnVM.SelectedTypeOfUser);
                if (cpnVM.Country != null && cpnVM.Country != "") cpn.Country = _context.LocationCountries.FirstOrDefault(m => m.country_code == cpnVM.Country).country_name;
                if (cpnVM.State != null && cpnVM.State != "") cpn.State = _context.LocationStates.FirstOrDefault(n => n.country_code == cpnVM.Country && n.state_code == cpnVM.State).state_name;
                cpn.City = cpnVM.City;

                _context.Add(cpn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(cpnVM);
        }

        

        private bool CompanyPushNotificationExists(int id)
        {
            return _context.CompanyPushNotification.Any(e => e.ID == id);
        }
    }
}
