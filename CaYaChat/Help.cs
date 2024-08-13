using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaYaChat
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            //label1.Text = SharedData.RegisteredUserDataInfo;
            if (SharedData.Fligran == null)
            {
                label1.Text = SharedData.RegisteredUserDataInfo;
            }
            else
            {
                label1.Text = SharedData.Fligran;
            }
        }
    }
}
