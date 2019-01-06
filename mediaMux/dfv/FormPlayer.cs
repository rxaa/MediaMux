using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{
    public partial class FormPlayer : Form
    {

        public static Action<FormPlayer> onInit = null;


        public FormPlayer()
        {
            InitializeComponent();
            onInit?.Invoke(this);

            AppLanguage.InitLanguage(contextMenuStrip1);


        }


        int getCropVal(int val)
        {
            return (int)Math.Ceiling(val * cropScale) & ~1;
        }



        void showCropInfo()
        {
            richTextBox1.Text = getSelectedRectStr();
        }

        public Rectangle getSelectedRect()
        {
            return new Rectangle(getCropVal(imageCroppingBox1.SelectedRectangle.X)
                , getCropVal(imageCroppingBox1.SelectedRectangle.Y)
                    , getCropVal(imageCroppingBox1.SelectedRectangle.Width)
                    , getCropVal(imageCroppingBox1.SelectedRectangle.Height));
        }

        public string getSelectedRectStr()
        {
            return "{x:" + getCropVal(imageCroppingBox1.SelectedRectangle.X)
               + ",y:" + getCropVal(imageCroppingBox1.SelectedRectangle.Y)
               + ",w:" + getCropVal(imageCroppingBox1.SelectedRectangle.Width)
               + ",h:" + getCropVal(imageCroppingBox1.SelectedRectangle.Height) + "}";
        }

        public bool cropStart(string file)
        {
            play(file);
            cropStart();
            ShowDialog();
            if (imageCroppingBox1.IsDrawed && selectedTime != "")
            {
                return true;
            }
            return false;
        }

        bool isDragging = false;
        private void FormPlayer_Load(object sender, EventArgs e)
        {
            imageCroppingBox1._Image = pictureBox1;
            imageCroppingBox1.onMove = me =>
            {
                showCropInfo();
            };

            labelTime.Text = "";
            this.myProgressBar1.addControlMove(labelTime);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);

            myProgressBar1.onMove = () =>
            {
                isDragging = true;

                FFplay.ffplay_set_position(FFplay.ffplay_get_duration() * myProgressBar1.Value / myProgressBar1.Maximum);
            };

            myProgressBar1.onUp = () =>
            {
                isDragging = false;
            };

            if (onLoad != null)
                this.BeginInvoke(onLoad);
        }

        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                FFplay.ffplay_set_position(FFplay.ffplay_get_position() - 10000);
            }
            else
            {
                FFplay.ffplay_set_position(FFplay.ffplay_get_position() + 10000);
            }
        }

        public void setFilters(string vf, string af)
        {
            if (vf != "")
                FFplay.ffplay_set_vf(vf);
            if (af != "")
                FFplay.ffplay_set_af(af);
        }

        Action onLoad = null;

        volatile string errorStr = "";

        void showErr(string err)
        {
            richTextBox1.Text = err;
            richTextBox1.Select(0, richTextBox1.Text.Length);
            richTextBox1.SelectionColor = Color.Red;
        }

        ActionString onErrorAction = null;

        void onError(string err)
        {
            errorStr += err;
            this.BeginInvoke(new Action(() =>
            {
                showErr(errorStr);
            }));
        }





        public string selectTime(string file)
        {
            play(file);
            ShowDialog();
            return selectedTime;
        }

        public void play(string name)
        {
            this.Text = Path.GetFileName(name);
            richTextBox1.Text = this.Text;
            onErrorAction = onError;

            onLoad = new Action(() =>
             {
                 try
                 {
                     FFplay.ffplay_on_error(onErrorAction);
                     FFplay.play(name
                                , pictureBox1.Handle
                                , this);
                     timer1.Start();
                 }
                 catch (Exception e)
                 {
                     timer1.Stop();
                     dfv.msgERR(e.Message);
                 }
             });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FFplay.ffplay_step_to_next_frame();
        }

        private void FormPlayer_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void FormPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {

            timer1.Stop();
            FFplay.stop();
        }



        private void FormPlayer_Resize(object sender, EventArgs e)
        {
            resizeW = 0;
            resizeH = 0;

        }
        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            cropResize();
        }

        int resizeW = 0;
        int resizeH = 0;
        private void FormPlayer_ResizeBegin(object sender, EventArgs e)
        {
            resizeW = Size.Width;
            resizeH = Size.Height;
            FFplay.ffplay_set_stop_show(1);
        }

        private void FormPlayer_ResizeEnd(object sender, EventArgs e)
        {
            FFplay.ffplay_set_stop_show(0);
        }

        private void FormPlayer_SizeChanged(object sender, EventArgs e)
        {
        }

        private void FormPlayer_Move(object sender, EventArgs e)
        {
            if (Size.Width == resizeW && resizeH == Size.Height)
                FFplay.ffplay_set_stop_show(0);
        }

        private void FormPlayer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                FFplay.ffplay_toggle_pause();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F)
            {
                FFplay.ffplay_step_to_next_frame();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                FFplay.ffplay_set_position(FFplay.ffplay_get_position() - 10000);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                FFplay.ffplay_set_position(FFplay.ffplay_get_position() + 10000);
                e.Handled = true;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                FFplay.ffplay_toggle_pause();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            FFplay.ffplay_toggle_pause();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                var dur = FFplay.ffplay_get_duration();


                var state = FFplay.ffplay_get_state();
                if (state == 1)
                {
                    buttonPause.Text = dfv.lang.dat.Pause;
                }
                else
                {
                    buttonPause.Text = dfv.lang.dat.Play;
                }

                var time = FFplay.ffplay_get_position();
                labelTime.Text = dfv.timeToStr2(time) + " / " + dfv.timeToStr2(dur);

                if (dur < 1 || isDragging || state == 0)
                    return;


                myProgressBar1.setValue((int)(time * 1000 / dur));
            }
            catch (Exception err)
            {
                timer1.Stop();
                dfv.msgERR(err.Message);
            }
        }

        string selectedTime = "";
        private void button1_Click_1(object sender, EventArgs e)
        {
            selectedTime = dfv.timeToStr2(FFplay.ffplay_get_position(), false);
            this.Close();
        }

        private void steptonextframeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FFplay.ffplay_step_to_next_frame();
        }

        private void copytimestapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dfv.SetClipboard(dfv.timeToStr2(FFplay.ffplay_get_position(), false));
        }


        int _width = 0;
        int _height = 0;
        double cropScale = 0;
        void cropResize()
        {
            if (!imageCroppingBox1.Visible)
                return;

            int scr_width = pictureBox1.Width;
            int scr_height = pictureBox1.Height;
            var aspect_ratio = FFplay.ffplay_get_aspect_ratio();
            _width = FFplay.ffplay_get_w();
            _height = FFplay.ffplay_get_h();
            int width = _width, height = _height;

            if (width < 1 || height < 1)
                return;

            height = scr_height;
            width = (int)Math.Ceiling(height * aspect_ratio) & ~1;
            if (width > scr_width)
            {
                width = scr_width;
                height = (int)Math.Ceiling(width / aspect_ratio) & ~1;
            }
            int x = (scr_width - width) / 2;
            int y = (scr_height - height) / 2;

            var oldScale = cropScale;
            cropScale = (double)_width / width;
            var newScale = oldScale / cropScale;


            imageCroppingBox1.Left = x;
            imageCroppingBox1.Top = y;
            imageCroppingBox1.Size = new Size(width, height);

            if (imageCroppingBox1.IsDrawed)
            {
                imageCroppingBox1.SelectedRectangle = new Rectangle(
                    (int)(imageCroppingBox1.SelectedRectangle.X * newScale)
                    , (int)(imageCroppingBox1.SelectedRectangle.Y * newScale)
                      , (int)(imageCroppingBox1.SelectedRectangle.Width * newScale)
                      , (int)(imageCroppingBox1.SelectedRectangle.Height * newScale));
            }
            showCropInfo();
        }

        Action cropAction = null;

        void cropStart()
        {
            cropAction = () =>
            {
                if (FFplay.ffplay_get_w() < 1 || FFplay.ffplay_get_h() < 1)
                    return;
                imageCroppingBox1.Clear();
                imageCroppingBox1.Show();
                cropResize();
                cropAction = null;
            };

            timer2.Start();
        }

        private void makeselectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cropStart();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            cropAction?.Invoke();

            //if (_width > 0 && FFplay.ffplay_get_w() != _width)
            //{
            //    cropResize();
            //}

            if (imageCroppingBox1.Visible)
                imageCroppingBox1.Invalidate();
        }

        void cancelSelection()
        {
            imageCroppingBox1.Clear();
            imageCroppingBox1.Hide();
            timer2.Stop();
        }

        private void cancelselectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cancelSelection();
        }
    }
}
