using System;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace CV_Maker
{
    public partial class BilgiGiris : Form
    {
        public BilgiGiris()
        {
            InitializeComponent();
            InitializeTextBox(skills1TextBox);
            InitializeTextBox(skills2TextBox);
            InitializeRichTextBox(projectRichTextBox);
            InitializeRichTextBoxCertificate(certificateRichTextBox);
            InitializeRichTextBox(hobiesRichTextBox);
        }
        // BilgiGiris sınıfında
        public string Skills1Text
        {
            get { return skills1TextBox.Text; }
        }

        public string Skills2Text
        {
            get { return skills2TextBox.Text; }
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
        private void languageDeleteBtn_Click(object sender, EventArgs e)
        {
            languageListBox.Items.RemoveAt(languageListBox.SelectedIndex);
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
        private int totalCheckedSkills = 0; // Toplam işaretlenmiş yetenek sayısı

        public void FillCheckBoxes()
        {
            string[] allSkills = SplitterSkills(); // Tüm yeteneklerin listesi alınır

            highSkillsCheckBox.Items.Clear(); // Yüksek seviyedeki işaret kutusunun öğeleri temizlenir
            midSkillsCheckBox.Items.Clear(); // Orta seviyedeki işaret kutusunun öğeleri temizlenir
            lowSkillsCheckBox.Items.Clear(); // Düşük seviyedeki işaret kutusunun öğeleri temizlenir

            foreach (string skill in allSkills)
            {
                highSkillsCheckBox.Items.Add(skill.Trim()); // Yüksek seviyedeki işaret kutusuna yetenek eklenir
                midSkillsCheckBox.Items.Add(skill.Trim()); // Orta seviyedeki işaret kutusuna yetenek eklenir
                lowSkillsCheckBox.Items.Add(skill.Trim()); // Düşük seviyedeki işaret kutusuna yetenek eklenir
            }

            // İşaret kutularının kontrol değişim olaylarına olay dinleyicisi eklenir
            highSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
            midSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
            lowSkillsCheckBox.ItemCheck += CheckBox_ItemCheck;
        }

        private void CheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox checkBox = (CheckedListBox)sender; // Olayı tetikleyen işaret kutusu alınır

            if (e.NewValue == CheckState.Checked && e.CurrentValue != CheckState.Checked) // Yeni işaret durumu "işaretlenmiş" ise ve önceki durumu "işaretlenmemiş" ise
            {
                totalCheckedSkills++; // Toplam işaretlenmiş yetenek sayısını arttır
                if (totalCheckedSkills == (highSkillsCheckBox.Items.Count)*2) // Eğer tüm yetenekler işaretlendi ise
                {
                    OpenNewForm(); // Yeni form aç
                }
            }
            else if (e.NewValue != CheckState.Checked && e.CurrentValue == CheckState.Checked) // Yeni işaret durumu "işaretlenmemiş" ise ve önceki durumu "işaretlenmiş" ise
            {
                --totalCheckedSkills; // Toplam işaretlenmiş yetenek sayısını azalt
            }

            if (e.NewValue == CheckState.Checked) // Eğer yeni işaret durumu "işaretlenmiş" ise
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

        private void OpenNewForm()
        {
            Form2 form2 =new Form2();
            form2.Show();
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
            string[] educationInfo = new string[2]; // educationInfo dizisi tanımla

            string schoolName, schoolDepartment, finishPoint, startDate, finishDate;

            if (checkBoxEdu.Checked)
            {
                educationInfo[0] = "Eğitim bilgisi yok";

                // Eğitim bilgilerini ListBox'a ekle
                listBoxEdu.Items.Add(educationInfo[0]);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox1);
            }
            else
            {
                schoolName = schoolNameTextBox.Text;
                schoolDepartment = programTextBox.Text;
                finishPoint = finishPointTextBox.Text;
                startDate = $"{startTimePicker.Value.Month}.{startTimePicker.Value.Year}";
                finishDate = finishTimePicker.Checked ? $"{finishTimePicker.Value.Month}.{finishTimePicker.Value.Year}" : "Devam etmekte";

                //Boş bilgileri yokla
                if (schoolName == "" && schoolDepartment == "" && finishPoint == "")
                {
                    educationInfo[0] = "Eğitim bilgisi yok";
                    // Eğitim bilgilerini ListBox'a ekle
                    listBoxEdu.Items.Add(educationInfo[0]);

                    // GroupBox içindeki metin kutularını temizle
                    ClearTextBoxes(groupBox1);
                    return;
                }
                else if (schoolName == "")
                {
                    MessageBox.Show("Okul adı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (schoolDepartment == "")
                {
                    MessageBox.Show("Program boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (finishPoint == "")
                {
                    MessageBox.Show("Bitirme puanı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Doğrulama: Eğer başlangıç tarihi bitiş tarihinden büyükse hata mesajı göster
                if (checkBoxEdu.Checked)
                {
                    if (startTimePicker.Value > finishTimePicker.Value)
                    {
                        MessageBox.Show("Başlangıç tarihi bitiş tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Doğrulama başarısız olduğu için işlemi sonlandır
                    }
                }

                // Eğitim bilgilerini birleştir ve diziye ekle
                educationInfo[0] = $"{startDate} - {finishDate} {schoolDepartment} - {finishPoint}";
                educationInfo[1] = schoolName;

                // Eğitim bilgilerini ListBox'a ekle
                listBoxEdu.Items.Add($"\n{startDate} - {finishDate} {schoolDepartment} - {finishPoint} \n{schoolName}");

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox1);
            }
        }
        private void saveJobExpBtn_Click(object sender, EventArgs e)
        {
            string[] jobInfo = new string[2]; // jobInfo dizisi tanımla

            string jobName, companyName, positionName, startDate, finishDate;

            if (checkBoxJobExp.Checked)
            {
                jobInfo[0] = "İş bilgisi yok";

                // İş bilgilerini ListBox'a ekle
                listBoxJobExp.Items.Add(jobInfo[0]);

                // GroupBox içindeki metin kutularını temizle
                ClearTextBoxes(groupBox2);
            }
            else
            {
                jobName = jobTextBox.Text;
                companyName = companyTextBox.Text;
                positionName = positionTextBox.Text;
                startDate = $"{jobStartPicker.Value.Month}.{jobStartPicker.Value.Year}";
                finishDate = jobFinishPicker.Checked ? $"{jobFinishPicker.Value.Month}.{jobFinishPicker.Value.Year}" : "Devam etmekte";

                //Boş bilgileri yokla
                if (jobName == "" && companyName == "" && positionName == "")
                {
                    jobInfo[0] = "İş bilgisi yok";
                    listBoxJobExp.Items.Add(jobInfo[0]);

                    // GroupBox içindeki metin kutularını temizle
                    ClearTextBoxes(groupBox2);
                    return;
                }
                else if (jobName == "")
                {
                    MessageBox.Show("İş adı boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (companyName == "")
                {
                    MessageBox.Show("Şirket boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (positionName == "")
                {
                    MessageBox.Show("Pozisyon boş.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // İş bilgilerini birleştir ve diziye ekle
                jobInfo[0] = $"{startDate} - {finishDate} {jobName} {positionName}";
                jobInfo[1] = companyName;

                // Doğrulama: Eğer başlangıç tarihi bitiş tarihinden büyükse hata mesajı göster
                if (checkBoxJobExp.Checked)
                {
                    if (jobStartPicker.Value > jobFinishPicker.Value)
                    {
                        MessageBox.Show("Başlangıç tarihi bitiş tarihinden önce olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Doğrulama başarısız olduğu için işlemi sonlandır
                    }
                }

                // İş bilgilerini ListBox'a ekle
                listBoxJobExp.Items.Add($"\n{startDate} - {finishDate} {jobName} {positionName} \n{companyName}");

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
        private void InitializeRichTextBox(Control textBox)
        {
            if (textBox is TextBox || textBox is RichTextBox)
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "Yazarken aralarına virgül bırakın!";
                textBox.Enter += TextBox_Enter;
                textBox.Leave += TextBox_LeaveForRichText;
            }
        }
        private void InitializeRichTextBoxCertificate(Control textBox)
        {
            if (textBox is TextBox || textBox is RichTextBox)
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "'Alınma Tarihi' - 'Sertifika Adı' Şeklinde Yazınız";
                textBox.Enter += TextBox_EnterCertificate;
                textBox.Leave += TextBox_LeaveForRichTextCertificate;
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
        private void TextBox_EnterCertificate(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            if (textBox.Text == "'Alınma Tarihi' - 'Sertifika Adı' Şeklinde Yazınız")
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
            if (!textBox.Text.Contains("Yazarken aralarına virgül bırakın"))
            {
                SplitterSkills();
                FillCheckBoxes();
            }
        }
        private void TextBox_LeaveForRichTextCertificate(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "'Alınma Tarihi' - 'Sertifika Adı' Şeklinde Yazınız";
                textBox.ForeColor = Color.Gray;
            }
        }
        private void TextBox_LeaveForRichText(object sender, EventArgs e)
        {
            Control textBox = (Control)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Yazarken aralarına virgül bırakın!";
                textBox.ForeColor = Color.Gray;
            }
        }
        private string GetCheckedItemsAsString(CheckedListBox checkedListBox)
        {
            string checkedItems = "";
            foreach (var item in checkedListBox.CheckedItems)
            {
                checkedItems += item.ToString() + ",";
            }
            return checkedItems;
        }
        //resim işlemi yapan fonksiyon
        private string selectedImagePath = ""; // Seçilen resmin yolunu saklamak için bir değişken

        private void pictureBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.Title = "Resim Seç";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Seçilen resmin yolunu sakla
                selectedImagePath = openFileDialog1.FileName;

                // Eski resmi kaldır
                pictureBox.Image = null;
                pictureBox.BackgroundImage = null;

                // Seçilen resmi PictureBox'a yükle
                pictureBox.Image = new Bitmap(selectedImagePath);
            }
        }
        // SkillStar'ı List<int> olarak tanımla
        public List<int> skillsStar = new List<int>();
        private void pdfShowcaseBtn_Click(object sender, EventArgs e)
        {
            // Formdaki bilgileri al
            string name = nameTxtBox.Text;
            string surname = surnameTxtBox.Text;
            string job = jobTxtBox.Text;
            string skillsImportant = skills1TextBox.Text;
            string skillsAdditional = skills2TextBox.Text;
            string highSkills = GetCheckedItemsAsString(highSkillsCheckBox);
            string midSkills = GetCheckedItemsAsString(midSkillsCheckBox);
            string lowSkills = GetCheckedItemsAsString(lowSkillsCheckBox);
            // Listbox'taki dilleri al
            List<string> languages = new List<string>();
            foreach (var item in languageListBox.Items)
            {
                languages.Add(item.ToString());
            }
            //Özet ve diğer uzun bilgiler
            string summary = sumRichTextBox.Text;
            string project = projectRichTextBox.Text;
            string certificate = certificateRichTextBox.Text;
            string hobies = hobiesRichTextBox.Text;
            //İletişim bilgileri
            string phone = phoneTextBox.Text;
            string email = emailTextBox.Text;
            string linkdln = linkedlnTextBox.Text;
            string github = githubTextBox.Text;
            string adress = adressTextBox.Text;
            //Eğitim bilgileri
            List<string> education = new List<string>();
            foreach (var item in listBoxEdu.Items)
            {
                education.Add(item.ToString());
            }
            //Deneyim bilgileri
            List<string> jobExperience = new List<string>();
            foreach (var item in listBoxJobExp.Items)
            {
                jobExperience.Add(item.ToString());
            }

            // PDF içeriğini oluştur
            string[] lines = { name + " " + surname, job, summary, phone, email, linkdln, github, adress, skillsImportant, skillsAdditional, string.Join(", ", languages), string.Join(" ", education), string.Join(", ", jobExperience), project, certificate, highSkills, midSkills, lowSkills, string.Join("", skillsStar), hobies };


            // Dosya yolunu seç
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Dosyası|*.pdf";
            saveFileDialog.Title = "PDF Kaydet";
            saveFileDialog.FileName = "CV.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"; // Varsayılan olarak indirilenler klasörü
            saveFileDialog.ShowDialog();


            if (saveFileDialog.FileName != "")
            {
                string outputPath = saveFileDialog.FileName;

                // Kullanıcı dosya adı belirtmediyse, varsayılan adı kullan
                if (Path.GetFileName(outputPath) == "")
                {
                    outputPath = Path.Combine(Path.GetDirectoryName(outputPath), "CV.pdf");
                }

                // Resim yolunu PdfHelper.CreatePdf metoduna iletiyoruz
                PdfHelper.CreatePdf(outputPath, lines, selectedImagePath);

                //MessageBox.Show("PDF dosyası oluşturuldu: " + outputPath);
            }
            BilgiGiris.ActiveForm.Close();
        }

        private void deleteEdu_Click(object sender, EventArgs e)
        {
            listBoxEdu.Items.RemoveAt(listBoxEdu.SelectedIndex);
        }

        private void deleteJob_Click(object sender, EventArgs e)
        {
            listBoxJobExp.Items.RemoveAt(listBoxJobExp.SelectedIndex);
        }

        // UpdateSkills yöntemi
        public void UpdateSkills(List<int> trackBarValues)
        {
            // skillsStar koleksiyonunu güncelle
            skillsStar.Clear();
            skillsStar.AddRange(trackBarValues);
        }
    }
}
