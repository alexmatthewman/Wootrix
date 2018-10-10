using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WootrixV2.Data
{
    public static class FormFileHelper
    {
        public static IFormFile PhysicalToIFormFile(this FileInfo physicalFile)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            FormFile tempFile = new FormFile(ms, 0, ms.Length, physicalFile.Name, physicalFile.FullName);

            return tempFile;
        }
    }
}
