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
        const string FFdll = "ffmpeg\\FFmpegPlayer.dll";
        static Form playForm = null;

        public static void play(string name, IntPtr hwndParent, Form form)
        {
            if (playForm != null)
            {
                playForm.Close();
                Thread.Sleep(400);
            }

            ffplay_start(name, hwndParent);
            playForm = form;
        }

        public static void stop()
        {
            playForm = null;
            try
            {
                ffplay_on_error(null);
                ffplay_on_complete(null);
                ffplay_on_success(null);
                ffplay_stop();
            }
            catch (Exception)
            {
            }

        }


        [DllImport(FFdll)]
        public static extern int ffplay_set_vf(
               [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]
            string name);

        [DllImport(FFdll)]
        public static extern int ffplay_set_af(
               [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]
            string name);

        [DllImport(FFdll)]
        static extern int ffplay_start(
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]
            string name,
            IntPtr hwndParent);


        [DllImport(FFdll)]
        static extern int ffplay_stop();

        [DllImport(FFdll)]
        public static extern int ffplay_step_to_next_frame();

        [DllImport(FFdll)]
        public static extern int ffplay_resize(int w, int h);

        [DllImport(FFdll)]
        public static extern void ffplay_set_stop_show(int w);

        [DllImport(FFdll)]
        public static extern void ffplay_on_complete(Action on_complete);

        [DllImport(FFdll)]
        public static extern void ffplay_on_error(ActionString on_err);

        [DllImport(FFdll)]
        public static extern void ffplay_on_success(Action on_succ);



        // 0.stop 1.playing -1.pause
        [DllImport(FFdll)]
        public static extern int ffplay_get_state();

        [DllImport(FFdll)]
        public static extern int ffplay_toggle_pause();

        [DllImport(FFdll)]
        public static extern long ffplay_get_duration();

        [DllImport(FFdll)]
        public static extern long ffplay_get_position();

        [DllImport(FFdll)]
        public static extern int ffplay_set_position(long position);


        [DllImport(FFdll)]
        public static extern int ffplay_get_w();
        [DllImport(FFdll)]
        public static extern int ffplay_get_h();
        [DllImport(FFdll)]
        public static extern int ffplay_get_top();
        [DllImport(FFdll)]
        public static extern int ffplay_get_left();
        [DllImport(FFdll)]
        public static extern float ffplay_get_aspect_ratio();

        [DllImport(FFdll)]
        public static extern IntPtr ffprobe_file_info(
               [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(UTF8Marshaler))]
            string file);



        public static string probeFileInfo(string file)
        {
            return UTF8Marshaler.PtrToStr(ffprobe_file_info(file));
        }
    }
}
