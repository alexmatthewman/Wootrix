using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class SegmentArticle
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        public int? Order { get; set; }

        [Display(Name = "Article Url", Prompt = "Please enter the article url if applicable", Description = "Article Url")]
        public string ArticleUrl { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Title", Prompt = "Please enter the title", Description = "Title")]
        public string Title { get; set; }

        [StringLength(1000)]
        [Display(Name = "Cover Image", Prompt = "Please select the cover image", Description = "Cover Image")]
        public string Image { get; set; }

        [Display(Name = "Embedded Video Url", Prompt = "Please enter the video url if applicable", Description = "Embedded Video Url")]
        public string EmbeddedVideo { get; set; }

        [Required]
        [StringLength(10000)]
        [Display(Name = "Article Content", Prompt = "Please enter the article content", Description = "Article Content")]
        public string ArticleContent { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Finish Date", Prompt = "When it stops displaying", Description = "Finish Date")]
        public DateTime? PublishTill { get; set; }

        [StringLength(10000)]
        [Display(Name = "Tags for search", Prompt = "Please enter Tags for search if you want them", Description = "Tags for search")]
        public string Tags { get; set; }

        [Display(Name = "Allow Comments")]
        public bool? AllowComments { get; set; }        

        //In case they want to override the company settings for a magazine
        [StringLength(1000)]
        [Display(Name = "Publisher Name", Prompt = "Will default to creator user name", Description = "Publisher Name")]
        public string Author { get; set; }
    }

    public class SegmentArticleViewModel
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        public int? Order { get; set; }

        [Display(Name = "Article Url", Prompt = "Please enter the article url if applicable", Description = "Article Url")]
        public string ArticleUrl { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Title", Prompt = "Please enter the title", Description = "Title")]
        public string Title { get; set; }
        
        [Display(Name = "Cover Image", Prompt = "Please select the cover image", Description = "Cover Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Embedded Video Url", Prompt = "Please enter the video url if applicable", Description = "Embedded Video Url")]
        public IFormFile EmbeddedVideo { get; set; }

        [Required]
        [StringLength(10000)]
        [Display(Name = "Article Content", Prompt = "Please enter the article content", Description = "Article Content")]
        public string ArticleContent { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Finish Date", Prompt = "When it stops displaying", Description = "Finish Date")]
        public DateTime? PublishTill { get; set; }

        [StringLength(10000)]
        [Display(Name = "Tags for search", Prompt = "Please enter Tags for search if you want them", Description = "Tags for search")]
        public string Tags { get; set; }

        [Display(Name = "Allow Comments")]
        public System.Boolean? AllowComments { get; set; }

        //In case they want to override the company settings for a magazine
        [StringLength(1000)]
        [Display(Name = "Publisher Name", Prompt = "Will default to creator user name", Description = "Publisher Name")]
        public string Author { get; set; }
    }
}
