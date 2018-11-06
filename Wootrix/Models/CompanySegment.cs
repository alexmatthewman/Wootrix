using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{

    

    public class CompanySegment
    {
       

        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        public int? Order { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Title", Prompt = "Please enter the title", Description = "Title")]
        public string Title { get; set; }
            
        [Required]
        [StringLength(1000)]
        [Display(Name = "Cover Image", Prompt = "Please select the cover image", Description = "Cover Image")]
        public string CoverImage { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Cover Image Mobile Friendly", Prompt = "Please keep the file size small", Description = "Mobile Friendly")]
        public string CoverImageMobileFriendly { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Finish Date", Prompt = "When it stops displaying", Description = "Finish Date")]
        public DateTime? FinishDate { get; set; }

        //In case they want to override the company settings for a magazine
        [Display(Name = "Publisher Name", Prompt = "Will default to publisher name", Description = "Publisher Name")]
        public string ClientName { get; set; }

        [Display(Name = "Publisher Avatar Image", Prompt = "Will default to publisher image", Description = "Publisher Avatar Image")]
        public string ClientLogoImage { get; set; }

        [Display(Name = "Theme Colour", Prompt = "Default are the company colours", Description = "Theme Colour")]
        public string ThemeColor { get; set; }

        [Display(Name = "Standard Colour", Prompt = "Default are the company colours", Description = "Standard Colour")]
        public string StandardColor { get; set; }

        [ScaffoldColumn(false)]
        public bool Draft { get; set; }

        [Display(Name = "Limit editors to this Department", Prompt = "Limit editors to this Department", Description = "Limit editors to this Department")]
        public string Department { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tags", Prompt = "Comma delimit multiple tags", Description = "Tags")]
        public string Tags { get; set; }
    }


    public class CompanySegmentViewModel
    {


        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        public int? Order { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Title", Prompt = "Please enter the title", Description = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Cover Image", Prompt = "Please select the cover image", Description = "Cover Image")]
        public IFormFile CoverImage { get; set; }

        [Required]
        [Display(Name = "Cover Image Mobile Friendly", Prompt = "Please keep the file size small", Description = "Mobile Friendly")]
        public IFormFile CoverImageMobileFriendly { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Finish Date", Prompt = "When it stops displaying", Description = "Finish Date")]
        public DateTime? FinishDate { get; set; }

        //In case they want to override the company settings for a magazine
        [Display(Name = "Publisher Name", Prompt = "Will default to publisher name", Description = "Publisher Name")]
        public string ClientName { get; set; }

        [Display(Name = "Publisher Avatar Image", Prompt = "Will default to publisher image", Description = "Publisher Avatar Image")]
        public IFormFile ClientLogoImage { get; set; }

        [Display(Name = "Theme Colour", Prompt = "Default are the company colours", Description = "Theme Colour")]
        public string ThemeColor { get; set; }

        [Display(Name = "Standard Colour", Prompt = "Default are the company colours", Description = "Standard Colour")]
        public string StandardColor { get; set; }

        [ScaffoldColumn(false)]
        public bool Draft { get; set; }

        [Display(Name = "Limit editors to this Department", Prompt = "Limit editors to this Department", Description = "Limit editors to this Department")]
        public string Department { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }

        [StringLength(1000)]
        [Display(Name = "Tags", Prompt = "Comma delimit multiple tags", Description = "Tags")]
        public string Tags { get; set; }


        //public CompanySegmentViewModel()
        //{
        //    Department = new List<string>();
           
        //}

    }
}
