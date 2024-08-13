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
    public partial class MongoDBAddress : Form
    {
        private static MongoDBAddress _instance;
        public static MongoDBAddress Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new MongoDBAddress();
                }
                return _instance;
            }
        }

        private static string NameFC = "";
        private static string NameCD = "";

        public void SetMessage(string message, string message2)
        {
            
            //NameFC = "";
            //NameCD = "CaYaChatSystem";

            NameFC = message;
            NameCD = message2;

            UpdateConnection();
        }

        public MongoDBAddress()
        {
            InitializeComponent();
        }

        public static string Message { get; set; }
        public static string ConnectionString { get; private set; }
        public static string DatabaseName { get; private set; }
        //public static string DatabaseName = "CaYaChatSystem";

        private void MongoDBAddress_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde ConnectionString'i güncelle
            //UpdateConnection();
        }

        private static void UpdateConnection()
        {
            // Published version: credentials are no longer embedded; read from environment variables if set.
            var envConn = Environment.GetEnvironmentVariable("CAYACHAT_MONGO");
            if (!string.IsNullOrEmpty(envConn)) NameFC = envConn;
            var envDb = Environment.GetEnvironmentVariable("CAYACHAT_MONGO_DB");
            if (!string.IsNullOrEmpty(envDb)) NameCD = envDb;

            if (!string.IsNullOrEmpty(NameFC))
            {
                ConnectionString = NameFC;
            }
            else
            {
                // Boş olduğunda yapılacak işlem
                ConnectionString = string.Empty; // veya uygun bir varsayılan değer
            }

            if (!string.IsNullOrEmpty(NameCD))
            {
                DatabaseName = NameCD;
            }
            else
            {
                // Boş olduğunda yapılacak işlem
                DatabaseName = string.Empty; // veya uygun bir varsayılan değer
            }
        }
    }
}
