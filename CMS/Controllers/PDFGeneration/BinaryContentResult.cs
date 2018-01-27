using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers.PDFGeneration
{
    public class BinaryContentResult : ActionResult
    {
        private readonly string contentType;
        private readonly byte[] contentBytes;
        private readonly string filename;
        public BinaryContentResult(byte[] contentBytes, string contentType,string FileName)
        {
            this.contentBytes = contentBytes;
            this.contentType = contentType;
            this.filename = FileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = this.contentType;
            response.AppendHeader("Content-Disposition", "form-data; filename=" + this.filename + ".pdf");
            using (var stream = new MemoryStream(this.contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }
    }
}