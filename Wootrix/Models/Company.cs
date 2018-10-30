using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class Company
    {
        [Key]
        public int ID { get; set; }

        [Required]        
        [StringLength(100)]
        [Display(Name = "Company Name", Prompt = "Enter the Company name", Description = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Company Logo", Prompt = "Please select a logo to upload - less than 500px wide and 100px high", Description = "Company Logo")]
        public string CompanyLogoImage { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Background Color")]
        public string CompanyBackgroundColor { get; set; }

        [Display(Name = "Company Background Image", Prompt = "Please select a background to upload - best size is 1920px wide and 1080px high", Description = "Company Background Image")]
        public string CompanyBackgroundImage { get; set; }

        [Display(Name = "Company Focus Image", Prompt = "Please select a focus image to upload - best size is 700px wide and 550px high", Description = "Company Focus Image")]
        public string CompanyFocusImage { get; set; }

        [StringLength(1000, ErrorMessage = "Please keep the max length to 1000")]
        [Display(Name = "Company Text Main", Prompt = "You can set an optional main message for all company users to see", Description = "Company Text Main")]
        public string CompanyTextMain { get; set; }


        [StringLength(5000, ErrorMessage = "Please keep the max length to 5000")]
        [Display(Name = "Company Text Secondary", Prompt = "You can set an optional message for all company users to see", Description = "Company Text Secondary")]
        public string CompanyTextSecondary { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Highlight Color")]
        public string CompanyHighlightColor { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Header Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Header Background Color")]
        public string CompanyHeaderBackgroundColor { get; set; }

        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Main Font Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Main Font Color")]
        public string CompanyMainFontColor { get; set; }

        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Header Font Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Header Font Color")]
        public string CompanyHeaderFontColor { get; set; }

        [Required]
        [Display(Name = "Number of Allowed Users", Prompt = "-1 for infinite or specify", Description = "Number of Allowed Users")]
        public int CompanyNumberOfUsers { get; set; }

        [Required]
        [Display(Name = "Number of Push Notifications", Prompt = "-1 for infinite or specify", Description = "Number of Push Notifications")]
        public int CompanyNumberOfPushNotifications { get; set; }

        public List<CompanyGroups> CompanyGroups { get; set; }

        public List<CompanyLanguages> CompanyLanguages { get; set; }

        public List<CompanyLocCountries> CompanyLocCountries { get; set; }
        public List<CompanyLocStates> CompanyLocStates { get; set; }
        public List<CompanyLocCities> CompanyLocCities { get; set; }

        public List<CompanyTopics> CompanyTopics { get; set; }

        public List<CompanySegment> CompanySegment { get; set; }

    }


    public class CompanyViewModel
    {

        [Required]        
        [StringLength(100)]
        [Display(Name = "Company Name", Prompt = "Enter the Company name", Description = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Company Logo", Prompt = "Please select a logo to upload - less than 500px wide and 100px high", Description = "Company Logo")]
        public IFormFile CompanyLogoImage { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Background Color")]
        public string CompanyBackgroundColor { get; set; }

        [Display(Name = "Company Background Image", Prompt = "Please select a background to upload - best size is 1920px wide and 1080px high", Description = "Company Background Image")]
        public IFormFile CompanyBackgroundImage { get; set; }

        [Display(Name = "Company Focus Image", Prompt = "Please select a focus image to upload - best size is 700px wide and 550px high", Description = "Company Focus Image")]
        public IFormFile CompanyFocusImage { get; set; }

        [StringLength(1000, ErrorMessage = "Please keep the max length to 1000")]
        [Display(Name = "Company Text Main", Prompt = "You can set an optional main message for all company users to see", Description = "Company Text Main")]
        public string CompanyTextMain { get; set; }

        [StringLength(5000, ErrorMessage = "Please keep the max length to 5000")]
        [Display(Name = "Company Text Secondary", Prompt = "You can set an optional message for all company users to see", Description = "Company Text Secondary")]
        public string CompanyTextSecondary { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Highlight Color")]
        public string CompanyHighlightColor { get; set; }

        [Required]
        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Header Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Header Background Color")]
        public string CompanyHeaderBackgroundColor { get; set; }

        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Main Font Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Main Font Color")]
        public string CompanyMainFontColor { get; set; }

        [RegularExpression(@"^#(?:[0-9a-fA-F]{3}){1,2}$", ErrorMessage = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        [StringLength(7, MinimumLength = 4)]
        [Display(Name = "Company Font Header Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000", Description = "Company Header Font Color")]
        public string CompanyHeaderFontColor { get; set; }



        [Required]
        [Display(Name = "Number of Allowed Users", Prompt = "-1 for infinite or specify", Description = "Number of Allowed Users")]
        public int CompanyNumberOfUsers { get; set; }

        [Required]
        [Display(Name = "Number of Push Notifications", Prompt = "-1 for infinite or specify", Description = "Number of Push Notifications")]
        public int CompanyNumberOfPushNotifications { get; set; }

    }




}
