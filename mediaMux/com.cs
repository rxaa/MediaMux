using df;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaMux
{
    public class com
    {
        public static ObjectFile<ConfigFile> cfg = new ObjectFile<ConfigFile>();
        public static ObjectFile<CodeListCfg> codes = new ObjectFile<CodeListCfg>();
        public static ObjectFile<LanguageFile> lang = null;

        public static string getVer()
        {
            var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return v.Major + "." + v.Minor + "." + v.Build;
        }

        public static string getLangStr()
        {
            if (com.cfg.dat.languageFile != "")
                return com.cfg.dat.languageFile;
            return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        }

        public static void updateLang()
        {
            com.lang = AppLanguage.select<LanguageFile>(com.cfg.dat.languageFile);
            dfv.lang = com.lang;
        }

        public static string colorToFF(Color c, int transparency)
        {
            var str = "&H";
            str += (255 * transparency / 100).ToString("X2");
            str += c.B.ToString("X2");
            str += c.G.ToString("X2");
            str += c.R.ToString("X2");
            str += "&";
            return str;
        }
        public static void setSubtitleFont()
        {
            FFmpeg.getSubtitle = (subtName, stream) =>
            {
                if (stream.codec_name == "sami")
                    return "";

                var res = "subtitles='" + dfv.strEscape(subtName) + "'";

                if (stream.codec_name == "ass" && com.cfg.dat.override_ass != "1")
                    return res;

                res += ":force_style='";
                if (com.cfg.dat.subtitle_font != null)
                {
                    res += "FontName=" + com.cfg.dat.subtitle_font.Name;
                    res += ",Fontsize=" + com.cfg.dat.subtitle_font.Size;
                    if (com.cfg.dat.subtitle_font.Bold)
                    {
                        res += ",Bold=-1";
                    }
                    if (com.cfg.dat.subtitle_font.Italic)
                    {
                        res += ",Italic=-1";
                    }
                }
                res += ",PrimaryColour=" + colorToFF(com.cfg.dat.subtitle_color, cfg.dat.subtitle_transparency);
                res += ",OutlineColour=" + colorToFF(com.cfg.dat.subtitle_outline_color, cfg.dat.subtitle_transparency);
                res += ",BackColour=" + colorToFF(com.cfg.dat.subtitle_shadow_color, cfg.dat.subtitle_transparency);
                res += ",Outline=" + com.cfg.dat.subtitle_outline;
                res += ",Shadow=" + com.cfg.dat.subtitle_shadow;

                if (com.cfg.dat.subtitle_alignment > 0)
                    res += ",Alignment=" + com.cfg.dat.subtitle_alignment;

                if (com.cfg.dat.margin_vertical != "")
                {
                    res += ",MarginV=" + com.cfg.dat.margin_vertical;
                }


                res += "'";
                return res;
            };
        }

        public static void resizeButtonImg(Button buttonAdd)
        {
            if (buttonAdd.Image == null)
                return;
            buttonAdd.Image = new Bitmap(buttonAdd.Image, buttonAdd.Height - 9, buttonAdd.Height - 9);
            buttonAdd.ImageAlign = ContentAlignment.MiddleRight;
            buttonAdd.TextImageRelation = TextImageRelation.ImageBeforeText;
        }

        public static void initFont(Control form)
        {
            if (com.cfg.dat.font_ui != null)
            {
                form.Font = com.cfg.dat.font_ui;
            }
        }

        public static void init(Form form)
        {
            AppLanguage.SetLang(form);
            form.setIcon();
            AppLanguage.InitLanguage(form);
            initFont(form);
        }


        public static string homeUrl()
        {
            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
               return "https://www.mediamux.net/";
            else
                return "https://mediamux.net/";
        }
    }
}
