using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class PdfHelper
{
    public static void CreatePdf(string outputPath, string[] lines, string imagePath = null)
    {
        string backgroundImagePath = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\canvaBack.png";

        PdfDocument document = new PdfDocument();
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        //Genel font
        XFont font = new XFont("Arial", 12);
        //Başlık fontu. Başka bir yerde kullanılmıyor
        XFont headerFont = new XFont("Arial", 25);

        //Arkaplan yerleştirme
        if (File.Exists(backgroundImagePath))
        {
            XImage backgroundImage = XImage.FromFile(backgroundImagePath);
            gfx.DrawImage(backgroundImage, 0, 0, page.Width, page.Height);
        }

        //Temel bilgiler
        DrawBasedInfo(gfx, headerFont, lines);

        // Özet
        DrawSummary(gfx, font, lines);

        //// Yetenekler
        //DrawSkills(gfx, font, lines);

        //// Diller
        //DrawLanguages(gfx, font, lines);

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
        double textX = (squareWidth / 2 + startX);
        double textY = (squareHeight / 2 + (startY / 2)); // Kare yüksekliğinin çeyreği kadar aşağıda
        gfx.DrawString(lines[0].ToUpper(), headerFont, XBrushes.Black, textX, textY, XStringFormats.Center);

        // İkinci satırı yerleştirin
        textY += (squareHeight / 3); // Kare yüksekliğinin yarısı kadar daha aşağıda
        gfx.DrawString(lines[1].ToUpper(), new XFont("Arial", 20), XBrushes.Black, textX, textY, XStringFormats.Center);
    }
    private static void DrawSummary(XGraphics gfx, XFont font, string[] lines)
    {
        double rectWidth = 246;
        double rectHeight = 688;
        double startY = 180 + rectHeight / 15;

        // Kareyi çizin
        gfx.DrawRectangle(XPens.Blue, 0, 155, rectWidth, rectHeight);
        // Sınırları çizin
        gfx.DrawRectangle(XPens.Purple, 0, 172 + rectHeight / 15, rectWidth, rectHeight / 3);

        // Kesikli çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        gfx.DrawLine(dashedPen, 10, 170 + rectHeight / 15, rectWidth - 10, 170 + rectHeight / 15); // Alt çizgi

        // "Özet" metnini çizin
        XSize summaryTextSize = gfx.MeasureString("Özet", new XFont("Arial", 25));
        double summaryTextX = (rectWidth - summaryTextSize.Width) / 2;
        gfx.DrawString("Özet", new XFont("Arial", 25), XBrushes.Black, summaryTextX, 201);

        // Metni belirli bir genişlikte wrap yaparak çizin
        XRect summaryRect = new XRect(10, 172 + rectHeight / 15, rectWidth - 20, rectHeight / 3); // Metin sınırları

        string summary = lines[2];
        XStringFormat format = XStringFormats.TopLeft;

        // Metni kelime bazında bölmek için boşluklara göre ayır
        string[] words = summary.Split(' ');

        double currentY = startY;
        string line = "";

        foreach (string word in words)
        {
            // Mevcut satıra kelimeyi ekleyerek uzunluğunu kontrol et
            string tempLine = line + word + " ";
            XSize textSize = gfx.MeasureString(tempLine, font);

            if (textSize.Width <= summaryRect.Width)
            {
                // Mevcut satıra sığarsa, kelimeyi satıra ekle
                line = tempLine;
            }
            else
            {
                // Mevcut satıra sığmazsa, mevcut satırı çizin ve yeni bir satıra geç
                gfx.DrawString(line, font, XBrushes.Black, summaryRect.X, currentY, format);
                currentY += font.GetHeight();
                line = word + " ";
            }
        }

        // Son satırı çizin
        gfx.DrawString(line, font, XBrushes.Black, summaryRect.X, currentY, format);

        // Özet metnin bitiş yüksekliğini döndür
        double summaryEndY = currentY + font.GetHeight() + 40;

        // İletişim bilgilerini çizin
        DrawContactInformation(gfx, font, lines, summaryEndY);
    }

    private static void DrawContactInformation(XGraphics gfx, XFont font, string[] lines, double startY)
    {
        double rectWidth = 246;
        double lineHeight = font.GetHeight();
        double iconSpacing = 10; // İkonlar arası boşluk
        double maxTextWidth = 155; // Maksimum metin genişliği

        // "İletişim" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("İletişim", new XFont("Arial", 25));
        double textX = (rectWidth - textSize.Width) / 2;
        gfx.DrawString("İletişim", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Kesikli çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        startY += 20; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 10, startY, rectWidth - 10, startY); // Alt çizgi

        startY += 20; // Bilgilerin hemen altına geçmek için

        // Icons klasöründen ikonları çekip çizim yapma
        string[] icons = new string[] { "icons8-phone-50.png", "icons8-mail-50.png", "icons8-linkedin-50.png", "icons8-github-50.png", "icons8-location-50.png" }; // İkon dosya adları

        for (int i = 3; i <= 7; i++)
        {
            // İkonları çekme işlemi
            string iconPath = Path.Combine("Icons", icons[i - 3]); // İkonun dosya yolunu oluştur
            XImage icon = XImage.FromFile(iconPath); // İkonu yükle

            // İkonun çizim yapma
            double iconHeight = lineHeight + 5; // İkonun yüksekliği satır yüksekliğine eşit olacak
            double iconWidth = iconHeight * icon.PixelWidth / icon.PixelHeight; // İkonun genişliği oranlı olarak ayarlanacak

            // İkonun çizimi
            gfx.DrawImage(icon, 10, startY, iconWidth, iconHeight);

            // İkonun sağında boşluk bırakma
            double iconRight = 10 + iconWidth + iconSpacing;

            // Metni yazma işlemi
            string currentLine = lines[i];
            double currentX = iconRight;
            double remainingWidth = maxTextWidth + 55;

            if (i == 7)
            {
                // lines[7] için özel wrap işlemi
                string[] words = currentLine.Split(' ');
                string lineToDraw = "";
                foreach (string word in words)
                {
                    string tempLine = lineToDraw + " " + word;
                    if (gfx.MeasureString(tempLine, font).Width > remainingWidth)
                    {
                        gfx.DrawString(lineToDraw.Trim(), font, XBrushes.Black, currentX, startY + 12.5);
                        startY += lineHeight;
                        lineToDraw = word;
                    }
                    else
                    {
                        lineToDraw = tempLine;
                    }
                }
                if (!string.IsNullOrEmpty(lineToDraw))
                {
                    gfx.DrawString(lineToDraw.Trim(), font, XBrushes.Black, currentX, startY + 12.5);
                }
            }

            else
            {
                // Diğer satırlar için standart wrap işlemi
                while (gfx.MeasureString(currentLine, font).Width > remainingWidth)
                {
                    int lengthToFit = (int)(currentLine.Length * remainingWidth / gfx.MeasureString(currentLine, font).Width);
                    string lineToDraw = currentLine.Substring(0, lengthToFit);
                    gfx.DrawString(lineToDraw, font, XBrushes.Black, currentX, startY + 12.5);
                    currentLine = currentLine.Substring(lengthToFit);
                    currentX = 10;
                    startY += lineHeight;
                    remainingWidth = maxTextWidth + 50;
                }

                gfx.DrawString(currentLine, font, XBrushes.Black, iconRight, startY + 12.5);
            }

            // Bir sonraki satıra geçme
            startY += lineHeight + 10;
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
