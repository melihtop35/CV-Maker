using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace CV_Maker
{
    public partial class Skill_Star : Form
    {
        // TrackBar değerlerini saklamak için bir dizi
        private List<int> trackBarValues;

        public Skill_Star()
        {
            InitializeComponent();
            InitializeTrackBars();
        }

        private void InitializeTrackBars()
        {
            try
            {
                string[] skills = GetSkillsFromForm1(); // Form1'den yetenekleri al

                int xPos = 40; // TrackBar'ların başlangıç X konumu
                int yPos = 20; // TrackBar'ların başlangıç Y konumu
                int count = 0; // Sayaç

                // TrackBar değerlerini saklamak için dizi oluştur
                trackBarValues = new List<int>();

                // Her yetenek için bir TrackBar oluştur
                foreach (string skill in skills)
                {
                    TrackBar trackBar = new TrackBar();
                    trackBar.Minimum = 1; // Minimum değer 1
                    trackBar.Maximum = 5; // Maksimum değer 5
                    trackBar.Value = 1; // Varsayılan değer 1
                    trackBar.Orientation = Orientation.Horizontal; // Yatay yönde oluştur
                    trackBar.AutoSize = true; // Otomatik boyutlandırma
                    trackBar.Location = new Point(xPos, yPos + 20); // Konum belirlenir
                    Controls.Add(trackBar); // Forma eklenir

                    // TrackBar'ın değerini sakla
                    trackBarValues.Add(trackBar.Value);

                    // TrackBar'ın ValueChanged olayına olay işleyici ekle
                    trackBar.ValueChanged += (sender, e) =>
                    {
                        TrackBar changedTrackBar = sender as TrackBar;
                        int index = Array.IndexOf(Controls.OfType<TrackBar>().ToArray(), changedTrackBar);
                        trackBarValues[index] = changedTrackBar.Value;
                    };

                    Label label = new Label();
                    label.Text = skill; // Yetenek adı atanır
                    label.Location = new Point(xPos, yPos); // Konum belirlenir
                    Controls.Add(label); // Forma eklenir

                    // X konumunu ve sayaç değerini güncelle
                    xPos += trackBar.Width + 50;
                    count++;

                    // Eğer TrackBar'lar formun genişliğini aşarsa yeni satıra geç
                    if (xPos + trackBar.Width > Width)
                    {
                        xPos = 40; // X konumunu sıfırla
                        yPos += 80; // Y konumunu bir sonraki satıra kaydır
                    }
                }

                // TrackBar ve etiketlerin son konumunu baz alarak formun genişliğini ve yüksekliğini ayarla
                int newWidth = 800; // En az bir minimum genişlik olmalı
                int newHeight = yPos + 100; // Son TrackBar'ın bulunduğu satırın sonuna ek bir boşluk

                Size = new Size(newWidth, newHeight); // Formun yeni boyutunu ayarla
            }
            catch
            {
                return;
            }
        }

        private void Skill_Star_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form1'e TrackBar değerlerini gönder
            BilgiGiris form1 = (BilgiGiris)Application.OpenForms["BilgiGiris"];
            form1.UpdateSkills(trackBarValues); // List<int> olmalı
        }

        private string[] GetSkillsFromForm1()
        {
            // Form1'de yeteneklerin metin kutularından beceri bilgilerini al
            BilgiGiris form1 = (BilgiGiris)Application.OpenForms["BilgiGiris"];
            string skills1 = form1.Skills1Text;
            string skills2 = form1.Skills2Text;

            // Yetenekleri birleştir
            string combinedSkills = skills1 + "," + skills2;

            // Virgüle göre ayırarak yetenekleri dizi olarak döndür
            return combinedSkills.Split(',');
        }
    }
}
