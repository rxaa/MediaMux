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
    public partial class FormEditProperty : Form
    {
        public FormEditProperty()
        {
            InitializeComponent();
            com.init(this);
            com.resizeButtonImg(button1);
        }
        Action onSaveAct = null;
        public void StartEdit(Action onSave)
        {
            onSaveAct = onSave;
            this.Show();
        }

        FFmpeg ffm;
        List<FFmpeg> ffList;
        object parasObj;
        public void setObj<T>(T obj, FFmpeg ff, List<FFmpeg> ffs)
        {
            ffm = ff;
            parasObj = obj;
            propertyGrid1.SelectedObject = parasObj;
            ffList = ffs;
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

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (ffList != null)
            {

                var str = JsonConvert.SerializeObject(ffm.parameters);
                foreach (var ff in ffList)
                {
                    var prev = ff.parameters;
                    ff.parameters = JsonConvert.DeserializeObject<FileConvertParameter>(str);
                    ff.parameters.fileName = prev.fileName;
                    ff.parameters.loop = prev.loop;
                    ff.parameters.overlay.position = prev.overlay.position;
                    ff.parameters.overlay.shortest = prev.overlay.shortest;
                }
                dfv.msg("ok");
            }
        }
    }
}
