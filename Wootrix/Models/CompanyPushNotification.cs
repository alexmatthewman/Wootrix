using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace WootrixV2.Models
{
    public class CompanyPushNotification
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Message Title")]
        public string MessageTitle { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Message Body")]
        public string MessageBody { get; set; }

        [Display(Name = "Message Type")]
        public string MessageType { get; set; }

        [Display(Name = "Sent At")]
        public DateTime SentAt { get; set; }

        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Display(Name = "Languages")]
        public string Languages { get; set; }

        [Display(Name = "User groups", Prompt = "User groups", Description = "User groups")]
        public string Groups { get; set; }

        [Display(Name = "User Topics", Prompt = "User Topics", Description = "User Topics")]
        public string Topics { get; set; }

        [Display(Name = "Type Of User", Prompt = "Type Of User", Description = "Type Of User")]
        public string TypeOfUser { get; set; }

        [Display(Name = "User Country", Prompt = "User Country", Description = "User Country")]
        public string Country { get; set; }

        [Display(Name = "State", Prompt = "State", Description = "State")]
        public string State { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public string City { get; set; }


        
    }

    public class CompanyPushNotificationViewModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        [Required]
        [Display(Name = "Message Title")]
        public string MessageTitle { get; set; }

        [Required]
        [Display(Name = "Message Body")]
        public string MessageBody { get; set; }

        [Display(Name = "Message Type")]
        public string MessageType { get; set; }

        [Display(Name = "Sent At")]
        public DateTime SentAt { get; set; }

        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Display(Name = "Languages")]
        public string Languages { get; set; }
        public IList<string> SelectedLanguages { get; set; }
        public IList<SelectListItem> AvailableLanguages { get; set; }

        [Display(Name = "User groups", Prompt = "User groups", Description = "User groups")]
        public string Groups { get; set; }
        public IList<string> SelectedGroups { get; set; }
        public IList<SelectListItem> AvailableGroups { get; set; }

        [Display(Name = "User Topics", Prompt = "User Topics", Description = "User Topics")]
        public string Topics { get; set; }
        public IList<string> SelectedTopics { get; set; }
        public IList<SelectListItem> AvailableTopics { get; set; }

        [Display(Name = "Type Of User", Prompt = "Type Of User", Description = "Type Of User")]
        public string TypeOfUser { get; set; }
        public IList<string> SelectedTypeOfUser { get; set; }
        public IList<SelectListItem> AvailableTypeOfUser { get; set; }

        [Display(Name = "User Country", Prompt = "User Country", Description = "User Country")]
        public string Country { get; set; }
        public IEnumerable<SelectListItem> Countries { get; set; }


        [Display(Name = "State", Prompt = "State", Description = "State")]
        public string State { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = "City", Prompt = "City", Description = "City")]
        public string City { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }



        public CompanyPushNotificationViewModel()
        {        
            SelectedGroups = new List<string>();
            AvailableGroups = new List<SelectListItem>();

            SelectedTopics = new List<string>();
            AvailableTopics = new List<SelectListItem>();

            SelectedTypeOfUser = new List<string>();
            AvailableTypeOfUser = new List<SelectListItem>();

            SelectedLanguages = new List<string>();
            AvailableLanguages = new List<SelectListItem>();

            Countries = new List<SelectListItem>();
            States = new List<SelectListItem>();
            Cities = new List<SelectListItem>();

        }
    }

    
}
