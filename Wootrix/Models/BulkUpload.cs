using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Models
{
    public class BulkUploadViewModel
    {

        [Required]
        [Display(Name = "Bulk Upload File", Prompt = "Please upload a csv file in the format of the example file above", Description = "Bulk Upload File")]
        public IFormFile BulkUploadFile { get; set; }


    }

    public class BulkUploadDataViewModel
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string WebsiteLanguage { get; set; }
               
        public string Topics { get; set; }

        public string Groups { get; set; }

        public string TypeOfUser { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }
    }




}
