using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Configuration;

namespace CMS.Utilities
{
    public class PdfUtility
    {
        // Create a simple Pdf document and add an image to it.
        public static MemoryStream GetSimplePdf(MemoryStream chartImage)
        {
            const int documentMargin = 10;

            var pdfStream = new MemoryStream();
            var pdfDocument = new Document(PageSize.LETTER);
            pdfDocument.SetMargins(documentMargin, documentMargin, documentMargin, documentMargin);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, pdfStream);

            Image image = Image.GetInstance(chartImage.GetBuffer());
            image.SetAbsolutePosition(documentMargin
                , pdfDocument.PageSize.Height - documentMargin - image.ScaledHeight);

            pdfDocument.Open();
            
            PdfPTable nace = new PdfPTable(4);
            float[] width = new float[] { 50f, 245f, 200f, 55f };
            nace.SetWidths(width);
            /// pdf image Sample
            /// 
            iTextSharp.text.Image LogonaceImage;
            string LogonaceImagePath = ConfigurationManager.AppSettings["AppLogos"] + "logonace.jpg";

            LogonaceImage = iTextSharp.text.Image.GetInstance(LogonaceImagePath);
           // LogonaceImage.ScaleAbsolute(55, 55);

            iTextSharp.text.Image LogoImage;
            string ImagePath = ConfigurationManager.AppSettings["RptCard"] + "logo.jpg";

            LogoImage = iTextSharp.text.Image.GetInstance(ImagePath);
           // LogoImage.ScaleAbsolute(55, 55);


            /// 



            PdfPCell nace1 = new PdfPCell(new Phrase("nace logo", new iTextSharp.text.Font(FontFactory.GetFont("Verdana", 10.0f, iTextSharp.text.Font.NORMAL))));
            nace1.Border = 0;
            nace1.AddElement(LogonaceImage);
            nace.AddCell(nace1);
            pdfDocument.NewPage();
            pdfDocument.Add(nace);
            pdfDocument.NewPage();
            pdfDocument.Add(image);

            pdfDocument.Close();
            pdfWriter.Flush();

            return pdfStream;
        }
    }
}