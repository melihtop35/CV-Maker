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

namespace CV_Maker
{
    public partial class BilgiGiris : Form
    {
        public BilgiGiris()
        {
            InitializeComponent();
            InitializeTextBox(skills1TextBox);
            InitializeTextBox(skills2TextBox);
            InitializeTextBox(projectRichTextBox);
            InitializeTextBox(certificateRichTextBox);
            InitializeTextBox(hobiesRichTextBox);
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
            string selectedLanguagesAndTier = $"{languageComboBox.SelectedItem} - {tierComboBox.SelectedItem}";
            languageListBox.Items.Add(selectedLanguagesAndTier);
        }
        public string[] SplitterSkills()
        {
            string[] allSkills = new string[0]; // varsayılan olarak boş bir dizi

            // İki metni birleştir ve virgülle ayır
            string combinedText = skills1TextBox.Text + "," + skills2TextBox.Text;

            if (!string.IsNullOrEmpty(combinedText) && !(combinedText.Contains("Yazarken aralarına virgül bırakın!")))
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
        private void saveEduBtn_Click(object sender, EventArgs e)
        {
            string schoolName, schoolDepartment, finishPoint, startDate, finishDate;
            string educationInfo;
            if (checkBoxEdu.Checked)
            {
                educationInfo = "Eğitim bilgisi yok";

                // Eğitim bilgilerini ListBox'a ekle
                listBoxEdu.Items.Add(educationInfo);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox1);
            }
            else
            {
                schoolName = schollNameTextBox.Text;
                schoolDepartment = programTextBox.Text;
                finishPoint = finishPointTextBox.Text;
                startDate = startTimePicker.Value.ToShortDateString();
                if (finishTimePicker.Checked) finishDate = finishTimePicker.Value.ToShortDateString();
                else finishDate = "Devam etmekte";

                // Eğitim bilgilerini birleştir
                educationInfo = $"Okul Adı: {schoolName}, Programı: {schoolDepartment}, Başlama Tarihi: {startDate} - Bitirme Tarihi: {finishDate}, Bitirme Puanı: {finishPoint}";

                // Doğrulama: Eğer başlangıç tarihi bitiş tarihinden büyükse hata mesajı göster
                if (checkBoxEdu.Checked)
                {
                    if (startTimePicker.Value > finishTimePicker.Value)
                    {
                        MessageBox.Show("Başlangıç tarihi bitiş tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Doğrulama başarısız olduğu için işlemi sonlandır
                    }
                }
                //Boş bilgileri yokla
                if (schoolName == "" & schoolDepartment == "" & finishPoint == "")
                {
                    educationInfo = "Eğitim bilgisi yok";
                }
                else if (schoolName == "")
                {
                    MessageBox.Show("Okul adı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (schoolDepartment == "")
                {
                    MessageBox.Show("Program boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (finishPoint == "")
                {
                    MessageBox.Show("Bitirme puanı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // Eğitim bilgilerini ListBox'a ekle
                listBoxEdu.Items.Add(educationInfo);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox1);
            }
        }
        private void saveJobExpBtn_Click(object sender, EventArgs e)
        {
            string jobName, companyName, positionName, startDate, finishDate;
            string jobİnfo;
            if (checkBoxJobExp.Checked)
            {
                jobİnfo = "İş bilgisi yok";

                // Eğitim bilgilerini ListBox'a ekle
                listBoxJobExp.Items.Add(jobİnfo);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox2);
            }
            else
            {
                jobName = jobTextBox.Text;
                companyName = companyTextBox.Text;
                positionName = positionTextBox.Text;
                startDate = jobStartPicker.Value.ToShortDateString();
                if (jobFinishPicker.Checked) finishDate = jobFinishPicker.Value.ToShortDateString();
                else finishDate = "Devam etmekte";

                // Eğitim bilgilerini birleştir
                jobİnfo = $"İş Adı: {jobName}, Şirket Adı: {companyName}, Çalışılan Pozisyon: {positionName}, Başlama Tarihi: {startDate} - Bitirme Tarihi: {finishDate}";

                // Doğrulama: Eğer başlangıç tarihi bitiş tarihinden büyükse hata mesajı göster
                if (checkBoxJobExp.Checked)
                {
                    if (jobStartPicker.Value > jobFinishPicker.Value)
                    {
                        MessageBox.Show("Başlangıç tarihi bitiş tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Doğrulama başarısız olduğu için işlemi sonlandır
                    }
                }
                //Boş bilgileri yokla
                if (jobName == "" & companyName == "" & positionName == "")
                {
                    jobİnfo = "İş bilgisi yok";
                }
                else if (jobName == "")
                {
                    MessageBox.Show("Okul adı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (companyName == "")
                {
                    MessageBox.Show("Program boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (positionName == "")
                {
                    MessageBox.Show("Bitirme puanı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // Eğitim bilgilerini ListBox'a ekle
                listBoxJobExp.Items.Add(jobİnfo);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox2);
            }
        }
        private void ClearTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                else if (c is GroupBox)
                {
                    ClearTextBoxes(c);
                }
            }
        }
        private void InitializeTextBox(Control textBox)
        {
            if (textBox is TextBox || textBox is RichTextBox)
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "Yazarken aralarına virgül bırakın!";
                textBox.Enter += TextBox_Enter;
                textBox.Leave += TextBox_Leave;
            }
        }
        private void TextBox_Enter(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            if (textBox.Text == "Yazarken aralarına virgül bırakın!")
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void TextBox_Leave(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Yazarken aralarına virgül bırakın!";
                textBox.ForeColor = Color.Gray;
            }
            if (!textBox.Text.Contains("Yazarken aralarına virgül bırakın!"))
            {
                SplitterSkills();
                FillCheckBoxes();
            }
        }
        //resim işlemi yapan fonksiyon
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
                pictureBox.BackgroundImage = null;
                // Seçilen resmin yolunu al ve PictureBox'a yükle
                pictureBox.Image = new Bitmap(openFileDialog1.FileName);
            }
        }
        private void pdfShowcaseBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
