using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WootrixV2.Models
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "File Upload")]
        public IFormFile UploadFile { get; set; }

    }
}
