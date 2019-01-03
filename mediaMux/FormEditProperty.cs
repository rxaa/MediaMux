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
    public partial class FormEditProperty : Form
    {
        public FormEditProperty()
        {
            InitializeComponent();
            com.init(this);

        }
        Action onSaveAct = null;
        public void StartEdit(Action onSave)
        {
            onSaveAct = onSave;
            this.Show();
        }

        FFmpeg ffm;
        object parasObj;
        public void setObj<T>(T obj, FFmpeg ff)
        {
            ffm = ff;
            parasObj = obj;
            propertyGrid1.SelectedObject = parasObj;
            ExpandOne(propertyGrid1);
        }

        private static void ExpandOne(PropertyGrid propertyGrid)
        {
            GridItem root = propertyGrid.SelectedGridItem;
            //Get the parent
            while (root.Parent != null)
                root = root.Parent;

            if (root != null)
            {
                foreach (GridItem g in root.GridItems)
                {
                    foreach (GridItem gg in g.GridItems)
                    {
                        gg.Expanded = true;
                    }
                }
            }
        }


        private void FormJson_Load(object sender, EventArgs e)
        {

        }

        private void FormJson_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = false;
                this.Close();   
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ffm != null)
            {
                var fp = new FormPlayer();
                fp.setFilters(ffm.parameters.filters.getCMD(), ffm.parameters.audio_filters.getCMD());
                fp.play(ffm.fileName);
                fp.Show();
            }
        }

        private void FormEditProperty_FormClosing(object sender, FormClosingEventArgs e)
        {
            onSaveAct?.Invoke();
        }
    }
}
