using System.Collections.Generic;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;

namespace Server.Services
{
    internal class Pdf
    {
        public static byte[] CreatePdf(IEnumerable<Models.Image> images)
        {
            PdfDocument document = new PdfDocument();
            foreach (var image in images)
            {
                PdfPageBase page = document.Pages.Add(PdfPageSize.A4, new PdfMargins(0));
                PdfImage pdfImage = PdfImage.FromStream(new MemoryStream(image.Data));
                page.Canvas.DrawImage(pdfImage, 0, 0, page.ActualSize.Width, page.ActualSize.Height);
            }
            MemoryStream stream = new MemoryStream();
            document.SaveToStream(stream);
            document.Close();
            return stream.ToArray();
        }
    }
}
