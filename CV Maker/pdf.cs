using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;

public class PdfHelper
{
    public static void CreatePdf(string outputPath, string[] lines, string imagePath = null)
    {
        // Öncelikle Canva tasarımının arka planını temsil eden resmi alın
        string backgroundImagePath = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\canvaBack.png";

        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Arial", 12);

        // Arka planı ekleme
        if (File.Exists(backgroundImagePath))
        {
            XImage backgroundImage = XImage.FromFile(backgroundImagePath);
            gfx.DrawImage(backgroundImage, 0, 0, page.Width, page.Height);
        }

        // Metni ve resmi ekleme (mevcut kod)
        double textStartingY = 40; // Metnin başlangıç yüksekliği
        XRect textRect = new XRect(40, textStartingY, page.Width.Point - 80, page.Height.Point - 80);
        XStringFormat format = XStringFormats.TopLeft;

        foreach (string line in lines)
        {
            gfx.DrawString(line, font, XBrushes.Black, textRect, format);
            textRect = new XRect(textRect.Left, textRect.Top + font.GetHeight(), textRect.Width, textRect.Height);
        }

        if (imagePath != null && File.Exists(imagePath))
        {
            XImage image = XImage.FromFile(imagePath);
            double targetWidth = page.Width.Point / 3.65;
            double targetHeight = targetWidth + 2.5;
            double x = page.Width.Point - targetWidth - 7;
            double y = 13.5;

            XGraphicsPath path = new XGraphicsPath();
            double centerX = x + targetWidth / 6;
            double centerY = y + targetHeight / 6;
            double radius = targetWidth / 6;
            path.AddEllipse(centerX - radius, centerY - radius, targetWidth, targetHeight);
            gfx.IntersectClip(path);
            gfx.DrawImage(image, x, y, targetWidth, targetHeight);
        }

        // PDF'yi kaydetme ve kapatma
        document.Save(outputPath);
        document.Close();
    }
}
