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
        private void languageProcessBtn_Click(object sender, EventArgs e)
        {
            string selectedLanguagesAndTier=$"{languageComboBox.SelectedItem} - {tierComboBox.SelectedItem}";
            languageListBox.Items.Add(selectedLanguagesAndTier);
        }
        public string[] SplitterSkills()
        {
            string[] allSkills = new string[0]; // varsayılan olarak boş bir dizi

            // İki metni birleştir ve virgülle ayır
            string combinedText = skills1TextBox.Text + "," + skills2TextBox.Text;

            if (!string.IsNullOrEmpty(combinedText) && !(combinedText.Contains("Yeteneklerin aralarına virgül ',' bırakın!")))
            {
                allSkills = combinedText.Split(',');
            }
            // Birleştirilmiş dize dizisini döndür
            return allSkills;
        }

        public void FillCheckBoxes()
        {
            string[] allSkills = SplitterSkills(); // Tüm yeteneklerin listesi alınır

            highSkillsCheckBox.Items.Clear(); // Yüksek seviyedeki işaret kutusunun öğeleri temizlenir
            midSkillsCheckBox.Items.Clear(); // Orta seviyedeki işaret kutusunun öğeleri temizlenir
            lowSkillsCheckBox.Items.Clear(); // Düşük seviyedeki işaret kutusunun öğeleri temizlenir

            foreach (string skill in allSkills)
            {
                highSkillsCheckBox.Items.Add(skill); // Yüksek seviyedeki işaret kutusuna yetenek eklenir
                midSkillsCheckBox.Items.Add(skill); // Orta seviyedeki işaret kutusuna yetenek eklenir
                lowSkillsCheckBox.Items.Add(skill); // Düşük seviyedeki işaret kutusuna yetenek eklenir
            }

            // İşaret kutularının kontrol değişim olaylarına olay dinleyicisi eklenir
            highSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
            midSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
            lowSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
        }

        private void CheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox checkBox = (CheckedListBox)sender; // Olayı tetikleyen işaret kutusu alınır

            if (e.NewValue == CheckState.Checked) // Yeni işaret durumu "işaretlenmiş" ise
            {
                // İşaretlenen öğe alınır
                string checkedItem = checkBox.Items[e.Index].ToString();

                // Diğer işaret kutularındaki aynı öğelerin işaretleri kaldırılır
                if (checkBox == highSkillsCheckBox) // Yüksek seviyedeki işaret kutusu ise
                {
                    UncheckItemInOtherCheckBoxes(checkedItem, midSkillsCheckBox, lowSkillsCheckBox);
                }
                else if (checkBox == midSkillsCheckBox) // Orta seviyedeki işaret kutusu ise
                {
                    UncheckItemInOtherCheckBoxes(checkedItem, highSkillsCheckBox, lowSkillsCheckBox);
                }
                else if (checkBox == lowSkillsCheckBox) // Düşük seviyedeki işaret kutusu ise
                {
                    UncheckItemInOtherCheckBoxes(checkedItem, highSkillsCheckBox, midSkillsCheckBox);
                }
            }
        }

        private void UncheckItemInOtherCheckBoxes(string item, params CheckedListBox[] checkBoxes)
        {
            foreach (CheckedListBox checkBox in checkBoxes)
            {
                int index = checkBox.Items.IndexOf(item); // Öğenin konumu alınır
                if (index != -1) // Öğe bulunursa
                {
                    checkBox.ItemCheck -= CheckBox_ItemCheck; // Özyinelemeli çağrıyı önlemek için olay dinleyicisi geçici olarak kaldırılır
                    checkBox.SetItemChecked(index, false); // Öğe işareti kaldırılır
                    checkBox.ItemCheck += CheckBox_ItemCheck; // Olay dinleyicisi tekrar eklenir
                }
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
            SplitterSkills();
            FillCheckBoxes();
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
            SplitterSkills();
            FillCheckBoxes();      
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

        private void pdfShowcaseBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
