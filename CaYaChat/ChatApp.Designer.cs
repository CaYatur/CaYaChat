namespace CaYaChat
{
    partial class CaYaChatApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaYaChatApp));
            listBoxMessages = new RichTextBox();
            oneTimeText = new Label();
            txtMessage = new TextBox();
            SuspendLayout();
            // 
            // listBoxMessages
            // 
            listBoxMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxMessages.BackColor = Color.Black;
            listBoxMessages.BorderStyle = BorderStyle.None;
            listBoxMessages.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            listBoxMessages.ForeColor = Color.White;
            listBoxMessages.HideSelection = false;
            listBoxMessages.Location = new Point(12, 12);
            listBoxMessages.Name = "listBoxMessages";
            listBoxMessages.ReadOnly = true;
            listBoxMessages.ScrollBars = RichTextBoxScrollBars.None;
            listBoxMessages.Size = new Size(302, 331);
            listBoxMessages.TabIndex = 0;
            listBoxMessages.TabStop = false;
            listBoxMessages.Text = "";
            // 
            // oneTimeText
            // 
            oneTimeText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            oneTimeText.AutoSize = true;
            oneTimeText.BackColor = Color.Black;
            oneTimeText.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            oneTimeText.ForeColor = Color.White;
            oneTimeText.Location = new Point(12, 368);
            oneTimeText.Name = "oneTimeText";
            oneTimeText.Size = new Size(10, 15);
            oneTimeText.TabIndex = 2;
            oneTimeText.Text = " ";
            // 
            // txtMessage
            // 
            txtMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtMessage.BackColor = Color.Black;
            txtMessage.BorderStyle = BorderStyle.None;
            txtMessage.ForeColor = Color.White;
            txtMessage.Location = new Point(12, 349);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(302, 16);
            txtMessage.TabIndex = 3;
            txtMessage.KeyDown += ChatApp_KeyDown;
            // 
            // CaYaChatApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(326, 386);
            Controls.Add(txtMessage);
            Controls.Add(oneTimeText);
            Controls.Add(listBoxMessages);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "CaYaChatApp";
            Opacity = 0.8D;
            Text = "ChatApp";
            Load += ChatApp_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox txtMessage;
        private RichTextBox txtMessages;
        private RichTextBox listBoxMessages;
        private Label oneTimeText;
        private TextBox textBox1;
    }
}