using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace df
{
    public class AppLanguage
    {
        public static string engLang = "english";

        public static string LangMenu
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "\\language\\";
            }
        }

        public static string getLangFile(string lang)
        {
            return LangMenu + lang + ".json";
        }

        public static ObjectFile<T> select<T>(string language) where T : class, new()
        {
            if (language == "")
            {
                language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }

            if (File.Exists(getLangFile(language)))
            {
                return new ObjectFile<T>(getLangFile(language));
            }
            return new ObjectFile<T>(getLangFile(engLang));
        }


        public static string getLang(string str)
        {
            var ty = dfv.lang.dat.GetType();
            var fi = ty.GetField(str);
            if (fi != null)
            {
                var val = fi.GetValue(dfv.lang.dat);
                if (val != null)
                {
                    return val + "";
                }
            }
            return str;
        }

        public static void SetLang(Control control)
        {
            if (control.Text != "")
            {
                var ty = dfv.lang.dat.GetType();
                var fi = ty.GetField(control.Text);
                if (fi != null)
                {
                    var val = fi.GetValue(dfv.lang.dat);
                    if (val != null)
                    {
                        control.Text = val + "";
                    }
                }
            }
        }


        public static void SetLang(ColumnHeader control)
        {
            if (control.Text != "")
            {
                var ty = dfv.lang.dat.GetType();
                var fi = ty.GetField(control.Text);
                if (fi != null)
                {
                    var val = fi.GetValue(dfv.lang.dat);
                    if (val != null)
                    {
                        control.Text = val + "";
                    }
                }
            }
        }

        public static void SetLang(ToolStripItem control)
        {
            if (control.Text != "")
            {
                var ty = dfv.lang.dat.GetType();
                var fi = ty.GetField(control.Text);
                if (fi != null)
                {
                    var val = fi.GetValue(dfv.lang.dat);
                    if (val != null)
                    {
                        control.Text = val + "";
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static void InitLanguage(Control control)
        {
            if (control is ListView)
            {
                var ls = (ListView)control;
                for (int i = 0; i < ls.Columns.Count; i++)
                {
                    SetLang(ls.Columns[i]);
                }

            }


            if (control is ToolStrip)
            {
                var ms = (ToolStrip)control;
                foreach (ToolStripItem con in ms.Items)
                {
                    if (!(con is ToolStripMenuItem))
                        continue;
                    SetLang(con);
                    foreach (ToolStripItem con2 in (con as ToolStripMenuItem).DropDownItems)
                    {
                        SetLang(con2);
                    }
                }
            }

            SetLang(control);

            foreach (Control ctrl in control.Controls)
            {
                InitLanguage(ctrl);
            }

            //工具栏或者菜单动态构建窗体或者控件的时候，重新对子控件进行处理
            //control.ControlAdded += (sender, e) =>
            //{
            //    InitLanguage(e.Control);
            //};
        }


    }
}
