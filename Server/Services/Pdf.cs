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
            document.Sections.Add();
            foreach (var image in images)
            {
                PdfPageBase page = document.Pages.Add();
                PdfImage pdfImage = PdfImage.FromStream(image.Stream.AsStream());
                page.Canvas.DrawImage(pdfImage, 0, 0, page.Size.Width, page.Size.Height);
            }
            document.Close();
            MemoryStream stream = new MemoryStream();
            document.SaveToStream(stream);
            return stream.ToArray();
        }
    }
}
