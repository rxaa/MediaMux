using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{

    public delegate void ActionString(string obj);

    public class FFplay
    {
        static Form playForm = null;

        public static void play(string name, IntPtr hwndParent, Form form)
        {
            if (playForm != null)
            {
                playForm.Close();
                Thread.Sleep(300);
            }

            ffplay_start(name, hwndParent);
            playForm = form;
        }

        public static void stop()
        {
            playForm = null;
            try
            {
                ffplay_stop();
            }
            catch (Exception)
            {
            }
           
        }


        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_set_vf(string name);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_set_af(string name);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        static extern int ffplay_start(string name, IntPtr hwndParent);


        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        static extern int ffplay_stop();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_step_to_next_frame();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_resize(int w, int h);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern void ffplay_set_stop_show(int w);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern void ffplay_on_complete(Action on_complete);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern void ffplay_on_error(ActionString on_err);

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern void ffplay_on_success(Action on_succ);

        

        // 0.stop 1.playing -1.pause
        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_get_state();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_toggle_pause();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern long ffplay_get_duration();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern long ffplay_get_position();

        [DllImport("ffmpeg\\FFmpegPlayer.dll")]
        public static extern int ffplay_set_position(long position);
    }
}
