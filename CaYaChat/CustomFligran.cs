using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace CaYaChat
{
    public partial class CustomFligran : Form
    {
        public CustomFligran()
        {
            InitializeComponent();
        }

        private void CustomFligran_Load(object sender, EventArgs e)
        {
            Theme();

            if (SharedData.Fligran == null)
            {
                label1.Text = SharedData.RegisteredUserDataInfo;
            }
            else
            {
                label1.Text = SharedData.Fligran;
            }
        }

        private void Theme()
        {
            this.BackColor = ColorTranslator.FromHtml("#212121");
            button1.BackColor = ColorTranslator.FromHtml("#212121");
            button2.BackColor = ColorTranslator.FromHtml("#212121");
            textBox1.BackColor = ColorTranslator.FromHtml("#525252");
        }


        private async void saveButton_Click(object sender, EventArgs e)
        {
            string valueToSave = textBox1.Text;

            if (!string.IsNullOrEmpty(valueToSave))
            {
                try
                {
                    await MongoDBService.SaveFligranValueAsync(valueToSave);
                    MessageBox.Show("Değer başarıyla kaydedildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Değer kaydedilirken bir hata oluştu: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Kaydedilecek değer girilmedi.");
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                await MongoDBService.DeleteFligranAsync();
                MessageBox.Show("Değer başarıyla silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Değer silinirken bir hata oluştu: {ex.Message}");
            }
        }


    }
}
