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
    public partial class FormDetails : Form
    {
        public FormDetails()
        {
            InitializeComponent();
            com.init(this);
            com.initFont(richTextBox1);
        }

        public void setText(string str)
        {
            richTextBox1.Text = str;
        }

        public void addText(string str, Color c, Font f = null)
        {

            var start = richTextBox1.Text.Length;

            richTextBox1.AppendText(str);
            richTextBox1.Select(start, str.Length);
            richTextBox1.SelectionColor = c;
            if (f != null)
                richTextBox1.SelectionFont = f;
            richTextBox1.Select(0,0);

        }
        private void FormDetails_Load(object sender, EventArgs e)
        {
            richTextBox1.AutoWordSelection = false;
        }
    }
}
