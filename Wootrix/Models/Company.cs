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
        public int ID { get; set; }

        [Required]
        [Display(Name = "Company Name", Prompt = "Enter the Company name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Company Logo Upload", Prompt = "Please select a logo to upload - less than 500px wide and 100px high")]
        public string CompanyLogoUrl { get; set; }

        [Display(Name = "Company Message", Prompt = "You can set an optional message for all company users to see")]
        public string CompanyMessage { get; set; }

        [Display(Name = "Company Primary Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        public string CompanyPrimaryHighlightColor { get; set; }

        [Display(Name = "Company Secondary Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        public string CompanySecondaryHighlightColor { get; set; }

        [Display(Name = "Company Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        public string CompanyBackgroundColor { get; set; }

        [Display(Name = "Company Header Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
        public string CompanyHeaderBackgroundColor { get; set; }
    }

    //public class CompanyFileUpload
    //{
    //    [Required]
    //    [Display(Name = "Company Name", Prompt = "Enter the Company name")]
    //    public string CompanyName { get; set; }

    //    [Required]
    //    [Display(Name = "Company Logo Upload", Prompt = "Please select a logo to upload - less than 500px wide and 100px high")]
    //    public IFormFile CompanyLogoUrl { get; set; }

    //    [Display(Name = "Company Message", Prompt = "You can set an optional message for all company users to see")]
    //    public string CompanyMessage { get; set; }

    //    [Display(Name = "Company Primary Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
    //    public string CompanyPrimaryHighlightColor { get; set; }

    //    [Display(Name = "Company Secondary Highlight Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
    //    public string CompanySecondaryHighlightColor { get; set; }

    //    [Display(Name = "Company Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
    //    public string CompanyBackgroundColor { get; set; }

    //    [Display(Name = "Company Header Background Color", Prompt = "Must be a hexidecimal color in the format #FFFFFF to #000000")]
    //    public string CompanyHeaderBackgroundColor { get; set; }       
    //}
}
