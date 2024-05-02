using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Drawing;
using System.IO;

public class PdfHelper
{
    public static void CreatePdf(string outputPath, string[] lines, string imagePath = null)
    {
        string backgroundImagePath = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\canvaBack.png";
        string backgroundImagePathSecond = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\secondPage.png";

        PdfDocument document = new PdfDocument();

        // Birinci sayfa
        PdfPage page1 = document.AddPage();
        XGraphics gfx1 = XGraphics.FromPdfPage(page1);
        XFont font = new XFont("Arial", 12);
        XFont headerFont = new XFont("Arial", 25);

        //Arkaplan yerleştirme
        if (File.Exists(backgroundImagePath))
        {
            XImage backgroundImage = XImage.FromFile(backgroundImagePath);
            gfx1.DrawImage(backgroundImage, 0, 0, page1.Width, page1.Height);
        }

        // Temel bilgiler
        DrawBasedInfo(gfx1, headerFont, lines);

        // Özet
        DrawSummary(gfx1, font, lines);

        // Resim ekleme
        if (imagePath != null && File.Exists(imagePath))
        {
            DrawImage(gfx1, page1, imagePath);
        }

        // İkinci sayfa
        PdfPage page2 = document.AddPage();
        XGraphics gfx2 = XGraphics.FromPdfPage(page2);

        //Arkaplan yerleştirme
        if (File.Exists(backgroundImagePath))
        {
            XImage backgroundImage = XImage.FromFile(backgroundImagePathSecond);
            gfx2.DrawImage(backgroundImage, 0, 0, page2.Width, page2.Height);
        }

        // Yetenek seviyesi
        DrawSkillsLevel(gfx2, font, lines);

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

        startY += 15;

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
        startY = 200;

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

        DrawJobExperience(gfx, font, lines, startY, rectWidth);
    }


    private static void DrawJobExperience(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        startY += 30;

        // "Eğitim" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("İş", new XFont("Arial", 25));
        double textX = 247 + (rectWidth - textSize.Width) / 2;
        gfx.DrawString("İş", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
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

        DrawProjects(gfx, font, lines, startY, rectWidth);
    }

    private static void DrawProjects(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        startY += 40;

        // "Projeler" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("Projeler", new XFont("Arial", 25));
        double textX = 247 + (rectWidth - textSize.Width) / 2;
        gfx.DrawString("Projeler", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;
        startY += 10; // Çizginin hemen altına geçmek için

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 257, startY, 257 + rectWidth - 20, startY); // Alt çizgi

        startY += 20; // Çizginin hemen altına geçmek için

        // Projeleri çizin
        string projectLine = lines[13].Trim(); // Projelerin tamamını al, başındaki ve sonundaki boşlukları temizle
        XSize projectSize = gfx.MeasureString(projectLine, font); // Projelerin boyutunu ölç

        double currentX = 257; // Başlangıçta projelerin yazılacağı x konumu
        double currentY = startY; // Başlangıçta projelerin yazılacağı y konumu

        string[] words = projectLine.Split(' '); // Projeleri kelimelere ayır
        foreach (string word in words)
        {
            XSize wordSize = gfx.MeasureString(word + " ", font); // Kelimenin boyutunu ve bir boşluk ekleyin
            if (currentX + wordSize.Width > 257 + rectWidth - 20) // Taşma kontrolü
            {
                currentX = 257; // X konumunu başlangıç noktasına döndür
                currentY += 15; // Y konumunu bir alt satıra taşı
            }

            if (currentX > 257) // Eğer bu kelimenin başı değilse virgül ekle
            {
                gfx.DrawString(" ", font, XBrushes.Black, currentX, currentY);
                currentX += gfx.MeasureString(" ", font).Width; // X konumunu güncelle
            }

            gfx.DrawString(word + " ", font, XBrushes.Black, currentX, currentY); // Kelimeyi yazdır
            currentX += wordSize.Width; // X konumunu güncelle
        }

        startY = currentY+10; // Yüksekliği en son satırın altına ayarla

        DrawCertificates(gfx, font, lines, startY, rectWidth);
    }

    private static void DrawCertificates(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        startY += 40;

        // "Sertifikalar" metnini genişliğe göre tam ortala
        string certificateLine = lines[14].Trim(); // Projelerin tamamını al, başındaki ve sonundaki boşlukları temizle
        XSize certificateSize = gfx.MeasureString(certificateLine, font);
        double textX = 257 + (rectWidth - certificateSize.Width) / 2;
        gfx.DrawString("Sertifikalar", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;
        startY += 5; // Çizginin hemen altına geçmek için
        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPenGauzy = new XPen(XColor.FromArgb(100, 40, 27, 20), 3);
        dashedPenGauzy.DashStyle = XDashStyle.Dash;

        // Kesikli çizgiyi çizin
        gfx.DrawLine(dashedPen, 257, startY, 257 + rectWidth - 20, startY); // Alt çizgi

        startY += 20; // Çizginin hemen altına geçmek için

        // Sertifikaları çizin
        string[] words = certificateLine.Split(','); // Virgüle göre ayır

        foreach (string word in words)
        {
            string trimmedWord = word.Trim(); // Boşlukları temizle
            XSize wordSize = gfx.MeasureString(trimmedWord, font);
            double wordX = 257 + (rectWidth - wordSize.Width) / 2;
            gfx.DrawString(trimmedWord, font, XBrushes.Black, wordX, startY); // Kelimeyi yazdır
            startY += 5;
            // Kesikli çizgiyi çizin
            gfx.DrawLine(dashedPenGauzy, 300, startY, 257 + rectWidth - 60, startY); // Alt çizgi
            startY += wordSize.Height + 5; // Y konumunu güncelle
        }
    }

    private static void DrawSkillsLevel(XGraphics gfx, XFont font, string[] lines)
    {
        double startY = 50;

        // "Yetenek Seviyesi" metnini genişliğe göre tam ortala
        XSize textSize = gfx.MeasureString("Yetenek Seviyesi", new XFont("Arial", 25));
        double rectWidth = 600;
        double textX = (rectWidth - textSize.Width) / 2;
        gfx.DrawString("Yetenek Seviyesi", new XFont("Arial", 25), XBrushes.Black, textX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;
        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPenGauzy = new XPen(XColor.FromArgb(100, 40, 27, 20), 3);
        dashedPenGauzy.DashStyle = XDashStyle.Dash;

        // Kesikli çizgiyi çizin
        XRect rect = new XRect(20, startY - 25, rectWidth - 40, 35);
        gfx.DrawRoundedRectangle(dashedPen, rect, new XSize(10, 10)); // Yumuşak köşeli dikdörtgen çiz

        startY += 40; // Çizginin hemen altına geçmek için

        // Başlık yazdır ve ölçüsünü al
        XSize levelTextSize1 = gfx.MeasureString("İleri Seviye", new XFont("Arial", 20));
        gfx.DrawLine(dashedPenGauzy, 40, startY + 10, levelTextSize1.Width + 60, startY + 10); // 1. çizgi

        // Başlık yazdır ve ölçüsünü al
        XSize levelTextSize2 = gfx.MeasureString("Orta Seviye", new XFont("Arial", 20));
        gfx.DrawLine(dashedPenGauzy, levelTextSize1.Width + 140, startY + 10, levelTextSize1.Width + levelTextSize2.Width + 160, startY + 10); // 2. çizgi

        // Başlık yazdır ve ölçüsünü al
        XSize levelTextSize3 = gfx.MeasureString("Temel Seviye", new XFont("Arial", 20));
        gfx.DrawLine(dashedPenGauzy, levelTextSize1.Width + levelTextSize2.Width + 220, startY + 10, levelTextSize1.Width + levelTextSize2.Width + levelTextSize3.Width + 250, startY + 10); // 3. çizgi


        double startX1 = (rectWidth / 3 - levelTextSize1.Width) / 2;
        double startX2 = rectWidth / 3 + (rectWidth / 3 - levelTextSize2.Width) / 2;
        double startX3 = 2 * rectWidth / 3 + (rectWidth / 3 - levelTextSize3.Width) / 2;

        gfx.DrawString("İleri Seviye", new XFont("Arial", 20), XBrushes.Black, startX1, startY);
        gfx.DrawString("Orta Seviye", new XFont("Arial", 20), XBrushes.Black, startX2, startY);
        gfx.DrawString("Temel Seviye", new XFont("Arial", 20), XBrushes.Black, startX3, startY);

        startY += 15; // Yetenek seviyelerinin altına geçmek için
        double textXLeft = 30;
        double textXMiddle = rectWidth / 3 + (rectWidth / 3 - 120) / 2;
        double textXRight = 2 * rectWidth / 3 + (rectWidth / 3 - 140) / 2;
        // Yetenek seviyelerini yazdır
        for (int i = 15; i <= 17; i++)
        {
            string skillLevel = lines[i];
            // İleri seviye
            if (i == 15)
            {
                startY += 20;
                DrawSkillLevel(gfx, font, skillLevel, textXLeft, startY, rectWidth / 3 - 40);
            }
            // Orta seviye
            else if (i == 16)
            {
                startY += 20;
                DrawSkillLevel(gfx, font, skillLevel, textXMiddle, startY, rectWidth / 3 - 40);
            }            // Başlangıç seviyesi
            else if (i == 17)
            {
                startY += 20;
                DrawSkillLevel(gfx, font, skillLevel, textXRight, startY, rectWidth / 3 - 40);
            }
        }

        startY += 120;

        DrawSkillsStar(gfx, font, lines, startY, rectWidth);
    }

    private static double DrawSkillLevel(XGraphics gfx, XFont font, string skillLevel, double textX, double startY, double maxWidth)
    {
        string[] words = skillLevel.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        double currentY = startY;
        string line = "";

        foreach (string word in words)
        {
            string trimmedWord = word.Trim(); // Kelimenin başındaki ve sonundaki boşlukları kaldır
            string testLine = (line + trimmedWord).Trim(); // Boşlukları temizle ve test satırına ekle
            XSize size = gfx.MeasureString(testLine, font);

            if (size.Width < maxWidth)
            {
                line = testLine + ", ";
            }
            else
            {
                gfx.DrawString(line.TrimEnd(',', ' '), font, XBrushes.Black, textX, currentY);
                currentY += font.Height;
                line = trimmedWord + ", "; // Yeni satır için kelimeyi ve virgülü ekle
            }
        }

        // Son kalan satırı çiz
        string lastLine = line.TrimEnd(',', ' ');
        if (!string.IsNullOrEmpty(lastLine))
        {
            gfx.DrawString(lastLine, font, XBrushes.Black, textX, currentY);
            currentY += font.Height;
        }

        return currentY;
    }
    private static void DrawSkillsStar(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        // Yetenek isimlerini birleştir
        string skillName = lines[8] + "," + lines[9];

        // Metni genişliğe göre tam ortala
        XSize titleSize = gfx.MeasureString("Yetenek Puanları", new XFont("Arial", 25));
        double titleX = (rectWidth - titleSize.Width) / 2;
        gfx.DrawString("Yetenek Puanları", new XFont("Arial", 25), XBrushes.Black, titleX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        // Kesikli çizgiyi çizin
        XRect rect = new XRect(20, startY - 25, rectWidth - 40, 35);
        gfx.DrawRoundedRectangle(dashedPen, rect, new XSize(10, 10)); // Yumuşak köşeli dikdörtgen çiz

        double currentX = 40; // Başlangıçta yazılacağı x konumu

        // Yetenek adını ve puanını çizdir
        string[] skillNames = skillName.Trim().Split(',');
        for (int i = 0; i < skillNames.Length; i++)
        {
            if (i % 3 == 0)
            {
                startY += 50;
                currentX = 40;
            }
            gfx.DrawString(skillNames[i].Trim(), new XFont("Arial", 12), XBrushes.Black, currentX, startY);

            // Yıldızları yerine koymak için StarReplacer fonksiyonunu çağır
            StarReplacer(Convert.ToInt32(lines[18][i].ToString()), gfx, currentX, startY + 10);

            currentX += 200; // İleriye kaydır
        }
        startY += 70;
        DrawHobbies(gfx, font, lines, startY, rectWidth);
    }

    private static void StarReplacer(int index, XGraphics gfx, double x, double y)
    {
        string imagePath = "C:\\Users\\melih\\OneDrive\\Belgeler\\Visual Studio 2022\\Code Snippets\\Visual C#\\My Code Snippets\\CV Maker\\CV Maker\\Backgrounds\\star.png"; // Star resminin yolunu belirt
        XImage starImage = XImage.FromFile(imagePath);

        switch (index)
        {
            case 1:
                gfx.DrawImage(starImage, x, y, 15, 15);
                break;
            case 2:
                gfx.DrawImage(starImage, x, y, 15, 15);
                gfx.DrawImage(starImage, x+=15, y, 15, 15);
                break;
            case 3:
                gfx.DrawImage(starImage, x, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                break;
            case 4:
                gfx.DrawImage(starImage, x, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                break;
            case 5:
                gfx.DrawImage(starImage, x, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                gfx.DrawImage(starImage, x += 15, y, 15, 15);
                break;
            default:
                break;
        }
    }



    private static void DrawHobbies(XGraphics gfx, XFont font, string[] lines, double startY, double rectWidth)
    {
        // "Hobiler" metnini genişliğe göre tam ortala
        XSize titleSize = gfx.MeasureString("Hobiler", new XFont("Arial", 25));
        double titleX = (rectWidth - titleSize.Width) / 2;
        gfx.DrawString("Hobiler", new XFont("Arial", 25), XBrushes.Black, titleX, startY);

        // Alt çizgi çizmek için DashStyle belirleyin
        XPen dashedPen = new XPen(XColor.FromArgb(40, 27, 20), 3);
        dashedPen.DashStyle = XDashStyle.Dash;

        // Kesikli çizgiyi çizin
        XRect rect = new XRect(20, startY - 25, rectWidth - 40, 35);
        gfx.DrawRoundedRectangle(dashedPen, rect, new XSize(10, 10)); // Yumuşak köşeli dikdörtgen çiz

        startY += 40; // Başlığın hemen altına geçmek için

        // Hobileri yazdır
        string hobbiesLine = lines[19].Trim(); // Hobileri al, başındaki ve sonundaki boşlukları temizle
        XSize hobbiesSize = gfx.MeasureString(hobbiesLine, font); // Hobilerin boyutunu ölç

        double currentX = 40; // Başlangıçta hobilerin yazılacağı x konumu
        double currentY = startY; // Başlangıçta hobilerin yazılacağı y konumu

        string[] words = hobbiesLine.Split(' '); // Hobileri kelimelere ayır
        foreach (string word in words)
        {
            XSize wordSize = gfx.MeasureString(word + " ", font); // Kelimenin boyutunu ve bir boşluk ekleyin
            if (currentX + wordSize.Width > rectWidth - 40) // Taşma kontrolü
            {
                currentX = 40; // X konumunu başlangıç noktasına döndür
                currentY += 20; // Y konumunu bir alt satıra taşı
            }

            if (currentX > 40) // Eğer bu kelimenin başı değilse bir boşluk ekle
            {
                gfx.DrawString(" ", font, XBrushes.Black, currentX, currentY);
                currentX += gfx.MeasureString(" ", font).Width; // X konumunu güncelle
            }

            gfx.DrawString(word + " ", font, XBrushes.Black, currentX, currentY); // Kelimeyi yazdır
            currentX += wordSize.Width; // X konumunu güncelle
        }
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
