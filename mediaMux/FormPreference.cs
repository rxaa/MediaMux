using df;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaMux
{
    public partial class FormPreference : Form
    {
        public FormPreference()
        {
            InitializeComponent();
            com.init(this);

            com.resizeButtonImg(buttonOk);
        }

        private void FormPreference_Load(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = com.cfg.dat;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            com.cfg.save();
            Close();
        }

        private void comboBoxFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            com.initFont(this);
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var conver = e.ChangedItem.PropertyDescriptor;
            if (conver.Name == "font_ui")
            {
                com.initFont(this);
            }
        }
    }
}
