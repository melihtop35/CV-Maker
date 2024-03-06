using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CV_Maker
{
    public partial class BilgiGiris : Form
    {
        public BilgiGiris()
        {
            InitializeComponent();
            skills1Back();
            skills2Back();
        }

        private void BilgiGiris_Load(object sender, EventArgs e)
        {
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.NeutralCultures))
            {
                languageComboBox.Items.Add(culture.DisplayName);
            }
        }
        




        private void skills1Back()
        {
            skills1TextBox.ForeColor = Color.Gray;
            skills1TextBox.Text = "Yeteneklerin aralarına virgül ',' bırakın!";
            skills1TextBox.Enter += Skills1_Enter;
            skills1TextBox.Leave += Skills1_Leave;
        }
        private void Skills1_Enter(object sender, EventArgs e)
        {
            // TextBox'a odaklandığında, arka plan metni temizleyin
            if (skills1TextBox.Text == "Yeteneklerin aralarına virgül ',' bırakın!")
            {
                skills1TextBox.Text = "";
                skills1TextBox.ForeColor = SystemColors.WindowText; // TextBox normal yazı rengi
            }
        }
        private void Skills1_Leave(object sender, EventArgs e)
        {
            // TextBox odaktan çıktığında, arka plan metnini geri ekleyin (yeterli giriş yoksa)
            if (string.IsNullOrWhiteSpace(skills1TextBox.Text))
            {
                skills1TextBox.Text = "Yeteneklerin aralarına virgül ',' bırakın!";
                skills1TextBox.ForeColor = Color.Gray;
            }
        }
        private void skills2Back()
        {
            skills2TextBox.ForeColor = Color.Gray;
            skills2TextBox.Text = "Yeteneklerin aralarına virgül ',' bırakın!";
            skills2TextBox.Enter += Skills2_Enter;
            skills2TextBox.Leave += Skills2_Leave;
        }
        private void Skills2_Enter(object sender, EventArgs e)
        {
            // TextBox'a odaklandığında, arka plan metni temizleyin
            if (skills2TextBox.Text == "Yeteneklerin aralarına virgül ',' bırakın!")
            {
                skills2TextBox.Text = "";
                skills2TextBox.ForeColor = SystemColors.WindowText; // TextBox normal yazı rengi
            }
        }
        private void Skills2_Leave(object sender, EventArgs e)
        {
            // TextBox odaktan çıktığında, arka plan metnini geri ekleyin (yeterli giriş yoksa)
            if (string.IsNullOrWhiteSpace(skills2TextBox.Text))
            {
                skills2TextBox.Text = "Yeteneklerin aralarına virgül ',' bırakın!";
                skills2TextBox.ForeColor = Color.Gray;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Sadece resim dosyalarını filtrele
            openFileDialog1.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.Title = "Resim Seç";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Eski resmi kaldır
                pictureBox.Image = null;
                // Seçilen resmin yolunu al ve PictureBox'a yükle
                pictureBox.Image = new Bitmap(openFileDialog1.FileName);
            }
        }
    }
}
