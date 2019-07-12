using df;
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
    public partial class FormCommand : Form
    {
        public FormCommand()
        {
            InitializeComponent();
            com.init(this);
            com.initFont(richTextBox1);
            com.resizeButtonImg(buttonStart);
        }

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

        public bool start = false;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            start = true;
            Close();
        }

        private void FormCommand_Load(object sender, EventArgs e)
        {
            richTextBox1.AutoWordSelection = false;
        }
    }
}
