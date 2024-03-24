using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

public class PdfHelper
{
    public static void CreatePdf(string outputPath, string[] lines, string imagePath = null)
    {
        string backgroundImagePath = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\canvaBack.png";

        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Arial", 12);
        XFont headerFont = new XFont("Arial", 25);

        if (File.Exists(backgroundImagePath))
        {
            XImage backgroundImage = XImage.FromFile(backgroundImagePath);
            gfx.DrawImage(backgroundImage, 0, 0, page.Width, page.Height);
        }

        //Temel bilgiler
        DrawBasedInfo(gfx, headerFont, lines);

        //// İletişim bilgileri
        //DrawContactInformation(gfx, font, lines);

        //// Yetenekler
        //DrawSkills(gfx, font, lines);

        //// Diller
        //DrawLanguages(gfx, font, lines);

        //// Özet
        //DrawSummary(gfx, font, lines);

        //// Eğitim bilgileri
        //DrawEducation(gfx, font, lines);

        //// Deneyim bilgileri
        //DrawJobExperience(gfx, font, lines);

        //// Projeler
        //DrawProjects(gfx, font, lines);

        //// Sertifikalar
        //DrawCertificates(gfx, font, lines);

        //// Hobiler
        //DrawHobbies(gfx, font, lines);

        // Resim ekleme
        if (imagePath != null && File.Exists(imagePath))
        {
            DrawImage(gfx, page, imagePath);
        }

        document.Save(outputPath);
        document.Close();
    }

    private static void DrawBasedInfo(XGraphics gfx, XFont headerFont, string[] lines)
    {
        double startX = 70;
        double startY = 42;

        // Kare boyutları
        double squareWidth = 360;
        double squareHeight = 110;

        // Ad soyadı yerleştirin
        double textX = (squareWidth / 2+startX);
        double textY = (squareHeight / 2 + (startY/2)); // Kare yüksekliğinin çeyreği kadar aşağıda
        gfx.DrawString(lines[0].ToUpper(), headerFont, XBrushes.Black, textX, textY, XStringFormats.Center);

        // İkinci satırı yerleştirin
        textY += (squareHeight / 3); // Kare yüksekliğinin yarısı kadar daha aşağıda
        gfx.DrawString(lines[1].ToUpper(), new XFont("Arial", 20), XBrushes.Black, textX, textY, XStringFormats.Center);
    }

    private static void DrawContactInformation(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("İletişim", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        for (int i = 3; i < 9; i++)
        {
            gfx.DrawString(lines[i], font, XBrushes.Black, 40, startY);
            startY += lineHeight;
        }
    }

    private static void DrawSkills(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Yetenekler", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        for (int i = 9; i < 12; i++)
        {
            gfx.DrawString(lines[i], font, XBrushes.Black, 40, startY);
            startY += lineHeight;
        }
    }

    private static void DrawLanguages(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Diller", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        gfx.DrawString(lines[12], font, XBrushes.Black, 40, startY);
    }

    private static void DrawSummary(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Özet", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        gfx.DrawString(lines[13], font, XBrushes.Black, 40, startY);
    }

    private static void DrawEducation(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Eğitim", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        gfx.DrawString(lines[14], font, XBrushes.Black, 40, startY);
    }

    private static void DrawJobExperience(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Deneyim", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        gfx.DrawString(lines[15], font, XBrushes.Black, 40, startY);
    }

    private static void DrawProjects(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Projeler", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        // Projeler
        for (int i = 16; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Projeler"))
                continue;

            gfx.DrawString(lines[i], font, XBrushes.Black, 40, startY);
            startY += lineHeight;
        }
    }

    private static void DrawCertificates(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Sertifikalar", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        // Sertifikalar
        for (int i = 16; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Sertifikalar"))
                continue;

            if (lines[i].StartsWith("Projeler"))
                break;

            gfx.DrawString(lines[i], font, XBrushes.Black, 40, startY);
            startY += lineHeight;
        }
    }

    private static void DrawHobbies(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 40;
        double lineHeight = font.GetHeight();

        gfx.DrawString("Hobiler", font, XBrushes.Black, 40, startY);
        startY += lineHeight;

        gfx.DrawString(lines[lines.Length - 1], font, XBrushes.Black, 40, startY);
    }

    private static void DrawImage(XGraphics gfx, PdfPage page, string imagePath)
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
}
