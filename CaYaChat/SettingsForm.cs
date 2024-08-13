using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CaYaChat
{
    // SettingsForm.cs
    public partial class SettingsForm : Form
    {
        public string SelectedAppName { get; private set; }

        private bool AppName = true;

        public SettingsForm()
        {
            InitializeComponent();
            LoadOpenApplications();
        }

        private void LoadOpenApplications()
        {
            // Initialize ListBox and Button
            //ListBox appListBox = new ListBox();
            //appListBox.Dock = DockStyle.Fill;
            appListBox.SelectedIndexChanged += AppListBox_SelectedIndexChanged;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            //Controls.Add(appListBox);

            //Button saveButton = new Button();
            //saveButton.Text = "Kaydet";
            //saveButton.Dock = DockStyle.Bottom;
            saveButton.Click += SaveButton_Click;
            //Controls.Add(saveButton);

            // Load applications
            IEnumerable<string> applications = GetOpenApplications();
            foreach (var app in applications)
            {
                appListBox.Items.Add(app);
            }

            IEnumerable<string> applications2 = GetOpenApplicationTitles();
            foreach (var app in applications2)
            {
                listBox1.Items.Add(app);
            }
        }

        private IEnumerable<string> GetOpenApplicationTitles()
        {
            List<string> titles = new List<string>();

            foreach (Process p in Process.GetProcesses())
            {
                try
                {
                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        titles.Add(p.MainWindowTitle);
                    }
                }
                catch
                {
                    // Handle cases where process access is denied or other issues
                }
            }

            return titles;
        }

        private void AppListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear selection in listBox1
            if (listBox1.SelectedIndex != 0)
            {
                if (AppName == true)
                {
                    listBox1.ClearSelected();
                }
            }

            ListBox listBox = sender as ListBox;
            SelectedAppName = listBox.SelectedItem as string;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear selection in appListBox
            if (appListBox.SelectedIndex != 0)
            {
                if (AppName == false)
                {
                    appListBox.ClearSelected();
                }  
            }

            ListBox listBox = sender as ListBox;
            SelectedAppName = listBox.SelectedItem as string;
            // Handle listBox1 selection change if needed
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedAppName))
            {
                MessageBox.Show("Lütfen bir uygulama seçin.");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private IEnumerable<string> GetOpenApplications()
        {
            List<string> applications = new List<string>();
            foreach (Process process in Process.GetProcesses())
            {
                try
                {
                    string windowTitle = process.MainWindowTitle;
                    if (!string.IsNullOrEmpty(windowTitle))
                    {
                        // Handle unsupported characters by encoding the title
                        string encodedTitle = EncodeTitle(windowTitle);
                        applications.Add($"{process.ProcessName}.exe");
                    }
                }
                catch
                {
                    // Process might not have access or could be in an invalid state
                }
            }
            return applications;
        }

        private string EncodeTitle(string title)
        {
            byte[] bytes = Encoding.Default.GetBytes(title);
            return Encoding.UTF8.GetString(bytes);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checked(checkBox1.Checked))
            {
                checkBox1.Text = "Yalnızca Belirlenen Uygulamada Göster";
                appListBox.Visible = false;
                AppName = false;
            }
            else
            {
                checkBox1.Text = "Yalnızca Belirlenen Pencerede Göster";
                appListBox.Visible = true;
                AppName = true;
            }
        }
    }
}
