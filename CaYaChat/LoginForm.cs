using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CaYaChat
{
    public partial class LoginForm : Form
    {
        public string Username { get; private set; }
        public string TextMessageMenu { get; private set; }
        public string WindowNameC { get; private set; }
        public Color TxtMessageBackColor { get; private set; }
        public Color TxtMessageForeColor { get; private set; }
        public Color ListBoxMessagesBackColor { get; private set; }
        public Color ListBoxMessagesForeColor { get; private set; }




        private static readonly string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CaYaChatMessage");
        private static readonly string usernameFilePath = Path.Combine(appDataPath, "CaYaUser.CYT");
        private static readonly string customizeFilePath = Path.Combine(appDataPath, "CaYaCustomize.CYT");
        private static readonly string hotkeysFilePath = Path.Combine(appDataPath, "Hotkeys.CYT");

        private bool Closing = true;


        private bool OwnerButtonActive;


        string IsOwner = "false";


        string selectedAppName;



        public LoginForm(StartuplicenceCheck checklicense)
        {
            InitializeComponent();

            //StartuplicenceCheck.LicenseCheckOwner += OwnerCheckAndLoad;
            //StartuplicenceCheck.LicenseCheckDefault += DefaultCheckAndLoad;

            LoadSettings();

            OwnerCheckAndLoad();
        }


        private async void OwnerCheckAndLoad()
        {

            IsOwner = SharedData.MessageForLicense;
            //MessageBox.Show(IsOwner);

            //OwnerButton.Visible = true;

            //MessageBox.Show("YEEEEE");

            if (IsOwner == "true")
            {
                //MessageBox.Show("Bu lisans sahibine aittir.");
                // Sahibine özel işlemler burada yapılabilir
                //await Task.Delay(3000);
                //OwnerButton.Visible = true;
                //MessageBox.Show(IsOwner + "YEEEEE");
                OwnerButton.Visible = true;
                //MessageBox.Show("Durum: " + OwnerButtonActive);

            }
            else if (IsOwner == "false")
            {
                OwnerButton.Visible = false;

                //MessageBox.Show(IsOwner + "NOOOO");
            }
            else
            {
                MessageBox.Show("Uygulamada lisans ile ilgili problem meydana geldi! Bu yüzden program bu mesajdan sonra kapatılıcaktır.");
                Environment.Exit(0);
            }
        }
        private async void DefaultCheckAndLoad()
        {
            MessageBox.Show("NOOOO");
        }

        private void LoadSettings()
        {

            if (File.Exists(usernameFilePath))
            {
                txtUsername.Text = File.ReadAllText(usernameFilePath);
            }

            if (File.Exists(customizeFilePath))
            {
                var lines = File.ReadAllLines(customizeFilePath);
                if (lines.Length >= 4)
                {
                    TxtMessageBackColor = ColorTranslator.FromHtml(lines[0]);
                    TxtMessageForeColor = ColorTranslator.FromHtml(lines[1]);
                    ListBoxMessagesBackColor = ColorTranslator.FromHtml(lines[2]);
                    ListBoxMessagesForeColor = ColorTranslator.FromHtml(lines[3]);
                }
                else
                {
                    ResetColorsToDefault();
                }
            }
            else
            {
                ResetColorsToDefault();
            }

            if (File.Exists(hotkeysFilePath))
            {
                TextMessageMenu = File.ReadAllText(hotkeysFilePath);
                HotKeyTM.Text = TextMessageMenu;
            }

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void SaveSettings()
        {
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            File.WriteAllText(usernameFilePath, Username);

            var customizeData = new[]
            {
                ColorTranslator.ToHtml(TxtMessageBackColor),
                ColorTranslator.ToHtml(TxtMessageForeColor),
                ColorTranslator.ToHtml(ListBoxMessagesBackColor),
                ColorTranslator.ToHtml(ListBoxMessagesForeColor)
            };
            File.WriteAllLines(customizeFilePath, customizeData);

            File.WriteAllText(hotkeysFilePath, TextMessageMenu);
        }

        private void ResetColorsToDefault()
        {
            TxtMessageBackColor = ColorTranslator.FromHtml("#BE1616");
            TxtMessageForeColor = Color.White;
            ListBoxMessagesBackColor = Color.Black;
            ListBoxMessagesForeColor = Color.White;

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Username = txtUsername.Text;
            if (!string.IsNullOrEmpty(Username))
            {
                if (Username == "System")
                {
                    MessageBox.Show("Bu kullanıcı adı sisteme aittir.", "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Username == SharedData.BotName)
                {
                    MessageBox.Show("Bu kullanıcı adı bota aittir.", "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    SaveSettings();
                    this.DialogResult = DialogResult.OK;
                    Closing = false;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Lütfen kullanıcı adınızı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTxtMessageBackColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    TxtMessageBackColor = colorDialog.Color;
                }
            }

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void btnTxtMessageForeColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    TxtMessageForeColor = colorDialog.Color;
                }
            }

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void btnListBoxMessagesBackColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    ListBoxMessagesBackColor = colorDialog.Color;
                }
            }

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void btnListBoxMessagesForeColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    ListBoxMessagesForeColor = colorDialog.Color;
                }
            }

            panel1.BackColor = ListBoxMessagesForeColor;
            panel2.BackColor = ListBoxMessagesBackColor;
            panel3.BackColor = TxtMessageForeColor;
            panel4.BackColor = TxtMessageBackColor;
        }

        private void btnResetColors_Click(object sender, EventArgs e)
        {
            ResetColorsToDefault();
            SaveSettings();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void HotKeyTM_TextChanged(object sender, EventArgs e)
        {
            // Tüm metni büyük harfe çevir
            HotKeyTM.Text = HotKeyTM.Text.ToUpper();

            // İmlecin doğru konumda kalmasını sağlamak için
            //HotKeyTM.SelectionStart = HotKeyTM.Text.Length;
            // Girilen metni büyük harfe çevir ve Türkçe 'i' karakterini kontrol et
            HotKeyTM.Text = HotKeyTM.Text.ToUpper().Replace('İ', 'I');

            TextMessageMenu = HotKeyTM.Text;
        }

        private void HotKeyTM_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Eğer basılan tuş bir harf değilse, bu karakterin girilmesini engelle
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }


            // Eğer girilen karakter küçük 'i' ise, 'I' olarak değiştir
            if (e.KeyChar == 'i')
            {
                e.KeyChar = 'I';
            }
            // Eğer girilen karakter küçük 'ı' ise, 'I' olarak değiştir
            else if (e.KeyChar == 'ı')
            {
                e.KeyChar = 'I';
            }
            // Eğer girilen karakter küçük bir harfse, büyük harfe çevir
            else if (e.KeyChar >= 'a' && e.KeyChar <= 'z')
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
            // Eğer girilen karakter büyük bir İngilizce harfse veya kontrol karakteriyse, bir şey yapma
            else if ((e.KeyChar >= 'A' && e.KeyChar <= 'Z') || char.IsControl(e.KeyChar))
            {
                // Do nothing
            }
            // Diğer tüm karakterleri iptal et
            else
            {
                e.Handled = true;
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Closing == true)
            {
                //e.Cancel = true;
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (OwnerButtonActive == true)
            {
                OwnerButton.Visible = true;
            }
            SettingsForm settingsForm = new SettingsForm();
            //settingsForm.ShowDialog();




            if (settingsForm.ShowDialog() == DialogResult.OK)
            {

                WindowNameC = settingsForm.SelectedAppName;

                //CaYaChatApp chatApp = new CaYaChatApp(selectedAppName);
                //chatApp.Show();
            }
        }

        private void OwnerButton_Click(object sender, EventArgs e)
        {
            StartupLicenceManagement lm = new StartupLicenceManagement();
            lm.ShowDialog();
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            //OwnerCheckAndLoad();
            
            //MessageBox.Show("Durum F: " + OwnerButtonActive);
            //MessageBox.Show("Durum FA: " + IsOwner);

            //IsOwner = Owner;
            //MessageBox.Show(IsOwner);
            //OwnerCheckAndLoad();
        }
    }
}
