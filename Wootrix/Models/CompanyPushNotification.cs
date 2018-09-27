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
        public int ID { get; set; }

        public int CompanyID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public string Options { get; set; }
        public DateTime SentAt { get; set; }
    }
}
