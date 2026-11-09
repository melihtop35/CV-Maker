# CV Maker

Türkçe ve İngilizce arayüz seçenekleri sunan, kişisel bilgilerinizi düzenleyip iki sayfalı bir CV'yi PDF olarak oluşturmanıza yardımcı olan Windows masaüstü uygulaması.

## Özellikler

- Türkçe veya İngilizce arayüz seçimi
- Kişisel bilgiler, iletişim bilgileri ve özet ekleme
- Eğitim ve iş deneyimi kayıtlarını ekleme ve düzenleme
- Yetenekleri seviye gruplarına ayırma ve yıldızla puanlama
- Diller, projeler, sertifikalar ve hobiler ekleme
- İsteğe bağlı profil fotoğrafı seçme
- CV'yi PDF dosyası olarak dışa aktarma

## Gereksinimler

- Windows
- Visual Studio (Windows Forms ve .NET Framework geliştirme araçlarıyla)
- .NET Framework 4.8 Developer Pack

Proje, C# ile geliştirilmiş bir Windows Forms uygulamasıdır ve `CV Maker/CV Maker.csproj` dosyasında .NET Framework 4.8 hedeflenir.

## Kurulum ve çalıştırma

1. Depoyu klonlayın:

   ```bash
   git clone https://github.com/melihtop35/CV-Maker.git
   ```

2. `CV Maker.sln` çözüm dosyasını Visual Studio'da açın.
3. NuGet paketlerinin geri yüklenmesini bekleyin veya çözüm menüsünden **Restore NuGet Packages** seçeneğini çalıştırın.
4. Başlangıç projesi olarak **CV Maker**'ı seçip uygulamayı çalıştırın.

Gerekli NuGet paketleri proje yapılandırmasında tanımlıdır: Newtonsoft.Json ve PDFsharp.

## Kullanım

1. Uygulamayı açıp arayüz dilini seçin.
2. Formdaki alanları doldurun; eğitim ve iş deneyimi kayıtlarını ekleyin.
3. Yeteneklerinizi ve seviyelerini seçin, ardından dil ve diğer CV bilgilerini girin.
4. İsterseniz profil fotoğrafı ekleyin.
5. PDF oluşturma düğmesine basıp kaydetme konumunu seçin.

## Proje yapısı

- `CV Maker/` — Windows Forms arayüzü, uygulama kodu ve kaynak dosyaları
- `CV Maker/Backgrounds/` — PDF şablonlarında kullanılan arka plan ve yıldız görselleri
- `CV Maker/translations.json` — Türkçe ve İngilizce arayüz metinleri
- `CV Maker.sln` — Visual Studio çözüm dosyası

## Teknolojiler

- C#
- Windows Forms
- .NET Framework 4.8
- PDFsharp
- Newtonsoft.Json

## Lisans

Depoda bir lisans dosyası bulunmuyor. Kullanım ve dağıtım koşulları için depo sahibine danışın.
