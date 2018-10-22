using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class SegmentArticleComment
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public int SegmentArticleID { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Comment", Prompt = "Enter your comment here", Description = "Comment")]
        public string Comment { get; set; }

        [ScaffoldColumn(false)]
        public string UserID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Date of comment")]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Date of last edit")]
        public DateTime? EditDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Comment Status")]
        public string Status { get; set; }
    }

    public class SegmentArticleCommentViewModel
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int CompanyID { get; set; }

        [ScaffoldColumn(false)]
        public int SegmentArticleID { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Comment", Prompt = "Enter your comment here", Description = "Comment")]
        public string Comment { get; set; }

        [ScaffoldColumn(false)]
        public string UserID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Date of comment")]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Date of last edit")]
        public DateTime? EditDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Comment Status")]
        public string Status { get; set; }
        public IEnumerable<SelectListItem> StatusOptions { get; set; }
    }
}
