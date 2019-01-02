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

        [Display(Name = "Order")]
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

        [Display(Name = "Embedded Video File", Prompt = "Please enter a video to embed in the article if applicable", Description = "Embedded Video File")]
        public string EmbeddedVideo { get; set; }

        [StringLength(10000)]
        [Display(Name = "Article Content", Prompt = "Please enter the article content", Description = "Article Content")]
        public string ArticleContent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}")]
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

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [Display(Name = "Segments which have this Article", Prompt = "Segments which have this Article", Description = "Segments which have this Article")]
        public string Segments { get; set; }

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

    public class SegmentArticleViewModel
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [Display(Name = "Order")]
        public int? Order { get; set; }

        [Display(Name = "Article Url", Prompt = "Please enter the article url if applicable", Description = "Article Url")]
        public string ArticleUrl { get; set; }

        [Required]
        [StringLength(1000)]
        [RegularExpression(@"^[^\|]+$", ErrorMessage = "Please no | characters")]
        [Display(Name = "Title", Prompt = "Please enter the title", Description = "Title")]
        public string Title { get; set; }
        
        [Display(Name = "Cover Image", Prompt = "Please select the cover image", Description = "Cover Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Upload Video", Prompt = "Please enter the video url if applicable", Description = "Embedded Video Url")]
        public IFormFile EmbeddedVideo { get; set; }

        [StringLength(10000)]
        [Display(Name = "Article Content", Prompt = "Please enter the article content", Description = "Article Content")]
        public string ArticleContent { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}")]
        [Display(Name = "Publish Date", Prompt = "When it goes public", Description = "Publish Date")]
        public DateTime? PublishFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy MMM dd}")]
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

        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }

        [Display(Name = "Article appears in these segments", Prompt = "Article appears in these segments", Description = "Article appears in these segments")]
        public string Segments { get; set; }
        public IList<string> SelectedSegments { get; set; }
        public IList<SelectListItem> AvailableSegments { get; set; }

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

        public SegmentArticleViewModel()
        {
            SelectedSegments = new List<string>();
            AvailableSegments = new List<SelectListItem>();
            
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

        //// This property contains the available options
        //public SelectList AvailableSegments { get; set; }

        //// This property contains the selected options
        //public IEnumerable<string> SelectedSegments { get; set; }

        //public SegmentArticleViewModel()
        //{
        //    AvailableSegments = new SelectList(
        //        new[] { "Google", "TV", "Radio", "A friend", "Crazy bloke down the pub" });

        //    SelectedSegments = new[] { "Google" };
        //}

        public int? ArticleApprovedCommentCount { get; set; }
    }
}
