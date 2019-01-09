using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{


    public class SlideText : Control
    {
        private TextBox textBox1;
        private TrackBar trackBar1;

        public SlideText()
        {
            InitializeComponent();
            this.Controls.Add(textBox1);
            this.Controls.Add(trackBar1);
        }

        double toFloat = 1;
        public int getText()
        {
            return (int)Math.Floor(double.Parse(textBox1.Text) * toFloat);
        }

        public void initFromAtt(PropertySlideAttribute att)
        {

            if (att == null)
                return;

            toFloat = att.toFloat;
            trackBar1.Maximum = att.max;
            trackBar1.Minimum = att.min;
            trackBar1.SmallChange = att.step;
            trackBar1.Value = att.defaul;
            if (textBox1.Text != "")
            {
                var val = getText();
                if (val < trackBar1.Minimum || val > trackBar1.Maximum)
                {
                    throw new Exception("Value should lay between " + trackBar1.Minimum / toFloat + " and "+ trackBar1.Maximum / toFloat);
                }
                trackBar1.Value = val;
            }


        }

        public string ValueStr
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }


        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 25);
            this.textBox1.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.Location = new System.Drawing.Point(0, 30);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 56);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // SlideText
            // 
            this.MinimumSize = new System.Drawing.Size(100, 60);
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(100, 60);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = (double)trackBar1.Value / toFloat + "";
        }
    }
}
