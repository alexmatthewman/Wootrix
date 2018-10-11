using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class SegmentArticleSegments
    {
        [Key]
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public int SegmentArticleID { get; set; }

        public int CompanySegmentID { get; set; }
    }
}
