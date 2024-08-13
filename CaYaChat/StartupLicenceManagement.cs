using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Linq;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace CaYaChat
{
    public partial class StartupLicenceManagement : Form
    {
        public StartupLicenceManagement()
        {
            InitializeComponent();
        }

        private async void btnCheckLicense_Click(object sender, EventArgs e)
        {
            string licenseKey = textBox1.Text;

            if (string.IsNullOrEmpty(licenseKey))
            {
                MessageBox.Show("Lütfen bir lisans anahtarı girin.");
                return;
            }

            try
            {
                var licenseStatus = await MongoDBService.CheckLicenseAsync(licenseKey);

                if (licenseStatus.Exists)
                {
                    if (licenseStatus.IsOwner)
                    {
                        // Sahibine özel işlem yapılır
                        MessageBox.Show("Lisans geçerli ve sahibisiniz.");
                    }
                    else
                    {
                        MessageBox.Show("Lisans geçerli fakat sahibi değilsiniz.");
                    }
                }
                else
                {
                    MessageBox.Show("Lisans geçersiz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lisans kontrolü sırasında bir hata oluştu: {ex.Message}");
            }
        }



        private async void btnBanID_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
            {
                MessageBox.Show("Program sahibi yasaklanamaz.");
                return;
            }

            string idInput = textBox2.Text;

            if (int.TryParse(idInput, out int id))
            {
                bool success = await MongoDBService.UpdateBanStatusByIdAsync(id, true);
                if (success)
                {
                    ListLicenseStatus();
                    MessageBox.Show("ID başarılı bir şekilde yasaklandı.");
                }
                else
                {
                    MessageBox.Show("Yasaklama işlemi başarısız oldu.");
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID formatı. Lütfen geçerli bir sayı girin.");
            }
        }

        private async void btnUnbanID_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
            {
                MessageBox.Show("Program sahibine bu işlem gerçekleştirilemez.");
                return;
            }

            string idInput = textBox2.Text;

            if (int.TryParse(idInput, out int id))
            {
                bool success = await MongoDBService.UpdateBanStatusByIdAsync(id, false);
                if (success)
                {
                    ListLicenseStatus();
                    MessageBox.Show("ID'nin yasağı başarılı bir şekilde kaldırıldı.");
                }
                else
                {
                    MessageBox.Show("Yasağı kaldırma işlemi başarısız oldu.");
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID formatı. Lütfen geçerli bir sayı girin.");
            }
        }



        private async void btnCheckID_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int id))
            {
                var document = await MongoDBService.GetLicenseDocumentByIdAsync(id);
                if (document != null)
                {
                    MessageBox.Show($"Bulunan belge: {document.ToJson()}");
                }
                else
                {
                    MessageBox.Show("Belge bulunamadı.");
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID formatı.");
            }
        }


        private async void btnListLicenses_Click(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#0a5fac");
            ListLicenseStatus();
        }

        private async void ListLicenseStatus()
        {
            richTextBox1.Clear(); // RichTextBox'ı temizleyelim.

            var licenses = await MongoDBService.GetAllLicensesAsync();

            foreach (var license in licenses)
            {
                int id = license.GetValue("ID").AsInt32;
                string licenseKey = license.GetValue("Licence").AsString;
                bool isOwner = license.GetValue("Owner").AsBoolean;
                bool isBanned = license.GetValue("Banned").AsBoolean;

                string bannedStatus = isBanned ? "Yasaklı" : "Yasaklı Değil";
                string ownerStatus = isOwner ? "Yetkili" : "Yetkili değil";

                // ID kısmını kalın yap
                richTextBox1.AppendText("ID: ");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText($"{id} ");

                // Lisans kısmını kalın yap
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                richTextBox1.AppendText(", Lisans: ");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText($"{licenseKey} ");

                // Yasaklı kısmını kalın yap
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                richTextBox1.AppendText(", Yasaklı?: ");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText($"{bannedStatus} ");

                // Yetkili kısmını kalın yap
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                richTextBox1.AppendText(", Yetkili?: ");
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText($"{ownerStatus}\n");

                // Tekrar varsayılan fonta dön
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            }
        }


        private void StartupLisance_Load(object sender, EventArgs e)
        {
            //this.BackColor = ColorTranslator.FromHtml("#212121");
            Theme();
            ListLicenseStatus();
        }

        private void Theme()
        {
            this.BackColor = ColorTranslator.FromHtml("#212121");
            button2.BackColor = ColorTranslator.FromHtml("#212121");
            button6.BackColor = ColorTranslator.FromHtml("#212121");
            button7.BackColor = ColorTranslator.FromHtml("#212121");
            button3.BackColor = ColorTranslator.FromHtml("#212121");
            button4.BackColor = ColorTranslator.FromHtml("#212121");
            button8.BackColor = ColorTranslator.FromHtml("#212121");
            button9.BackColor = ColorTranslator.FromHtml("#212121");
            richTextBox1.BackColor = ColorTranslator.FromHtml("#212121");
            richTextBox2.BackColor = ColorTranslator.FromHtml("#212121");
            richTextBox3.BackColor = ColorTranslator.FromHtml("#212121");
            textBox2.BackColor = ColorTranslator.FromHtml("#525252");
            this.BackColor = ColorTranslator.FromHtml("#212121");
        }











        private async void btnAddLicense_Click(object sender, EventArgs e)
        {
            string licenseKey = GenerateLicenseKey();

            // Yeni ID'yi al
            int newId = await MongoDBService.GetNextAvailableIdAsync();

            if (newId >= 0 && !string.IsNullOrEmpty(textBox1.Text))
            {
                //string licenseKey = textBox1.Text;
                bool isOwner = checkBoxIsOwner.Checked;
                bool isBanned = checkBoxIsBanned.Checked;

                bool success = await MongoDBService.AddLicenseAsync(newId, licenseKey, isOwner, isBanned);
                if (success)
                {
                    ListLicenseStatus();
                    MessageBox.Show("Lisans başarılı bir şekilde eklendi.");
                }
                else
                {
                    MessageBox.Show("Lisans eklenirken bir hata oluştu.");
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID veya lisans anahtarı.");
            }
        }


        public static string GenerateLicenseKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            var segments = new StringBuilder();

            for (int i = 0; i < 4; i++) // 4 segment
            {
                if (i > 0)
                    segments.Append('-'); // Segmentleri ayırmak için tire ekle

                // Her segment için 4 karakter ekle
                segments.Append(new string(Enumerable.Repeat(chars, 4)
                                                     .Select(s => s[random.Next(s.Length)])
                                                     .ToArray()));
            }

            return segments.ToString();
        }




        private async void btnDeleteLicense_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "0")
            {
                MessageBox.Show("Program sahibinin lisansı silinemez.");
                return;
            }

            if (int.TryParse(textBox2.Text, out int id))
            {
                bool success = await MongoDBService.DeleteLicenseByIdAsync(id);
                if (success)
                {
                    ListLicenseStatus();
                    MessageBox.Show("Lisans başarılı bir şekilde silindi.");
                }
                else
                {
                    MessageBox.Show("Lisans silinirken bir hata oluştu.");
                }
            }
            else
            {
                MessageBox.Show("Geçersiz ID.");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Eğer basılan tuş sayı değilse ve basılan tuş geri alma tuşu değilse
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Tuşun işlenmesini engelle
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CustomFligran cf = new CustomFligran();
            cf.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            BotSettings bs = new BotSettings();
            bs.ShowDialog();
        }
    }
}
