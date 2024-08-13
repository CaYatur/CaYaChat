using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static CaYaChat.MongoDBService;

namespace CaYaChat
{
    public partial class StartuplicenceCheck : Form
    {
        //public static event EventHandler LicenseCheckOwner;
        //public static event EventHandler LicenseCheckDefault;

        private const int MaxLength = 19;
        private static readonly string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CaYaChatMessage");
        private static readonly string licenseFilePath = Path.Combine(appDataPath, "CaYaLicence.CYT");

        private bool Closing = true;

        public StartuplicenceCheck()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Köşe yarıçapı
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
            base.OnPaint(e);
        }

        private void StartupLisanceCheck_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#0a5fac");
            panel1.BackColor = ColorTranslator.FromHtml("#212121");
            textBox1.MaxLength = MaxLength;
            label2.Text = SharedData.RegisteredUserDataInfo;




            // Kayıtlı lisans anahtarını yükle
            if (File.Exists(licenseFilePath))
            {
                string savedLicenseKey = File.ReadAllText(licenseFilePath);
                textBox1.Text = savedLicenseKey;
                LoginWithLicence();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text.ToUpper();
            text = Regex.Replace(text, "[ıiİŞÜ]", "I");
            text = text.Replace("-", "");
            text = string.Join("-", SplitIntoChunks(text, 4));
            textBox1.Text = text;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private static string[] SplitIntoChunks(string text, int chunkSize)
        {
            int length = text.Length;
            int numChunks = (int)Math.Ceiling((double)length / chunkSize);
            string[] chunks = new string[numChunks];

            for (int i = 0; i < numChunks; i++)
            {
                int start = i * chunkSize;
                int size = Math.Min(chunkSize, length - start);
                chunks[i] = text.Substring(start, size);
            }

            return chunks;
        }

        private async void btnCheckLicense_Click(object sender, EventArgs e)
        {
            //ValidateLicense();
            LoginWithLicence();
        }

        private async void ValidateLicense()
        {
            int licenseKey;

            // Lisans anahtarını al ve geçerli bir sayı olup olmadığını kontrol et
            if (!int.TryParse(textBox1.Text, out licenseKey))
            {
                MessageBox.Show("Lütfen geçerli bir lisans anahtarı (sayı) girin.");
                return;
            }

            try
            {
                // Lisansı kontrol et
                var licenseStatus = await CheckIDLicenseAsync(licenseKey);

                // Lisans geçerliyse ve yasaklı değilse
                if (licenseStatus.IsOwner)
                {

                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Lisans kontrolü sırasında bir hata oluştu: {ex.Message}");
                if (ex.Message == "Bu ID yasaklı.")
                {

                }
            }
        }

        private async void LoginWithLicence()
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
                        if (SharedData.Fligran == null)
                        {
                            label2.Text = SharedData.RegisteredUserDataInfo;
                        }
                        else
                        {
                            label2.Text = SharedData.Fligran;
                        }
                        // Sahibine özel işlem yapılır
                        this.BackColor = ColorTranslator.FromHtml("#12863c");
                        InfoLabel.ForeColor = ColorTranslator.FromHtml("#12863c");
                        InfoLabel.Visible = true;
                        InfoLabel.Text = "Lisans Onaylandı!";
                        button1.Enabled = false;
                        textBox1.Enabled = false;

                        // Lisans anahtarını kaydet
                        if (!Directory.Exists(appDataPath))
                        {
                            Directory.CreateDirectory(appDataPath);
                        }
                        File.WriteAllText(licenseFilePath, licenseKey);


                        //LicenseCheckOwner?.Invoke(this, EventArgs.Empty);


                        SharedData.MessageForLicense = "true";

                        await Task.Delay(1000);

                        //lg.SetMessage("true");


                        Closing = false;
                        Close(); // Mevcut formu gizle

                        //MessageBox.Show("Lisans geçerli ve sahibisiniz.");
                    }
                    else
                    {
                        if (SharedData.Fligran == null)
                        {
                            label2.Text = SharedData.RegisteredUserDataInfo;
                        }
                        else
                        {
                            label2.Text = SharedData.Fligran;
                        }
                        this.BackColor = ColorTranslator.FromHtml("#12863c");
                        InfoLabel.ForeColor = ColorTranslator.FromHtml("#12863c");
                        InfoLabel.Visible = true;
                        InfoLabel.Text = "Lisans Onaylandı!";
                        button1.Enabled = false;
                        textBox1.Enabled = false;

                        // Lisans anahtarını kaydet
                        if (!Directory.Exists(appDataPath))
                        {
                            Directory.CreateDirectory(appDataPath);
                        }
                        File.WriteAllText(licenseFilePath, licenseKey);

                        //LoginForm lg = LoginForm.Instance;
                        //lg.Data = "false";
                        //lg.Load("false");

                        //LicenseCheckDefault?.Invoke(this, EventArgs.Empty);

                        SharedData.MessageForLicense = "false";


                        await Task.Delay(1000);

                        Closing = false;
                        Close();
                        //MessageBox.Show("Lisans geçerli fakat sahibi değilsiniz.");
                    }
                }
                else
                {
                    if (SharedData.Fligran == null)
                    {
                        label2.Text = SharedData.RegisteredUserDataInfo;
                    }
                    else
                    {
                        label2.Text = SharedData.Fligran;
                    }
                    this.BackColor = ColorTranslator.FromHtml("#d22a46");
                    InfoLabel.Visible = true;
                    InfoLabel.Text = "Lisans geçersiz.";
                    //MessageBox.Show("Lisans geçersiz.");
                }
            }
            catch (Exception ex)
            {
                if (SharedData.Fligran == null)
                {
                    label2.Text = SharedData.RegisteredUserDataInfo;
                }
                else
                {
                    label2.Text = SharedData.Fligran;
                }
                this.BackColor = ColorTranslator.FromHtml("#d22a46");
                string customErrorMessage = ex.Message;
                if (customErrorMessage == "Bu lisans yasaklı.")
                {
                    InfoLabel.Visible = true;
                    InfoLabel.Text = ex.Message;
                    MessageBox.Show($"Bu lisans yasaklandı! Uygulamanız devre dışı bırakılmıştır. Lütfen yöneticinizden yeni lisans anahtarı alın veya yasağınızı açtırın.");
                }
                else
                {
                    button2.Visible = true;
                    MessageBox.Show($"Lisans kontrolü sırasında bir hata oluştu: {ex.Message}");
                }

            }
        }

        private void StartuplicenceCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Closing == true)
            {
                Environment.Exit(0);
            }
            else if (Closing == false)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Mevcut uygulamanın çalıştırılabilir dosyasının yolunu al
            string exePath = Application.ExecutablePath;

            // Yeni bir işlem başlat (mevcut programı yeniden başlat)
            Process.Start(exePath);

            // Mevcut uygulamayı kapat
            Environment.Exit(0);
        }
    }
}
