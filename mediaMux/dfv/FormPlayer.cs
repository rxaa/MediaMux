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

        bool isDragging = false;
        private void FormPlayer_Load(object sender, EventArgs e)
        {
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
                FFplay.ffplay_set_vf(af);
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
                     FFplay.ffplay_on_success(() =>
                     {
                         this.BeginInvoke(new Action(() =>
                         {
                             timer1.Start();
                         }));

                     });
                     FFplay.play(name
                                , pictureBox1.Handle
                                , this);
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
    }
}
