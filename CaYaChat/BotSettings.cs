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

namespace CaYaChat
{
    public partial class BotSettings : Form
    {
        private bool _suppressCheckBoxEvent = false;

        public BotSettings()
        {
            InitializeComponent();
        }

        private void BotSettings_Load(object sender, EventArgs e)
        {
            Theme();

            if (SharedData.BotName == null)
            {
                label1.Text = "CaYaBot";
            }
            else
            {
                label1.Text = SharedData.BotName;
            }

            if (SharedData.DefaultBotResponse == "true")
            {
                _suppressCheckBoxEvent = true; // Olay tetiklenmesini engelle
                checkBox1.CheckState = CheckState.Checked;
                _suppressCheckBoxEvent = false; // Olay tetiklenmesini tekrar etkinleştir
            }
            else
            {
                _suppressCheckBoxEvent = true; // Olay tetiklenmesini engelle
                checkBox1.CheckState = CheckState.Unchecked;
                _suppressCheckBoxEvent = false; // Olay tetiklenmesini tekrar etkinleştir
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
                    await MongoDBService.SaveFligranValueAsync2(valueToSave);
                    SharedData.BotName = textBox1.Text;
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
                await MongoDBService.DeleteFligranAsync2();
                SharedData.BotName = null;
                MessageBox.Show("Değer başarıyla silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Değer silinirken bir hata oluştu: {ex.Message}");
            }
        }


        private async void SaveBotResponse()
        {
            string valueToSave = "true";

            if (!string.IsNullOrEmpty(valueToSave))
            {
                try
                {
                    await MongoDBService.SaveFligranValueAsync3(valueToSave);
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

        private async void DeleteBotResponse()
        {
            try
            {
                await MongoDBService.DeleteFligranAsync3();
                MessageBox.Show("Değer başarıyla silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Değer silinirken bir hata oluştu: {ex.Message}");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressCheckBoxEvent)
                return;

            if (checkBox1.Checked)
            {
                SaveBotResponse();
                SharedData.DefaultBotResponse = "true";
            }
            else
            {
                DeleteBotResponse();
                SharedData.DefaultBotResponse = "false";
            }
        }
    }
}
