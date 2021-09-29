using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace Scanner.Services
{
    static class PdfCreator
    {
        public static byte[] CreatePdf(IEnumerable<Scanner.Models.Image> images)
        {
            PdfDocument document = new PdfDocument();
            foreach (var image in images)
            {
                PdfPage page = document.AddPage();
                XGraphics graphics = XGraphics.FromPdfPage(page);
                XImage xImage = XImage.FromStream(() => new MemoryStream(image.Data));
                page.Height = xImage.PixelHeight;
                page.Width = xImage.PixelWidth;
                //double x = (250 - xImage.PixelWidth * 72 / xImage.HorizontalResolution) / 2;
                graphics.DrawImage(xImage, 0, 0);
                graphics.Save();
                page.Close();
            }
            document.Close();
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
    }
}
