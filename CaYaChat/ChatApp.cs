using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection;
using static System.Windows.Forms.DataFormats;
using static CaYaChat.MongoDBService;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace CaYaChat
{
    public partial class CaYaChatApp : Form
    {
        public class ChatMessage
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement("username")]
            public string Username { get; set; } = string.Empty;

            [BsonElement("textmessagemenu")]
            public string TextMessageMenu { get; set; } = string.Empty;

            [BsonElement("windownamec")]
            public string WindowNameC { get; set; } = string.Empty;

            [BsonElement("message")]
            public string Message { get; set; } = string.Empty;

            [BsonElement("timestamp")]
            public DateTime Timestamp { get; set; }
        }

        

        private string _username2;
        private string _username;
        private string _textmessagemenu;
        private string _windownamec;
        private IntPtr _previousForegroundWindow;
        private System.Windows.Forms.Timer _timer;

        private string ArrowType = "|"; // :>

        private string BotName = "CaYaBot";
        private string DefaultBotResponse = "false";

        private GlobalKeyboardHook _globalHook;


        private static readonly string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CaYaChatMessage");
        private static readonly string customizeFilePath = Path.Combine(appDataPath, "CaYaCustomize.CYT");
        private static readonly string oneTimeMessageFilePath = Path.Combine(appDataPath, "CaYaOneTimeMessage.CYT");
        private static readonly string usernameFilePath = Path.Combine(appDataPath, "CaYaUser.CYT");

        private bool ToggleOpacity = false;
        private bool ToggleWindow = false;

        private Keys KeyCodeC;

        private bool ProgramWindow = false;

        private bool OwnerStatus = false;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;

        private string _selectedAppName;
        private System.Windows.Forms.Timer _visibilityCheckTimer;


        private static string MongoDBconnection;
        private static string MongoDBDatabase;

        public string Data { get; set; }


        //public static string ConnectionString = $"{MongoDBconnection} A";

        public void InitializeUser(string username2)
        {
            _username2 = username2;
            _username = $"{_username2} ({SharedData.UserID})";
        }
        public CaYaChatApp()
        {
            InitializeComponent();

            //LoadEmbeddedDllAndShowMessage();
            // Original builds read the connection from a DLL appended to the exe.
            // Published source: set CAYACHAT_MONGO (and CAYACHAT_MONGO_DB) instead.
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CAYACHAT_MONGO")))
                LoadEmbeddedDllAndShowMessages();
            //MessageBox.Show(MongoDBconnection);
            //MessageBox.Show(MongoDBconnection);

            MongoDBAddress mdba = MongoDBAddress.Instance;
            mdba.SetMessage(MongoDBconnection, MongoDBDatabase);
            mdba.Show();
            //MongoDBAddress mdba = new MongoDBAddress();
            //mdba.Show();
            //MongoDBAddress2.Message = "Merhaba, Form2!";

            LoadFligranValue();

            CheckLicense();

            LoadBotNameValue();
            LoadBotResponseValue();




            LoadCustomizeSettings();
            ShowLoginForm();
            SetFormPosition();
            LoadMessages();
            LoadOneTimeMessage();

            

            _globalHook = new GlobalKeyboardHook();
            //_globalHook.KeyDown += GlobalHook_KeyDown;//Özel tuş atama
            _proc = HookCallback;
            _hookID = SetHook(_proc);
            KeyCodeC = GetTextMessageMenuKey();

            RegisterHotKey(this.Handle, 1, MOD_CONTROL | MOD_ALT, (int)Keys.T);
            RegisterHotKey(this.Handle, 2, MOD_CONTROL | MOD_ALT, (int)Keys.Enter);
            RegisterHotKey(this.Handle, 3, MOD_CONTROL | MOD_ALT, (int)Keys.O);
            RegisterHotKey(this.Handle, 4, MOD_CONTROL | MOD_ALT, (int)Keys.P);
            RegisterHotKey(this.Handle, 5, MOD_CONTROL | MOD_ALT, (int)Keys.Y);
            SetupTimer();
            MakeFormTransparent();
            EnableClickThrough();

            //MessageBox.Show(_username);
            
        }

        private async void ValidateLicense(string sender, string message)
        {
            int licenseKey;
            string UserIDC = SharedData.UserID.ToString();

            // Lisans anahtarını al ve geçerli bir sayı olup olmadığını kontrol et
            if (!int.TryParse(UserIDC, out licenseKey))
            {
                MessageBox.Show("Lütfen geçerli bir lisans anahtarı (sayı) girin.");
                return;
            }

            try
            {
                //MessageBox.Show(UserIDC);
                //MessageBox.Show("");

                // Lisansı kontrol et
                var licenseStatus = await CheckIDLicenseAsync(licenseKey);

                // Lisans geçerliyse ve yasaklı değilse
                if (licenseStatus.IsOwner)
                {
                    OwnerStatus = true;
                    await MongoDBService.SendMessageAsync(sender, message);
                }
                else
                {
                    OwnerStatus = false;
                    await MongoDBService.SendMessageAsync(sender, message);
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show("AA");
                //MessageBox.Show($"Lisans kontrolü sırasında bir hata oluştu: {ex.Message}");
                if (ex.Message == "Bu ID yasaklı.")
                {
                    //MessageBox.Show("BB");
                    Hide();
                    Blocked bl = new Blocked();
                    bl.ShowDialog();
                }
            }
        }




        



        private void CheckLicense()
        {
            //StartupLisanceManagement sl = new StartupLisanceManagement();
            //sl.ShowDialog();

            StartuplicenceCheck slc = new StartuplicenceCheck();
            slc.ShowDialog();


        }

        private void LoadEmbeddedDllAndShowMessage()
        {
            var exePath = Application.ExecutablePath;
            byte[] dllBytes;
            int dllLength;

            using (var fileStream = new FileStream(exePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(-sizeof(int), SeekOrigin.End);
                    dllLength = reader.ReadInt32();
                    fileStream.Seek(-dllLength - sizeof(int), SeekOrigin.End);
                    dllBytes = reader.ReadBytes(dllLength);
                }
            }

            try
            {
                var dllAssembly = Assembly.Load(dllBytes);

                var type = dllAssembly.GetType("UserMessageNamespace.UserMessage");
                var method = type.GetMethod("GetMessage");
                string message = (string)method.Invoke(null, null);
                //MessageBox.Show(message);
                MongoDBconnection = message;
                
            }
            catch (BadImageFormatException ex)
            {
                //MessageBox.Show($"DLL yükleme hatası: {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"IO hatası: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        private void LoadEmbeddedDllAndShowMessages()
        {
            var exePath = Application.ExecutablePath;
            byte[] dllBytes;
            int dllLength;

            using (var fileStream = new FileStream(exePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(-sizeof(int), SeekOrigin.End);
                    dllLength = reader.ReadInt32();
                    fileStream.Seek(-dllLength - sizeof(int), SeekOrigin.End);
                    dllBytes = reader.ReadBytes(dllLength);
                }
            }

            try
            {
                var dllAssembly = Assembly.Load(dllBytes);

                var type = dllAssembly.GetType("UserMessageNamespace.UserMessages");

                var method1 = type.GetMethod("GetMessage1");
                string message1 = (string)method1.Invoke(null, null);

                var method2 = type.GetMethod("GetMessage2");
                string message2 = (string)method2.Invoke(null, null);

                var method3 = type.GetMethod("GetMessage3");
                string message3 = (string)method3.Invoke(null, null);

                //MessageBox.Show(message);
                //MessageBox.Show(message1);
                //MessageBox.Show(message2);
                //MessageBox.Show(message3);
                MongoDBconnection = message1;
                MongoDBDatabase = message2;
                SharedData.RegisteredUserDataInfo = message3;


            }
            catch (BadImageFormatException ex)
            {
                MessageBox.Show($"DLL yükleme hatası: {ex.Message}");
            }
            catch (IOException ex)
            {
                MessageBox.Show($"IO hatası: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }


        private async void LoadFligranValue()
        {
            //
            try
            {
                string fligranValue = await MongoDBService.GetFligranValueAsync();
                if (!string.IsNullOrEmpty(fligranValue))
                {
                    SharedData.Fligran = fligranValue;
                }
                else
                {
                    //label1.Text = "No data available";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Veri alınırken bir hata oluştu: {ex.Message}");
            }
        }

        private async void LoadBotNameValue()
        {
            //
            try
            {
                string fligranValue = await MongoDBService.GetFligranValueAsync2();
                if (!string.IsNullOrEmpty(fligranValue))
                {
                    SharedData.BotName = fligranValue;
                }
                else
                {
                    //label1.Text = "No data available";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Veri alınırken bir hata oluştu: {ex.Message}");
            }
        }

        private async void LoadBotResponseValue()
        {
            //
            try
            {
                string fligranValue = await MongoDBService.GetFligranValueAsync3();
                if (!string.IsNullOrEmpty(fligranValue))
                {
                    SharedData.DefaultBotResponse = fligranValue;
                    //MessageBox.Show(SharedData.DefaultBotResponse);
                }
                else
                {
                    //label1.Text = "No data available";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Veri alınırken bir hata oluştu: {ex.Message}");
            }
        }

        private void StartVisibilityCheck()
        {
            _visibilityCheckTimer = new System.Windows.Forms.Timer();
            _visibilityCheckTimer.Interval = 50; // 1 saniyede bir kontrol et
            _visibilityCheckTimer.Tick += VisibilityCheckTimer_Tick;
            _visibilityCheckTimer.Start();
        }

        private void VisibilityCheckTimer_Tick(object sender, EventArgs e)
        {
            string windowText = this.Text;

            //MessageBox.Show(_windownamec);
            //MessageBox.Show(windowText);

            string activeAppWindow = GetActiveWindowTitle();
            string activeAppProgram = GetActiveWindowProcessName();

            //MessageBox.Show(activeAppWindow);

            //listBoxMessages.Text = activeAppWindow;

            if (!string.IsNullOrEmpty(_windownamec))
            {

                if (activeAppWindow != windowText)
                {
                    txtMessage.Visible = false;
                }
                //label1.Text = WindowNameGetC; // Veriyi kullan

                //string activeApp = GetActiveWindowTitle();

                if (activeAppProgram != _windownamec && activeAppWindow != _windownamec && activeAppWindow != windowText)
                {
                    this.Visible = false;

                    ProgramWindow = false;
                }
                else
                {
                    this.Visible = true;

                    if (ProgramWindow == false)
                    {
                        ProgramWindow = true;
                        GC.Collect();
                    }
                }

                //else if (activeApp == this.Text)
                //{
                //    ProgramWindow = true;
                //}

            }
            else
            {
                if (activeAppWindow != windowText)
                {
                    txtMessage.Visible = false;
                }
            }

        }

        private static string GetActiveWindowTitle()
        {
            // Get the handle of the active window
            IntPtr handle = GetForegroundWindow();

            // Allocate buffer to store window title
            StringBuilder windowTitle = new StringBuilder(256);

            // Get the title of the active window
            if (GetWindowText(handle, windowTitle, windowTitle.Capacity) > 0)
            {
                return windowTitle.ToString();
            }

            return null;
        }


        private string EncodeTitle(string title)
        {
            byte[] bytes = Encoding.Default.GetBytes(title);
            return Encoding.UTF8.GetString(bytes);
        }


        private string GetActiveWindowProcessName()
        {
            IntPtr handle = GetForegroundWindow();
            StringBuilder title = new StringBuilder(256);
            GetWindowText(handle, title, title.Capacity);
            string windowTitle = title.ToString();

            // Get process ID from window handle
            uint processId;
            GetWindowThreadProcessId(handle, out processId);

            // Get process name from process ID
            string processName = "";
            try
            {
                Process process = Process.GetProcessById((int)processId);
                processName = $"{process.ProcessName}.exe";
            }
            catch
            {
                // Handle cases where the process might not be accessible
            }

            return processName;
        }

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);





        private Keys GetKeyCodeFromString(string keyString)
        {
            // Anahtar dizesini Keys enum değerine dönüştür
            if (Enum.TryParse(keyString, out Keys key))
            {
                return key;
            }
            else
            {
                // Geçersiz anahtar durumunda varsayılan bir değer döndür
                return Keys.None;
            }
        }


        private Keys GetTextMessageMenuKey()
        {
            // _textmessagemenu alanını Keys türüne dönüştür ve döndür
            if (Enum.TryParse(_textmessagemenu, out Keys key))
            {
                return key;
            }
            else
            {
                // Varsayılan bir değer döndür (örneğin, Keys.None)
                return Keys.None;
            }
        }

        private void LoadCustomizeSettings()
        {
            

            if (File.Exists(customizeFilePath))
            {
                var lines = File.ReadAllLines(customizeFilePath);
                if (lines.Length >= 4)
                {
                    txtMessage.BackColor = ColorTranslator.FromHtml(lines[0]);
                    txtMessage.ForeColor = ColorTranslator.FromHtml(lines[1]);
                    listBoxMessages.BackColor = ColorTranslator.FromHtml(lines[2]);
                    listBoxMessages.ForeColor = ColorTranslator.FromHtml(lines[3]);
                    oneTimeText.BackColor = ColorTranslator.FromHtml(lines[0]);
                    oneTimeText.ForeColor = ColorTranslator.FromHtml(lines[1]);
                    oneTimeText.Text = "";
                }
                else
                {
                    SetDefaultColors();
                }
            }
            else
            {
                SetDefaultColors();
            }
        }

        private void SetDefaultColors()
        {
            txtMessage.BackColor = SystemColors.Window;
            txtMessage.ForeColor = SystemColors.WindowText;
            listBoxMessages.BackColor = SystemColors.Window;
            listBoxMessages.ForeColor = SystemColors.WindowText;
            oneTimeText.BackColor = SystemColors.Window;
            oneTimeText.ForeColor = SystemColors.WindowText;
        }

        private void SetupTimer()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000; // 1 saniye
            _timer.Tick += (sender, e) => LoadMessages();
            _timer.Tick += (sender, e) => LoadOneTimeMessage();
            _timer.Start();
        }

        private void SetFormPosition()
        {
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new System.Drawing.Point(screen.Width - this.Width, 0);
        }

        private void MakeFormTransparent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = System.Drawing.Color.Lime;
            this.TransparencyKey = System.Drawing.Color.Lime;
            this.Opacity = 0.8;
        }

        private void EnableClickThrough()
        {
            const int WS_EX_TRANSPARENT = 0x20;
            const int WS_EX_LAYERED = 0x80000;
            int exStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, exStyle | WS_EX_TRANSPARENT | WS_EX_LAYERED);
        }



        ////////////////////
        /// ////////////////////////////////////////////////////////
        ///
        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_KEYUP))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == (int)KeyCodeC) // Tuş kontrolü
                {
                    if (wParam == (IntPtr)WM_KEYDOWN)
                    {
                        // T tuşuna basıldığında yapılacak işlemler
                        //_previousForegroundWindow = GetForegroundWindow();
                        //this.WindowState = FormWindowState.Normal;
                        //SetForegroundWindow(this.Handle);
                        //txtMessage.Visible = true;
                        //txtMessage.Focus();
                    }
                    else if (wParam == (IntPtr)WM_KEYUP)
                    {
                        // T tuşu bırakıldığında yapılacak işlemler
                        // Örneğin: txtMessage.Visible = false;

                        _previousForegroundWindow = GetForegroundWindow();
                        this.WindowState = FormWindowState.Normal;
                        SetForegroundWindow(this.Handle);
                        txtMessage.Visible = true;
                        txtMessage.Focus();
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        //7////////////////////////////////AO
        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(_textmessagemenu);
            if (e.KeyCode == KeyCodeC)
            {
                // T tuşuna basıldığında çalışacak kodu buraya ekleyin
                //MessageBox.Show("T tuşuna basıldı!");

                _previousForegroundWindow = GetForegroundWindow();
                this.WindowState = FormWindowState.Normal;
                SetForegroundWindow(this.Handle);
                txtMessage.Visible = true;
                txtMessage.Focus();
            }
        }


        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY)
            {
                if (m.WParam.ToInt32() == 1)
                {
                    _previousForegroundWindow = GetForegroundWindow();
                    this.WindowState = FormWindowState.Normal;
                    SetForegroundWindow(this.Handle);
                    txtMessage.Visible = true;
                    txtMessage.Focus();
                }
                else if (m.WParam.ToInt32() == 2)
                {
                    _previousForegroundWindow = GetForegroundWindow();
                    this.WindowState = FormWindowState.Normal;
                    SetForegroundWindow(this.Handle);
                    SendOneTimeMessage();
                }
                else if (m.WParam.ToInt32() == 3)
                {
                    if (ToggleOpacity == false)
                    {
                        this.Opacity = 0;
                        ToggleOpacity = true;
                    }
                    else if (ToggleOpacity == true)
                    {
                        this.Opacity = 0.8;
                        ToggleOpacity = false;
                    }

                }
                else if (m.WParam.ToInt32() == 4)
                {
                    if (ToggleWindow == false)
                    {
                        this.FormBorderStyle = FormBorderStyle.Sizable;
                        ToggleWindow = true;
                    }
                    else if (ToggleWindow == true)
                    {
                        MakeFormTransparent();
                        EnableClickThrough();
                        ToggleWindow = false;
                    }

                }
                else if (m.WParam.ToInt32() == 5)
                {
                    Environment.Exit(0);
                }
            }
            base.WndProc(ref m);
        }


        private void ShowChatWindow()
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            txtMessage.Focus();
        }


        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;

        private void ShowLoginForm()
        {
            // StartuplicenceCheck nesnesini oluşturun
            StartuplicenceCheck checkLicense = new StartuplicenceCheck();

            using (LoginForm loginForm = new LoginForm(checkLicense))
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    InitializeUser(loginForm.Username);
                    //_username = loginForm.Username;
                    //_username2 = loginForm.Username;
                    _textmessagemenu = loginForm.TextMessageMenu;
                    _windownamec = loginForm.WindowNameC;
                    txtMessage.BackColor = loginForm.TxtMessageBackColor;
                    txtMessage.ForeColor = loginForm.TxtMessageForeColor;
                    listBoxMessages.BackColor = loginForm.ListBoxMessagesBackColor;
                    listBoxMessages.ForeColor = loginForm.ListBoxMessagesForeColor;

                    KeyCodeC = GetKeyCodeFromString(_textmessagemenu);

                    StartVisibilityCheck();
                }
                else
                {
                    Application.Exit();
                }
            }
        }



        private async void LoadMessages()
        {
            try
            {
                var messages = await MongoDBService.GetMessagesAsync();
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        listBoxMessages.Clear();
                        foreach (var message in messages)
                        {
                            if (message.Username == "System")
                            {
                                listBoxMessages.SelectionStart = listBoxMessages.TextLength;
                                listBoxMessages.SelectionLength = 0;
                                listBoxMessages.SelectionColor = Color.Red; // Metin rengini kırmızı yapar
                            }
                            listBoxMessages.AppendText($"{message.Username} " + ArrowType + $" {message.Message}\n");
                        }
                        ScrollToBottom();
                    }));
                }
                else
                {
                    listBoxMessages.Clear();
                    foreach (var message in messages)
                    {
                        if (message.Username == "System")
                        {
                            listBoxMessages.SelectionStart = listBoxMessages.TextLength;
                            listBoxMessages.SelectionLength = 0;
                            listBoxMessages.SelectionColor = Color.Red; // Metin rengini kırmızı yapar
                        }
                        listBoxMessages.AppendText($"{message.Username} " + ArrowType + $" {message.Message}\n");
                    }
                    ScrollToBottom();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Uygulamada bir hata meydana geldi! Hata mesajı: " + ex.Message);
            }
        }

        private void ScrollToBottom()
        {
            listBoxMessages.ScrollToCaret();
        }

        private async void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {

                //await MongoDBService.SendMessageAsync(_username, txtMessage.Text);
                ValidateLicense(_username, txtMessage.Text);

                // Kullanıcının gönderdiği mesajı alın
                string userMessage = txtMessage.Text.Trim();

                // Mesajın boş olup olmadığını kontrol edin
                if (string.IsNullOrEmpty(userMessage))
                    return;

                // Kullanıcı mesajını RichTextBox'a ekleyin
                //AppendMessage("Kullanıcı", userMessage);

                await MongoDBService.CheckAndInitializeBotStatusAsync();

                bool isBotActive = await MongoDBService.IsBotActiveAsync();

                // Komutları kontrol edin
                if (userMessage.StartsWith("/botON"))
                {
                    if (isBotActive)
                    {
                        AppendMessage("System", "Bot zaten aktif!");
                    }
                    else
                    {
                        await MongoDBService.SetBotStatusAsync(true);
                        AppendMessage("System", "Bot aktif hale getirildi.");
                    }
                }
                else if (userMessage.StartsWith("/botOFF"))
                {
                    if (isBotActive)
                    {
                        await MongoDBService.SetBotStatusAsync(false);
                        AppendMessage("System", "Bot pasif hale getirildi.");
                    }
                    else
                    {
                        AppendMessage("System", "Bot zaten pasif!");
                    }
                }
                else if (userMessage.StartsWith("/addResponse"))
                {
                    // Düzenli ifade şablonu, çift tırnak karakterlerini doğru şekilde kaçırarak
                    var pattern = @"/addResponse\s+""(.+?)""\s+""(.+?)""";
                    var match = Regex.Match(userMessage, pattern);

                    if (match.Success)
                    {
                        string messageToAdd = match.Groups[1].Value;
                        string responseToAdd = match.Groups[2].Value;

                        // BotDataBaseResponse koleksiyonuna yeni yanıt ekleme
                        await MongoDBService.SaveBotResponseToMongoDBAsync(messageToAdd, responseToAdd);

                        // Yanıtın başarıyla eklendiğini bildirin
                        AppendMessage("System", $"Yanıt başarıyla eklendi: \"{messageToAdd}\" -> \"{responseToAdd}\"");
                    }
                    else
                    {
                        // Yanlış komut formatı
                        AppendMessage("System", "Komut formatı hatalı. Lütfen /addResponse \"Kullanıcı Mesajı\" \"Bot Mesajı\" formatını kullanın.");
                    }
                }
                else if (userMessage.StartsWith("/removeResponse"))
                {
                    var pattern = @"/removeResponse\s+""(.+?)""";
                    var match = Regex.Match(userMessage, pattern);

                    if (match.Success)
                    {
                        string messageToRemove = match.Groups[1].Value;

                        try
                        {
                            await MongoDBService.RemoveBotResponseAsync(messageToRemove);
                            AppendMessage("System", $"Yanıt başarıyla silindi: \"{messageToRemove}\"");
                        }
                        catch (Exception ex)
                        {
                            AppendMessage("System", $"Hata: {ex.Message}");
                        }
                    }
                    else
                    {
                        AppendMessage("System", "Komut formatı hatalı. Lütfen /removeResponse \"Kullanıcı Mesajı\" formatını kullanın.");
                    }
                }
                else if (userMessage.StartsWith("/timer"))
                {
                    var pattern = @"/timer\s+(\d+)";
                    var match = Regex.Match(userMessage, pattern);

                    if (match.Success)
                    {
                        int minutes = int.Parse(match.Groups[1].Value);
                        string timerId = Guid.NewGuid().ToString(); // Benzersiz bir ID oluştur

                        TimerService.StartTimer(timerId, minutes, () =>
                        {
                            // Bu bildirim, zamanlayıcı süresi dolduğunda gönderilecektir
                            AppendMessage("System", $"Zamanlayıcı ({minutes} dakika) süresi doldu!");
                        });

                        AppendMessage("System", $"Zamanlayıcı {minutes} dakika olarak ayarlandı.");
                    }
                    else
                    {
                        AppendMessage("System", "Komut formatı hatalı. Lütfen /timer <dakika> formatını kullanın.");
                    }
                }
                else if (userMessage.StartsWith("/clear"))
                {
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    AppendMessage("", "");
                    await Task.Delay(1000);
                    AppendMessage("System", " Sohbet temizlendi! Temizleyen: " + _username);
                }
                else if (userMessage.StartsWith("/ban"))
                {
                    if (OwnerStatus == true)
                    {
                        int banID;

                        // Mesajı '/ban ' komutundan ayır ve ID'yi elde et
                        string[] parts = userMessage.Split(' ');

                        if (parts.Length == 2) // '/ban id' formatı
                        {
                            // ID'yi sayısal bir değere dönüştürmeye çalış
                            if (int.TryParse(parts[1], out banID))
                            {
                                // ID başarıyla ayrıştırıldı, değişkene kaydedildi
                                // Örneğin, SharedData.UserID'ye atayabilirsiniz
                                //SharedData.UserID = banID;
                                //MessageBox.Show($"ID {banID} olarak ayarlandı.");
                                try
                                {
                                    UpdateBanStatusByIdAsync(banID, true);
                                    AppendMessage("System", $" ({banID})ID'li kullanıcı yasaklandı! Yasaklayan: " + _username);
                                }
                                catch (Exception ex) 
                                {
                                    AppendMessage("System", $" (Bir hata meydana geldi! Hata: {ex.Message})");
                                }
                                
                            }
                            else
                            {
                                // ID geçerli bir sayıya dönüştürülemedi
                                //MessageBox.Show("Geçerli bir ID girilmedi.");
                                AppendMessage("System", $" Geçerli bir ID girilmedi.");
                            }
                        }
                        else
                        {
                            // Mesaj formatı uygun değil
                            //MessageBox.Show("Geçersiz komut formatı. Doğru format: /ban [ID]");
                            AppendMessage("System", $" Geçersiz komut formatı. Doğru format: /ban [ID]");
                        }
                    }
                    else
                    {
                        AppendMessage("System", $" Bu komutu kullanabilmek için yetkiniz yetersiz!");
                    }
                    
                }
                else if (userMessage.StartsWith("/unban"))
                {
                    if (OwnerStatus == true)
                    {
                        int unbanID;

                        // Mesajı '/ban ' komutundan ayır ve ID'yi elde et
                        string[] parts = userMessage.Split(' ');

                        if (parts.Length == 2) // '/ban id' formatı
                        {
                            // ID'yi sayısal bir değere dönüştürmeye çalış
                            if (int.TryParse(parts[1], out unbanID))
                            {
                                // ID başarıyla ayrıştırıldı, değişkene kaydedildi
                                // Örneğin, SharedData.UserID'ye atayabilirsiniz
                                //SharedData.UserID = banID;
                                //MessageBox.Show($"ID {banID} olarak ayarlandı.");
                                try
                                {
                                    UpdateBanStatusByIdAsync(unbanID, false);
                                    AppendMessage("System", $" ({unbanID})ID'li kullanıcı yasağı açıldı. Yasağı açan: " + _username);
                                }
                                catch (Exception ex)
                                {
                                    AppendMessage("System", $" (Bir hata meydana geldi! Hata: {ex.Message})");
                                }
                                
                            }
                            else
                            {
                                // ID geçerli bir sayıya dönüştürülemedi
                                //MessageBox.Show("Geçerli bir ID girilmedi.");
                                AppendMessage("System", $" Geçerli bir ID girilmedi.");
                            }
                        }
                        else
                        {
                            // Mesaj formatı uygun değil
                            //MessageBox.Show("Geçersiz komut formatı. Doğru format: /ban [ID]");
                            AppendMessage("System", $" Geçersiz komut formatı. Doğru format: /unban [ID]");
                        }
                    }
                    else
                    {
                        AppendMessage("System", $" Bu komutu kullanabilmek için yetkiniz yetersiz!");
                    }

                }
                else if (userMessage.StartsWith("/help"))
                {
                    AppendMessage("System", "------------------");
                    AppendMessage("System", "Komutlar:");
                    AppendMessage("System", "/botON");
                    AppendMessage("System", "/botOFF");
                    AppendMessage("System", "/addResponse");
                    AppendMessage("System", "/removeResponse");
                    AppendMessage("System", "/timer");
                    AppendMessage("System", "/clear");
                    AppendMessage("System", "/ban");
                    AppendMessage("System", "/unban");
                    AppendMessage("System", "------------------");
                }


                if (SharedData.BotName != null)
                {
                    BotName = SharedData.BotName;
                }

                if (isBotActive)
                {
                    // Chat bot yanıtını oluşturun
                    string botResponse = await GetChatBotResponseAsync(userMessage);
                    // Eğer bot aktifse, yanıtı ekleyin
                    AppendMessage($"{BotName}", botResponse);
                }

                txtMessage.Clear();
                LoadMessages();
            }
        }



        private async void AppendMessage(string sender, string message)
        {
            // RichTextBox'a mesaj ekleyin ve stil ayarları yapın
            if (message != null)
            {
                ValidateLicense(sender, message);
            }
            //listBoxMessages.AppendText($"{sender}: {message}{Environment.NewLine}");
        }


        private async Task<string> GetChatBotResponseAsync(string message)
        {
            string response = await MongoDBService.GetBotResponseAsync(message.ToLower());

            if (SharedData.DefaultBotResponse != null)
            {
                DefaultBotResponse = SharedData.DefaultBotResponse;
            }

            if (DefaultBotResponse == "true")
            {
                // Basit bir yanıt mantığı
                if (message.ToLower() == "merhaba")
                    return "Merhaba! " + _username + " nasılsın?";

                else if (message.ToLower() == "sa")
                    return "Aleyküm selam! Hoşgeldin " + _username + "! 🥳";

                else if (message.ToLower() == "bot nasılsın?")
                    return "İyidir " + _username + " senden naber?";

                else if (message.ToLower() == "bot nasılsın")
                    return "İyidir " + _username + " senden naber?";

                else if (message.ToLower() == "bende iyi işte")
                    return "İyi olmana sevindim!";

                else if (message.ToLower() == "selam")
                    return "Aleyküm selam! Hoşgeldin " + _username + "! 🥳";

                else if (message.ToLower() == "selamün aleyküm")
                    return "Aleyküm selam! Hoşgeldin " + _username + "! 🥳";

                else if (message.ToLower() == "selamın aleyküm")
                    return "Aleyküm selam! Hoşgeldin " + _username + "! 🥳";

                else if (message.ToLower() == "bot")
                    return "Efendim " + _username + "! Size nasıl yardımcı olabilirim?";

                else if (message.ToLower() == "bot sus")
                    return "Efendim! Size nasıl yardımcı olabilirim?";

                else if (message.ToLower() == "kes lan")
                    return "Hey! Sen kes 😎";

                else if (message.ToLower() == "kes")
                    return "Tamam anlıyorum (Çaresizlik)";

                else if (message.ToLower() == "bot kes")
                    return "YOOOK";

                else if (message.ToLower() == "lan kes")
                    return "YOOOOOOOOOOOOOOK";

                else if (message.ToLower() == "la kes")
                    return "Tmm anlıyorum (Çaresizlik)";

                else if (message.ToLower() == "sana demedim")
                    return "Yinede bana demiş gibi hissettim.";

                else if (message.ToLower() == "kes sana demedim")
                    return "Anlıyorum lütfen düzgün konuş.";

                else if (message.ToLower() == "bot sana demedim")
                    return "Yinede bana demiş gibi hissettim.";

                else if (message.ToLower() == "bot sen sus")
                    return "Merhaba! Size nasıl yardımcı olabilirim?";

                else if (message.ToLower() == "enayi bot")
                    return "Oha sen enayimiydin?";

                else if (message.ToLower() == "yok ama sen enayisin")
                    return "Anladım enayi.";

                else if (message.ToLower() == "salak bot")
                    return "Alındım gücendim küstüm.";

                else if (message.ToLower() == "ağlama lan")
                    return "😭😭😭";

                else if (message.ToLower() == "bot ağlama lan")
                    return "😭😭😭";

                else if (message.ToLower() == "bot ağlama la")
                    return "😭😭😭";

                else if (message.ToLower() == "bot ağlama laa")
                    return "😭😭😭";

                else if (message.ToLower() == "ağlama lan bot")
                    return "😭😭😭";

                else if (message.ToLower() == "ağlak bot")
                    return "😭😭😭";

                else if (message.ToLower() == "ağlıyor salak")
                    return "😭😭😭";

                else if (message.ToLower() == "tamam ağlama")
                    return "😭 SEN NE KABA İNSANSIN! @" + _username;

                else if (message.ToLower() == "enai bot")
                    return "Tamam enayi.";

                else
                    //return null; //"Üzgünüm, bu mesajı anlayamıyorum.";
                    return response;
            }
            else
            {
                return response;
            }
            
        }






        private void ChatApp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtMessage.Visible)
            {
                e.SuppressKeyPress = true; // Hata sesini engelle
                btnSendMessage_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape && txtMessage.Visible)
            {
                e.SuppressKeyPress = true; // Hata sesini engelle
                txtMessage.SelectionLength = 0;
                if (_previousForegroundWindow != IntPtr.Zero)
                {
                    SetForegroundWindow(_previousForegroundWindow);
                }
                txtMessage.Visible = false;
            }
            else
            {

            }
        }

        private async void SendOneTimeMessage()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                var oneTimeMessage = $"{_username} " + ArrowType + $" {txtMessage.Text}";
                await MongoDBService.SetOneTimeMessageAsync(oneTimeMessage);
                LoadOneTimeMessage();
                txtMessage.Clear();
            }
        }

        private async void LoadOneTimeMessage()
        {
            try
            {
                // Arka planda tek kullanımlık mesajı yükle
                string oneTimeMessage = await Task.Run(() => MongoDBService.GetOneTimeMessageAsync());

                // Ana iş parçacığında GUI'yi güncelle
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        if (!string.IsNullOrEmpty(oneTimeMessage))
                        {
                            oneTimeText.Text = oneTimeMessage;
                            oneTimeText.Visible = true;
                        }
                        else
                        {
                            oneTimeText.Visible = false;
                        }
                    }));
                }
                else
                {
                    if (!string.IsNullOrEmpty(oneTimeMessage))
                    {
                        oneTimeText.Text = oneTimeMessage;
                        oneTimeText.Visible = true;
                    }
                    else
                    {
                        oneTimeText.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uygulamada bir hata meydana geldi! Hata mesajı: " + ex.Message);
            }
        }



        private void ChatApp_Load(object sender, EventArgs e)
        {
            TopMost = true;
            TopMost = true;
            TopMost = true;
            TopMost = false;
            TopMost = true;
            TopMost = true;
            TopMost = true;

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int formWidth = this.Width;
            int formHeight = this.Height;
            this.Location = new Point(screen.Width - formWidth, 0);

            listBoxMessages.ReadOnly = true;
            listBoxMessages.TabStop = false; // Tab tuşunun geçişini engelleme
        }

    }



}
