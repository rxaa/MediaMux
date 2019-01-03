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
    public partial class FormEditTitle : Form
    {
        public ConvertMedia convert = new ConvertMedia();
        IList<FFmpeg> ffList = null;

        public void toCursor()
        {
            var poi = this.PointToClient(Cursor.Position);
            StartPosition = FormStartPosition.Manual;
            Location = poi;
        }

        public FormEditTitle()
        {
            InitializeComponent();
            com.init(this);
        }


        public string Title
        {
            get
            {
                return textBoxTitle.Text;
            }
            set
            {
                textBoxTitle.Text = value;
            }
        }


        public void setObj(ConvertMedia obj, IList<FFmpeg> ffs)
        {
            ffList = ffs;
            convert = obj;
            propertyGrid1.SelectedObject = convert;
        }


        void init()
        {
            listBoxCfg.Items.Clear();
            com.codes.dat.list.ForEach(it =>
            {
                listBoxCfg.Items.Add(it.Key);
            });
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (this.textBoxTitle.Text == "")
            {
                dfv.msgERR(com.lang.dat.Please_input_title);
                return;
            }

            try
            {

                com.codes.dat.list[textBoxTitle.Text] = convert;
                com.codes.save();
                init();
            }
            catch (Exception err)
            {
                dfv.msgERR(com.lang.dat.Json_error + "\r\n" + err.Message);
                return;
            }
        }


        bool hasSelected = false;
        public bool startSelect()
        {
            this.ShowDialog();
            return hasSelected;
        }

        private void listBoxCfg_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxCfg.SelectedIndex < 0)
                return;


            hasSelected = true;
            this.Close();
        }

        private void listBoxCfg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxCfg.SelectedIndex < 0)
                return;

            var key = listBoxCfg.SelectedItem + "";

            com.codes.dat.list.GetVal(key, res =>
            {
                textBoxTitle.Text = key;
                setObj(JsonConvert.DeserializeAnonymousType(JsonConvert.SerializeObject(res), res), ffList);
            });
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBoxCfg.SelectedIndex < 0)
                return;

            var key = listBoxCfg.SelectedItem + "";
            com.codes.dat.list.Remove(key);
            com.codes.save();
            listBoxCfg.Items.RemoveAt(listBoxCfg.SelectedIndex);

        }



        private void FormEditTitle_Load(object sender, EventArgs e)
        {
            init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hasSelected = true;
            this.Close();
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {

            foreach (var ff in ffList)
            {
                if (ff.info.streams.Sum(it => (it.isVideo() || it.isAudio()) ? 1 : 0) > 0)
                {
                    var fp = new FormPlayer();
                    var vf = dfv.joinStr(",", convert.video_filters.getCMD(), FFmpeg.getFilesSubtitle(ffList));
                    fp.setFilters(vf, convert.audio_filters.getCMD());
                    fp.play(ff.fileName);

                    fp.Show();
                    break;
                }
            }

        }
    }
}
