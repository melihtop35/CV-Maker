using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
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
        double startX = 68;
        double startY = 42;

        // Metni ölçmek için kullanılacak kare boyutları
        double squareWidth = 355;
        double squareHeight = 110;

        gfx.DrawRectangle(XPens.Black, startX, startY, squareWidth, squareHeight);
        // Ad soyadı yerleştirin
        XSize nameSize = gfx.MeasureString(lines[0].ToUpper(), headerFont);
        double textX = startX + (squareWidth - nameSize.Width) / 2;
        double textY = startY + (squareHeight - nameSize.Height) / 2; // Kare yüksekliğinin çeyreği kadar aşağıda
        gfx.DrawString(lines[0].ToUpper(), headerFont, XBrushes.Black, textX, textY);

        // İkinci satırı yerleştirin
        XFont secondLineFont = null;
        XSize secondLineSize = new XSize();
        double secondLineFontSize = 20;
        do
        {
            secondLineFont = new XFont("Arial", secondLineFontSize);
            secondLineSize = gfx.MeasureString(lines[1].ToUpper(), secondLineFont);
            secondLineFontSize -= 1; // Font boyutunu küçült
        } while (secondLineSize.Width > squareWidth && secondLineFontSize >= 10); // Kare içine sığana kadar veya font boyutu minimum 10 olana kadar
        textY += squareHeight / 3; // Kare yüksekliğinin yarısı kadar daha aşağıda
        textX = startX + (squareWidth - secondLineSize.Width) / 2;
        gfx.DrawString(lines[1].ToUpper(), secondLineFont, XBrushes.Black, textX, textY);

    }
    private static void DrawSummary(XGraphics gfx, XFont font, string[] lines)
    {
        double rectWidth = 246;
        double rectHeight = 688;
        double startY = 200;

        // "Özet" metnini çizin
        XSize summaryTextSize = gfx.MeasureString("Özet", new XFont("Arial", 25));
        double summaryTextX = (rectWidth - summaryTextSize.Width) / 2;
        gfx.DrawString("Özet", new XFont("Arial", 25), XBrushes.Black, summaryTextX, startY);

        startY += 10;
        // Kareyi çizin
        gfx.DrawRectangle(XPens.Blue, 0, 155, rectWidth, rectHeight);
        // Sınırları çizin
        gfx.DrawRectangle(XPens.Purple, 0, 172 + rectHeight / 15, rectWidth, rectHeight / 3);

        // Kesikli çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 10, startY, rectWidth - 10, startY); // Alt çizgi

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
        DrawContactInformation(gfx, font, lines, summaryEndY, rectWidth);
    }

    private static void DrawContactInformation(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
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

        startY += 10; // Çizginin hemen altına geçmek için

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
        // İletişim metnin bitiş yüksekliğini döndür
        double contactEndY = startY + font.GetHeight() + 20;

        //Önemli yetenekleri çizin
        DrawSkills(gfx, font, lines, contactEndY, rectWidth);
    }


    private static void DrawSkills(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        // "Yetenekler" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("Yetenekler", new XFont("Arial", 25));
        double textX = (rectWidth - textSize.Width) / 2;
        gfx.DrawString("Yetenekler", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Kesikli çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        startY += 10; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 10, startY, rectWidth - 10, startY); // Alt çizgi

        double lineHeight = font.GetHeight();

        startY += lineHeight;

        // Metni wrap ederek çizmek için XTextFormatter kullanın
        XTextFormatter tf = new XTextFormatter(gfx);
        XRect rect1 = new XRect(10, startY, rectWidth - 10, gfx.PageSize.Height - startY); // Metnin sığdırılacağı dikdörtgen alan
        tf.DrawString(lines[8], font, XBrushes.Black, rect1, XStringFormats.TopLeft);

        lineHeight = font.GetHeight();

        startY += lineHeight * 5;

        // "Ek Yetenekler" metnini genişliğe göre tam ortala
        XSize textSize1 = gfx.MeasureString("Ek Yetenekler", new XFont("Arial", 25));
        double textX1 = (rectWidth - textSize1.Width) / 2;
        gfx.DrawString("Ek Yetenekler", new XFont("Arial", 25), XBrushes.Black, textX1, startY);
        startY += 10;
        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 10, startY, rectWidth - 10, startY); // Alt çizgi

        startY += 10;

        XRect rect2 = new XRect(10, startY, rectWidth - 10, gfx.PageSize.Height - startY); // Metnin sığdırılacağı dikdörtgen alan
        tf.DrawString(lines[9], font, XBrushes.Black, rect2, XStringFormats.TopLeft);

        // Yetenekler metnin bitiş yüksekliğini döndür
        double skillsEndY = startY + font.GetHeight() + 20;

        DrawLanguages(gfx, font, lines, skillsEndY, rectWidth);
    }

    private static void DrawLanguages(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        startY += font.GetHeight() + 40;

        // "Diller" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("Diller", new XFont("Arial", 25));
        double textX = (rectWidth - textSize.Width) / 2;
        gfx.DrawString("Diller", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Kesikli çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        startY += 10; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 10, startY, rectWidth - 10, startY); // Alt çizgi

        startY += 20; // Çizginin hemen altına geçmek için

        // Dilleri alt alta yazdır
        for (int i = 0; i < lines[10].Split(',').Length; i++)
        {
            string language = lines[10].Split(',')[i].Trim();
            XSize languageSize = gfx.MeasureString(language, font);
            double languageX = (rectWidth - languageSize.Width) / 2;
            gfx.DrawString(language, font, XBrushes.Black, languageX, startY);
            startY += font.GetHeight() + 5; // Bir sonraki satırın altına geç
        }

        // Eğitim bilgileri
        DrawEducation(gfx, font, lines, 180);
    }

    private static void DrawEducation(XGraphics gfx, XFont font, string[] lines, double startY)
    {
        double rectWidth = 348;
        double rectHeight = 689;
        startY = 200;

        // Kareyi çizin
        gfx.DrawRectangle(XPens.Black, 247, 155, rectWidth, rectHeight);

        // "Eğitim" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("Eğitim", new XFont("Arial", 25));
        double textX = 247 + (rectWidth - textSize.Width) / 2;
        gfx.DrawString("Eğitim", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Yumuşak köşeli dikdörtgeni çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;
        startY += 10; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 257, startY, 257 + rectWidth - 20, startY); // Alt çizgi

        startY += 20; // Çizginin hemen altına geçmek için

        // Eğitim bilgilerini çizin
        string[] educationLines = lines[11].Split('\n');
        double[] lineY = new double[educationLines.Length]; // Satır yüksekliklerini saklamak için dizi
        double maxWidth = 0; // En geniş satırı saklamak için değişken
        bool firstLine = true; // İlk satırı işaretlemek için bir bayrak kullanıyoruz
        for (int i = 0; i < educationLines.Length; i++)
        {
            if (!firstLine)
            {
                XSize lineSize = gfx.MeasureString(educationLines[i], font);
                maxWidth = Math.Max(maxWidth, lineSize.Width);
                lineY[i] = lineSize.Height; // Satırın yüksekliğini sakla
            }
            else
            {
                double lineX = 247 + (rectWidth - maxWidth) / 2; // Metni tam ortaya hizala
                gfx.DrawString(educationLines[i], font, XBrushes.Black, lineX, startY);
                startY += lineY[i]; // Bir sonraki satıra geçmek için
                firstLine = false; // İlk satırı işaretleme bayrağını kapat
            }
        }

        // Eğitim bilgilerini dikdörtgene orantılı olarak çizin
        for (int i = 1; i < educationLines.Length; i++)
        {
            if (!firstLine)
            {
                double lineX = 247 + (rectWidth - maxWidth) / 2; // Metni tam ortaya hizala
                gfx.DrawString(educationLines[i], font, XBrushes.Black, lineX, startY);
                startY += lineY[i]; // Bir sonraki satıra geçmek için

                if (i % 2 == 0) // Okul adı yazıldıktan sonra
                {
                    startY += 15; // Bir satır boşluk bırak
                }
            }
        }

        DrawJobExperience(gfx, font, lines, startY,rectWidth,rectHeight);
    }


    private static void DrawJobExperience(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth,double rectHeight)
    {
        startY += 40;
        // Kareyi çizin
        gfx.DrawRectangle(XPens.Black, 247, 155, rectWidth, rectHeight);

        // "Eğitim" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("İş", new XFont("Arial", 25));
        double textX = 247 + (rectWidth - textSize.Width) / 2;
        gfx.DrawString("İş", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Yumuşak köşeli dikdörtgeni çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;
        startY += 10; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 257, startY, 257 + rectWidth - 20, startY); // Alt çizgi

        startY += 20; // Çizginin hemen altına geçmek için

        // Eğitim bilgilerini çizin
        string[] jobLines = lines[12].Split('\n');
        double[] lineY = new double[jobLines.Length]; // Satır yüksekliklerini saklamak için dizi
        double maxWidth = 0; // En geniş satırı saklamak için değişken
        bool firstLine = true; // İlk satırı işaretlemek için bir bayrak kullanıyoruz
        for (int i = 0; i < jobLines.Length; i++)
        {
            if (!firstLine)
            {
                XSize lineSize = gfx.MeasureString(jobLines[i], font);
                maxWidth = Math.Max(maxWidth, lineSize.Width);
                lineY[i] = lineSize.Height; // Satırın yüksekliğini sakla
            }
            else
            {
                double lineX = 247 + (rectWidth - maxWidth) / 2; // Metni tam ortaya hizala
                gfx.DrawString(jobLines[i], font, XBrushes.Black, lineX, startY);
                startY += lineY[i]; // Bir sonraki satıra geçmek için
                firstLine = false; // İlk satırı işaretleme bayrağını kapat
            }
        }

        // Eğitim bilgilerini dikdörtgene orantılı olarak çizin
        for (int i = 1; i < jobLines.Length; i++)
        {
            if (!firstLine)
            {
                double lineX = 247 + (rectWidth - maxWidth) / 2; // Metni tam ortaya hizala
                gfx.DrawString(jobLines[i], font, XBrushes.Black, lineX, startY);
                startY += lineY[i]; // Bir sonraki satıra geçmek için

                if (i % 2 == 0) // Okul adı yazıldıktan sonra
                {
                    startY += 15; // Bir satır boşluk bırak
                }
            }
        }
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
        double targetWidth = page.Width.Point / 3.62;
        double targetHeight = targetWidth + 2.5;
        double x = page.Width.Point - targetWidth - 6;
        double y = 12.8;

        XGraphicsPath path = new XGraphicsPath();
        double centerX = x + targetWidth / 6;
        double centerY = y + targetHeight / 6;
        double radius = targetWidth / 6;
        path.AddEllipse(centerX - radius, centerY - radius, targetWidth, targetHeight);
        gfx.IntersectClip(path);
        gfx.DrawImage(image, x, y, targetWidth, targetHeight);
    }
}
