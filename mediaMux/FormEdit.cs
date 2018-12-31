using df;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaMux
{
    public partial class FormEdit : Form
    {
        public FormEdit()
        {
            InitializeComponent();
            com.init(this);
            com.initFont(richTextBox1);

           
        }

        public bool verifyJson = false;

        public string EditText
        {
            get
            {
                return richTextBox1.Text;
            }
            set
            {
                richTextBox1.Text = value;
            }
        }

        bool save = false;
        public bool StartEdit()
        {
            this.ShowDialog();
            return save;
        }

        public void setJSON<T>(T obj)
        {
            verifyJson = true;
            EditText = JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (verifyJson)
            {
                try
                {
                    JsonConvert.DeserializeObject(richTextBox1.Text);
                }
                catch (Exception err)
                {
                    dfv.msgERR(com.lang.dat.Json_error + "\r\n" + err.Message);
                    return;
                }
            }
            save = true;
            Close();
        }

        private void FormEdit_Load(object sender, EventArgs e)
        {
            richTextBox1.AutoWordSelection = false;
        }
    }
}
