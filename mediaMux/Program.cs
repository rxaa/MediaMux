using df;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MediaMux
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            dfv.setDpiAware();

            com.updateLang();
            com.setSubtitleFont();

            FormPlayer.onInit = (form) =>
            {
                com.init(form);
                com.initFont(form.contextMenuStrip1);
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
