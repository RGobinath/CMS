﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers.PDFGeneration
{
    /// <summary>
    /// Extends the controller with functionality for rendering PDF views
    /// </summary>
    public class PdfViewController : BaseController
    {
        public HtmlViewRenderer htmlViewRenderer;
        public StandardPdfRenderer standardPdfRenderer;

        public PdfViewController()
        {
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        public ActionResult ViewPdf(string pageTitle, string viewName, object model, string orientation,string FileName)
        {
            // Render the view html to a string.
            string htmlText = this.htmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle, orientation);

            // Return the PDF as a binary stream to the client.
            //return new BinaryContentResult(buffer, "application/pdf");
            return new BinaryContentResult(buffer, "application/pdf", FileName);
        }
    }
}