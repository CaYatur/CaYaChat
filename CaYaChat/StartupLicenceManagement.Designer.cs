namespace CaYaChat
{
    partial class StartupLicenceManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupLicenceManagement));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            richTextBox1 = new RichTextBox();
            checkBoxIsOwner = new CheckBox();
            checkBoxIsBanned = new CheckBox();
            button6 = new Button();
            button7 = new Button();
            label1 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            richTextBox3 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            button8 = new Button();
            button9 = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(678, 415);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(110, 23);
            textBox1.TabIndex = 0;
            textBox1.Text = "Lisans";
            textBox1.Visible = false;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.ActiveCaptionText;
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point);
            textBox2.ForeColor = SystemColors.Control;
            textBox2.Location = new Point(121, 28);
            textBox2.MaxLength = 15;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(138, 29);
            textBox2.TabIndex = 1;
            textBox2.KeyPress += textBox2_KeyPress;
            // 
            // button1
            // 
            button1.Location = new Point(678, 370);
            button1.Name = "button1";
            button1.Size = new Size(110, 39);
            button1.TabIndex = 2;
            button1.Text = "Lisansı Kontrol et";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += btnCheckLicense_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ActiveCaptionText;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = SystemColors.Control;
            button2.Location = new Point(6, 63);
            button2.Name = "button2";
            button2.Size = new Size(253, 41);
            button2.TabIndex = 3;
            button2.Text = "Girilen ID'li lisans anahtarını yasakla";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnBanID_Click;
            // 
            // button3
            // 
            button3.BackColor = SystemColors.ActiveCaptionText;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button3.ForeColor = SystemColors.Control;
            button3.Location = new Point(6, 201);
            button3.Name = "button3";
            button3.Size = new Size(253, 37);
            button3.TabIndex = 4;
            button3.Text = "Girilen ID'nin yasağını kaldır";
            button3.UseVisualStyleBackColor = false;
            button3.Click += btnUnbanID_Click;
            // 
            // button4
            // 
            button4.BackColor = SystemColors.ActiveCaptionText;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button4.ForeColor = SystemColors.Control;
            button4.Location = new Point(6, 158);
            button4.Name = "button4";
            button4.Size = new Size(253, 41);
            button4.TabIndex = 5;
            button4.Text = "Girilen ID'nin tüm verilerini göster";
            button4.UseVisualStyleBackColor = false;
            button4.Click += btnCheckID_Click;
            // 
            // button5
            // 
            button5.BackColor = SystemColors.ActiveCaptionText;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button5.ForeColor = SystemColors.Control;
            button5.Location = new Point(688, 341);
            button5.Name = "button5";
            button5.Size = new Size(100, 23);
            button5.TabIndex = 6;
            button5.Text = "Yenile";
            button5.UseVisualStyleBackColor = false;
            button5.Visible = false;
            button5.Click += btnListLicenses_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.BackColor = SystemColors.ActiveCaptionText;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox1.ForeColor = SystemColors.Window;
            richTextBox1.Location = new Point(12, 281);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.Size = new Size(678, 220);
            richTextBox1.TabIndex = 7;
            richTextBox1.Text = "";
            // 
            // checkBoxIsOwner
            // 
            checkBoxIsOwner.AutoSize = true;
            checkBoxIsOwner.BackColor = Color.Transparent;
            checkBoxIsOwner.ForeColor = SystemColors.Control;
            checkBoxIsOwner.Location = new Point(6, 85);
            checkBoxIsOwner.Name = "checkBoxIsOwner";
            checkBoxIsOwner.Size = new Size(99, 19);
            checkBoxIsOwner.TabIndex = 8;
            checkBoxIsOwner.Text = "Yönetici hakkı";
            checkBoxIsOwner.UseVisualStyleBackColor = false;
            // 
            // checkBoxIsBanned
            // 
            checkBoxIsBanned.AutoSize = true;
            checkBoxIsBanned.BackColor = Color.Transparent;
            checkBoxIsBanned.ForeColor = SystemColors.Control;
            checkBoxIsBanned.Location = new Point(6, 167);
            checkBoxIsBanned.Name = "checkBoxIsBanned";
            checkBoxIsBanned.Size = new Size(61, 19);
            checkBoxIsBanned.TabIndex = 9;
            checkBoxIsBanned.Text = "Yasaklı";
            checkBoxIsBanned.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            button6.BackColor = SystemColors.ActiveCaptionText;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button6.ForeColor = SystemColors.Control;
            button6.Location = new Point(6, 199);
            button6.Name = "button6";
            button6.Size = new Size(253, 41);
            button6.TabIndex = 10;
            button6.Text = "Yeni bir lisans anahtarı oluştur";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnAddLicense_Click;
            // 
            // button7
            // 
            button7.BackColor = SystemColors.ActiveCaptionText;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button7.ForeColor = SystemColors.Control;
            button7.Location = new Point(6, 110);
            button7.Name = "button7";
            button7.Size = new Size(253, 41);
            button7.TabIndex = 11;
            button7.Text = "Girilen ID'li lisans anahatını sil";
            button7.UseVisualStyleBackColor = false;
            button7.Click += btnDeleteLicense_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(6, 34);
            label1.Name = "label1";
            label1.Size = new Size(109, 15);
            label1.TabIndex = 12;
            label1.Text = "Kullanıcı ID giriniz:";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(button7);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(button4);
            groupBox1.Controls.Add(button3);
            groupBox1.ForeColor = SystemColors.Control;
            groupBox1.Location = new Point(12, 13);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(265, 248);
            groupBox1.TabIndex = 14;
            groupBox1.TabStop = false;
            groupBox1.Text = "İşlem yapmak istediğiniz kullanıcı ID'sini girin.";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox2.BackColor = Color.Transparent;
            groupBox2.Controls.Add(richTextBox3);
            groupBox2.Controls.Add(richTextBox2);
            groupBox2.Controls.Add(button6);
            groupBox2.Controls.Add(checkBoxIsBanned);
            groupBox2.Controls.Add(checkBoxIsOwner);
            groupBox2.ForeColor = SystemColors.Control;
            groupBox2.Location = new Point(425, 13);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(265, 248);
            groupBox2.TabIndex = 15;
            groupBox2.TabStop = false;
            groupBox2.Text = "Yeni bir lisans anahtarı oluştur";
            // 
            // richTextBox3
            // 
            richTextBox3.BackColor = SystemColors.ActiveCaptionText;
            richTextBox3.BorderStyle = BorderStyle.None;
            richTextBox3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox3.ForeColor = SystemColors.Control;
            richTextBox3.Location = new Point(6, 110);
            richTextBox3.Name = "richTextBox3";
            richTextBox3.ReadOnly = true;
            richTextBox3.Size = new Size(253, 51);
            richTextBox3.TabIndex = 17;
            richTextBox3.Text = "Yasaklı:\nBu lisansı başlangıçta yasaklar ve yasak açılana kadar giriş sağlamayı engeller.";
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = SystemColors.ActiveCaptionText;
            richTextBox2.BorderStyle = BorderStyle.None;
            richTextBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            richTextBox2.ForeColor = SystemColors.Control;
            richTextBox2.Location = new Point(6, 28);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(253, 51);
            richTextBox2.TabIndex = 16;
            richTextBox2.Text = "Yönetici hakkı:\nBu lisansa sahip birinin tüm programdaki lisansları ve yasaklamaları yönetmesini sağlar.";
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Top;
            button8.BackColor = SystemColors.ActiveCaptionText;
            button8.FlatStyle = FlatStyle.Flat;
            button8.ForeColor = Color.White;
            button8.Location = new Point(283, 22);
            button8.Name = "button8";
            button8.Size = new Size(136, 40);
            button8.TabIndex = 16;
            button8.Text = "Özel Filigran Ayarla";
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Anchor = AnchorStyles.Top;
            button9.BackColor = SystemColors.ActiveCaptionText;
            button9.FlatStyle = FlatStyle.Flat;
            button9.ForeColor = Color.White;
            button9.Location = new Point(283, 68);
            button9.Name = "button9";
            button9.Size = new Size(136, 40);
            button9.TabIndex = 17;
            button9.Text = "Bot Ayarları";
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // StartupLicenceManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDarkDark;
            ClientSize = new Size(702, 509);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(richTextBox1);
            Controls.Add(button5);
            Controls.Add(button1);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "StartupLicenceManagement";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Lisans Ayarları";
            Load += StartupLisance_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private RichTextBox richTextBox1;
        private CheckBox checkBoxIsOwner;
        private CheckBox checkBoxIsBanned;
        private Button button6;
        private Button button7;
        private Label label1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox2;
        private Button button8;
        private Button button9;
    }
}