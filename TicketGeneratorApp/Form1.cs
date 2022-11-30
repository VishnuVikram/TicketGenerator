using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicketGeneratorApp.DAL;
using TicketGeneratorApp.Models;
using ZXing.QrCode;

namespace TicketGeneratorApp
{
    public partial class Form1 : Form
    {
        PdfPage page;
        PdfDocument PDFDoc = null;
        PdfDocument document = new PdfDocument();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnHrdCopy_Click(object sender, EventArgs e)
        {
            int itemcount = 0;
            foreach (var emp in GetAllEmployees())
            {
                CreateTicket(emp, itemcount); ;
                itemcount = itemcount == 4 ? 0 : ++itemcount;

            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            PDFDoc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\TicketsHardCopy.pdf");
        }


        private void btnSftCopy_Click(object sender, EventArgs e)
        {
            foreach (var emp in GetAllEmployees())
            {
                CreateSoftTicket(emp);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                PDFDoc.Save(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\Ticket " + emp.EmpCode.ToLower() + ".pdf");
            }
        }

        #region HardCopy

        private void CreateTicket(Employee employee, int itemcount)
        {
            BuildTicket(itemcount == 0 ? true : false, itemcount, document, employee.EmpCode.ToUpper());

            int ticketHeight = 216 * itemcount;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Neo Sans Std", 8.2, XFontStyle.Bold);
            gfx.DrawString(employee.Name.ToUpper(), font, XBrushes.White, 85, ticketHeight + 170);

            font = new XFont("Neo Sans Std", 8.2, XFontStyle.Regular);
            gfx.DrawString(employee.EmpCode.ToUpper(), font, XBrushes.White, 85, ticketHeight + 182.5);

            font = new XFont("Neo Sans Std", 10, XFontStyle.Bold);
            gfx.DrawString(employee.Name.ToUpper(), font, XBrushes.Black, 505, ticketHeight + 135, XStringFormat.TopCenter);

            font = new XFont("Neo Sans Std", 9, XFontStyle.Regular);
            gfx.DrawString(employee.EmpCode.ToUpper(), font, XBrushes.Black, 505, ticketHeight + 150.5, XStringFormat.TopCenter);

            font = new XFont("Neo Sans Std", 14, XFontStyle.Bold);
            gfx.DrawString(employee.NoOfAttendiees, font, XBrushes.Black, 274, ticketHeight + 171);

            font = new XFont("Neo Sans Std", 7.5, XFontStyle.Regular);
            gfx.DrawString("MEMBERS", font, XBrushes.Black, 265, ticketHeight + 182.5);

            font = new XFont("OCR A Extended", 17, XFontStyle.Regular);
            gfx.RotateAtTransform(270, new XPoint(30, ticketHeight + 187.5));
            gfx.DrawString(employee.TicketNo, font, XBrushes.White, 30, ticketHeight + 187.5);

            gfx.Dispose();

            //++itemcount;

        }

        private void BuildTicket(bool isCreatePage, int row, PdfDocument document, string empCode)
        {

            // Create an empty page or load existing
            if (isCreatePage)
            {
                page = document.AddPage();
                page.Size = PdfSharp.PageSize.A3;
            }

            // Get an XGraphics object for drawing
            int ticketHeight = 216 * row;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawImage(gfx, ReadImage(TicketType.HardCopy), 0, ticketHeight, 576, 216);
            Bitmap qr = CreateQRBitmap(empCode.ToUpper());
            gfx.DrawImage(qr, 332, ticketHeight + 114, 76.79, 76.79);
            gfx.DrawImage(qr, 465, ticketHeight + 50, 75.79, 75.79);

            gfx.Dispose();

            PDFDoc = document;
        }

        #endregion

        #region Soft Copy

        private void CreateSoftTicket(Employee emp)
        {
            PDFDoc = BuildSoftTicket(new PdfDocument(), emp);
        }

        private PdfDocument BuildSoftTicket(PdfDocument document, Employee emp)
        {
            page = document.AddPage();
            page.Size = PdfSharp.PageSize.GovernmentLetter;

            int ticketHeight = 216;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawImage(gfx, ReadImage(TicketType.SoftCopy), 90, 10, 405, 720);

            Bitmap qr = CreateQRBitmap(emp.EmpCode.ToUpper());
            gfx.DrawImage(qr, 240, ticketHeight + 261, 103.79, 103.79);

            XFont font = new XFont("Neo Sans Std", 20, XFontStyle.Bold);
            gfx.DrawString(emp.Name.ToUpper(), font, XBrushes.White, 287, ticketHeight + 198, XStringFormat.TopCenter);

            font = new XFont("Neo Sans Std", 16, XFontStyle.Regular);
            gfx.DrawString(emp.EmpCode.ToUpper(), font, XBrushes.White, 293, ticketHeight + 226, XStringFormat.TopCenter);

            font = new XFont("Neo Sans Std", 16, XFontStyle.Bold);
            gfx.DrawString(emp.NoOfAttendiees + " MEMBERS", font, XBrushes.White, 289, ticketHeight + 380.5, XStringFormat.TopCenter);


            gfx.Dispose();
            return document;
        }

        #endregion

        #region Image Handle

        private Bitmap CreateQRBitmap(string qrData)
        {

            var width = 250; // width of the Qr Code
            var height = 250; // height of the Qr Code
            var margin = 0;
            var qrCodeWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            var pixelData = qrCodeWriter.Write(qrData);

            Bitmap bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
            }
            return bitmap;
        }

        private void DrawImage(XGraphics gfx, Bitmap img, int x, int y, int width, int height)
        {
            XImage image = XImage.FromGdiPlusImage(img);
            gfx.DrawImage(image, x, y, width, height);
        }

        private Bitmap GetBitmapImage(byte[] fileImg)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(fileImg))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        private Bitmap ReadImage(TicketType type)
        {
            byte[] imgdata = null;

            switch (type)
            {
                case TicketType.SoftCopy:
                    imgdata = System.IO.File.ReadAllBytes(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\SoftTicketTemplate.png");
                    break;
                case TicketType.HardCopy:
                    imgdata = System.IO.File.ReadAllBytes(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\TicketTemplate.png");
                    break;
                default:
                    break;
            }
            return GetBitmapImage(imgdata);
        }

        #endregion

        #region Dummy Data

        private List<Employee> GetAllEmployees()
        {
            return SQLConnector.GetEmployees();
            //return new List<Employee>()
            //{
            //new Employee(){ID = 1, EmpCode = "CS0231",FirstName = "Vishnu", LastName = "VIKRAMAN", NoOfAttendiees = "2"},
            //new Employee(){ID = 2, EmpCode = "CS0550",FirstName = "Vaisakh", LastName = "R", NoOfAttendiees = "4"},
            //new Employee(){ID = 3, EmpCode = "CS0927",FirstName = "Anamika", LastName = "Menon", NoOfAttendiees = "1"},
            //new Employee(){ID = 4, EmpCode = "CS0374",FirstName = "VISHNU", LastName = "Bose", NoOfAttendiees = "1"},
            //new Employee(){ID = 5, EmpCode = "CS0372",FirstName = "Naveen", LastName = "Davis", NoOfAttendiees = "2"},
            //new Employee(){ID = 11, EmpCode = "CS0142",FirstName = "Suneer", LastName = "P.A", NoOfAttendiees = "2"},
            //new Employee(){ID = 12, EmpCode = "CS0346",FirstName = "Parthiv", LastName = "Ravi", NoOfAttendiees = "3"},
            //new Employee(){ID = 13, EmpCode = "CS018",FirstName = "Akhil", LastName = "NP", NoOfAttendiees = "2"},
            //new Employee(){ID = 14, EmpCode = "CS0168",FirstName = "Binoj", LastName = "V", NoOfAttendiees = "2"},
            //new Employee(){ID = 15, EmpCode = "CS0056",FirstName = "Muhammed", LastName = "Shafeek", NoOfAttendiees = "1"},
            //new Employee(){ID = 111, EmpCode = "CS0583",FirstName = "Aby", LastName = "Varghese", NoOfAttendiees = "1"},
            //new Employee(){ID = 112, EmpCode = "CS1332",FirstName = "Akhil", LastName = "Krishna", NoOfAttendiees = "2"},
            //new Employee(){ID = 113, EmpCode = "CS1312",FirstName = "JiJo", LastName = "Joseph", NoOfAttendiees = "2"},
            //new Employee(){ID = 114, EmpCode = "CS1193",FirstName = "Neethu", LastName = "M", NoOfAttendiees = "2"},
            //new Employee(){ID = 115, EmpCode = "CS0781",FirstName = "Josmy", LastName = "Joseph", NoOfAttendiees = "`"}
            //};
        }

        #endregion
    }

    public enum TicketType
    {
        SoftCopy,
        HardCopy
    }
}
