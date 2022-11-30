using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace TicketGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CreateTicket();
        }

        private static void CreateTicket()
        {
            PdfDocument PDFDoc = PdfReader.Open(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\TicketTemplate.pdf", PdfDocumentOpenMode.Import);
            PdfDocument PDFNewDoc = new PdfDocument();

            for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
            {
                PdfPage pp = PDFNewDoc.AddPage(PDFDoc.Pages[Pg]);

                XGraphics gfx = XGraphics.FromPdfPage(pp);
                XFont font = new XFont("Arial", 10, XFontStyle.Regular);
                gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, pp.Width, pp.Height), XStringFormats.BottomCenter);
            }

            PDFNewDoc= CreateAndSetQRBitmap(PDFNewDoc, "CS0231");
            PDFNewDoc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Test2.pdf");
        }

        private static PdfDocument CreateAndSetQRBitmap(PdfDocument pdf, string qrData)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(10);


            PdfPage page = pdf.Pages[0]; //I will add it to 1st page        // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XImage image = XImage.FromGdiPlusImage(qrCodeImage); //you can use XImage.FromGdiPlusImage to get the bitmap object as image (not a stream)
            gfx.DrawImage(image, 50, 50, 150, 150);
            //save your pdf, dispose other objects

            return pdf;
        }

        //private static string GenerateQRCode(string data)
        //{
        //    string qrcode = string.Empty;
        //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //    QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        //    QRCode qrCode = new QRCode(qrCodeData);

        //    using (Bitmap bitMap = qrCode.GetGraphic(40))
        //    {
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            byte[] byteImage = ms.ToArray();
        //            qrcode = "data:image/png;base64," + Convert.ToBase64String(byteImage);
        //        }

        //    }
        //    return qrcode;
        //}


    }
}
