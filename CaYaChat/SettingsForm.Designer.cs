namespace CaYaChat
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            appListBox = new ListBox();
            saveButton = new Button();
            listBox1 = new ListBox();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // appListBox
            // 
            appListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            appListBox.FormattingEnabled = true;
            appListBox.ItemHeight = 15;
            appListBox.Location = new Point(12, 42);
            appListBox.Name = "appListBox";
            appListBox.Size = new Size(335, 274);
            appListBox.TabIndex = 0;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            saveButton.Location = new Point(12, 329);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(335, 49);
            saveButton.TabIndex = 1;
            saveButton.Text = "Kaydet";
            saveButton.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 42);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(335, 274);
            listBox1.TabIndex = 2;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 17);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(218, 19);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "Yalnızca Belirlenen Pencerede Göster";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(359, 390);
            Controls.Add(checkBox1);
            Controls.Add(saveButton);
            Controls.Add(appListBox);
            Controls.Add(listBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sadece Belirli Uygulamada Göster";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox appListBox;
        private Button saveButton;
        private ListBox listBox1;
        private CheckBox checkBox1;
    }
}