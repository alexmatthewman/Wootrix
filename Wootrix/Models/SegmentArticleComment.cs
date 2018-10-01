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

        public int SegmentArticleID { get; set; }
        public string Comment { get; set; }
        public int UserID { get; set; }        
        public DateTime CreatedDated { get; set; }
        public string status { get; set; }
    }
}
