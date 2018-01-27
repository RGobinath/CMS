using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace CMS.Controllers.PDFGeneration
{
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 50;
        private const int VerticalMargin = 25;

        public byte[] Render(string htmlText, string pageTitle, string orientation)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                try
                {
                    if (orientation == "portrait")
                    {
                        using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                        {
                            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                            pdfWriter.CloseStream = false;
                            pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                            pdfDocument.Open();
                            using (var htmlViewReader = new StringReader(htmlText))
                            {
                                using (var htmlWorker = new HTMLWorker(pdfDocument))
                                {
                                    htmlWorker.Parse(htmlViewReader);
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var pdfDocument = new Document(PageSize.A4.Rotate(), HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                        {
                            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                            pdfWriter.CloseStream = false;
                            pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                            pdfDocument.Open();
                            using (var htmlViewReader = new StringReader(htmlText))
                            {
                                //StyleSheet Style = new StyleSheet();
                                //HTMLTagProcessors tag;
                                //Dictionary<string, object> Provider = new Dictionary<string, object>();
                                //foreach (IElement el in iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(htmlViewReader, Style))
                                //{

                                // if (el is PdfPTable)
                                // {
                                // //((PdfPTable)el).HeaderRows = 1;
                                // //if ((PdfPTable)el == 791.00)
                                // if (((iTextSharp.text.pdf.PdfPTable)(el)).TotalWidth == 790.0)
                                // {
                                // ((PdfPTable)el).HeaderRows = 1;
                                // }
                                // }
                                // pdfDocument.Add(el);
                                //}
                                using (var htmlWorker = new HTMLWorkerExtended(pdfDocument))
                                {
                                    htmlWorker.Parse(htmlViewReader);
                                }
                            }
                        }
                    }
                    renderedBuffer = new byte[outputMemoryStream.Position];
                    outputMemoryStream.Position = 0;
                    outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
                }
                catch (System.Exception ex)
                {

                    throw ex;
                }
            }
            return renderedBuffer;
        }
        public class HTMLWorkerExtended : HTMLWorker
        {
            public HTMLWorkerExtended(IDocListener document)
                : base(document)
            {

            }
            public override void StartElement(string tag, IDictionary<string, string> str)
            {
                if (tag.Equals("newpage"))
                    document.Add(Chunk.NEXTPAGE);
                else
                    base.StartElement(tag, str);
            }
        }

    }
}