namespace CaYaChat
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            txtUsername = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            label2 = new Label();
            HotKeyTM = new TextBox();
            button8 = new Button();
            OwnerButton = new Button();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsername.Location = new Point(12, 36);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(310, 23);
            txtUsername.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(193, 21);
            label1.TabIndex = 1;
            label1.Text = "Kullanıcı Adınızı Giriniz:";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button1.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(12, 65);
            button1.Name = "button1";
            button1.Size = new Size(310, 34);
            button1.TabIndex = 2;
            button1.Text = "Giriş Yap";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnLogin_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new Point(12, 406);
            button2.Name = "button2";
            button2.Size = new Size(310, 34);
            button2.TabIndex = 3;
            button2.Text = "Mesaj Listesi Metin Rengi";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnListBoxMessagesForeColor_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button3.Location = new Point(12, 448);
            button3.Name = "button3";
            button3.Size = new Size(310, 34);
            button3.TabIndex = 4;
            button3.Text = "Mesaj Listesi Rengi";
            button3.UseVisualStyleBackColor = true;
            button3.Click += btnListBoxMessagesBackColor_Click;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button4.Location = new Point(12, 494);
            button4.Name = "button4";
            button4.Size = new Size(310, 34);
            button4.TabIndex = 5;
            button4.Text = "Mesaj Yazma Metin Rengi";
            button4.UseVisualStyleBackColor = true;
            button4.Click += btnTxtMessageForeColor_Click;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button5.Location = new Point(12, 536);
            button5.Name = "button5";
            button5.Size = new Size(310, 34);
            button5.TabIndex = 6;
            button5.Text = "Mesaj Yazma Rengi";
            button5.UseVisualStyleBackColor = true;
            button5.Click += btnTxtMessageBackColor_Click;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button6.BackColor = Color.IndianRed;
            button6.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button6.ForeColor = Color.White;
            button6.Location = new Point(12, 348);
            button6.Name = "button6";
            button6.Size = new Size(310, 43);
            button6.TabIndex = 7;
            button6.Text = "Renk Ayarlarını Sıfırla";
            button6.UseVisualStyleBackColor = false;
            button6.Click += btnResetColors_Click;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button7.BackColor = Color.YellowGreen;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button7.ForeColor = Color.White;
            button7.Location = new Point(13, 308);
            button7.Name = "button7";
            button7.Size = new Size(310, 34);
            button7.TabIndex = 8;
            button7.Text = "Yardım";
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.Location = new Point(-8, 402);
            panel1.Name = "panel1";
            panel1.Size = new Size(349, 42);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = SystemColors.ActiveBorder;
            panel2.Location = new Point(-7, 444);
            panel2.Name = "panel2";
            panel2.Size = new Size(349, 42);
            panel2.TabIndex = 10;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel3.BackColor = SystemColors.ActiveBorder;
            panel3.Location = new Point(-6, 490);
            panel3.Name = "panel3";
            panel3.Size = new Size(349, 42);
            panel3.TabIndex = 11;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BackColor = SystemColors.ActiveBorder;
            panel4.Location = new Point(-6, 532);
            panel4.Name = "panel4";
            panel4.Size = new Size(349, 42);
            panel4.TabIndex = 12;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(12, 227);
            label2.Name = "label2";
            label2.Size = new Size(151, 45);
            label2.TabIndex = 14;
            label2.Text = "Özel Sohbet Açma Tuşu \r\nYalnızca Harf\r\nİsteğe Bağlı Boş Bırakılabilir\r\n";
            // 
            // HotKeyTM
            // 
            HotKeyTM.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            HotKeyTM.Location = new Point(13, 275);
            HotKeyTM.MaxLength = 1;
            HotKeyTM.Name = "HotKeyTM";
            HotKeyTM.Size = new Size(309, 23);
            HotKeyTM.TabIndex = 15;
            HotKeyTM.TextChanged += HotKeyTM_TextChanged;
            HotKeyTM.KeyPress += HotKeyTM_KeyPress;
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button8.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            button8.Location = new Point(12, 105);
            button8.Name = "button8";
            button8.Size = new Size(310, 49);
            button8.TabIndex = 16;
            button8.Text = "Sohbeti Yalnızca Seçilen Uygulamada Görüntüle";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // OwnerButton
            // 
            OwnerButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OwnerButton.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            OwnerButton.Location = new Point(12, 160);
            OwnerButton.Name = "OwnerButton";
            OwnerButton.Size = new Size(310, 49);
            OwnerButton.TabIndex = 17;
            OwnerButton.Text = "Yönetici Ayarları";
            OwnerButton.UseVisualStyleBackColor = true;
            OwnerButton.Visible = false;
            OwnerButton.Click += OwnerButton_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(336, 582);
            Controls.Add(OwnerButton);
            Controls.Add(button8);
            Controls.Add(HotKeyTM);
            Controls.Add(label2);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(txtUsername);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(panel4);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Giriş yapın";
            FormClosing += LoginForm_FormClosing;
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUsername;
        private Label label1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Label label2;
        private TextBox HotKeyTM;
        private Button button8;
        private Button OwnerButton;
    }
}