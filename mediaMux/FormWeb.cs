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
        public FormWeb()
        {
            InitializeComponent();
            com.init(this);
        }

        public void setUrl(string str)
        {
            this.webBrowser1.Navigate(str);
        }
        private void FormWeb_Load(object sender, EventArgs e)
        {

        }
    }
}
