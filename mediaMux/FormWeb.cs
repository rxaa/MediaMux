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
    public partial class FormWeb : Form
    {
        public FormWeb(bool small = false)
        {
            InitializeComponent();
            if (small)
            {
                Size = new Size(this.Size.Width / 2, (int)(Size.Height / 1.5));
            }

            com.init(this);
        }

        public void setUrl(string str)
        {
            this.webBrowser1.Navigate(str);
        }

        public void setDocumentText(string str)
        {
            this.webBrowser1.DocumentText = str;
        }
        private void FormWeb_Load(object sender, EventArgs e)
        {

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

        }
    }
}
