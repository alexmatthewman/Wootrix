using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class SegmentArticleComment
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public int SegmentArticleID { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please only enter a string")]
        [StringLength(1000)]
        [Display(Name = "Comment", Prompt = "Enter your comment here", Description = "Comment")]
        public string Comment { get; set; }

        [ScaffoldColumn(false)]
        public int UserID { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [ScaffoldColumn(false)]
        public string status { get; set; }
    }
}
