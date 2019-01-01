using df;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Forms;

public class dfv
{
    /// <summary>
    /// close console
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();

    /// <summary>
    /// open console
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();

    public static IObjectFile<LangDfv> lang = new ObjectFile<LangDfv>();

    public static void setDpiAware()
    {
        //
        if (Environment.OSVersion.Version.Major >= 6)
            SetProcessDPIAware();
    }

    static double dpiX_ = 0;
    static double dpiY_ = 0;
    static void getDpi()
    {
        if (dpiX_ == 0)
        {
            using (var dpi_ = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX_ = dpi_.DpiX / 96;
                dpiY_ = dpi_.DpiY / 96;
            }
        }
    }

    public static double fixPixX(double val)
    {
        getDpi();
        return ((int)(val * dpiX_)) / dpiX_;
    }

    public static double fixPixY(double val)
    {
        getDpi();
        return ((int)(val * dpiY_)) / dpiY_;
    }


    public static string strEscape(string str)
    {
        return System.Text.RegularExpressions.Regex.Escape(str)
            .Replace(":", "\\:")
            .Replace(";", "\\;")
             .Replace("]", "\\]")
             .Replace("'", "'\\\\\\''")
             //.Replace("-", "\\-")
             //.Replace("!", "\\!")
             //.Replace("_", "\\_")
            ;
    }

    public static string codeWord = "0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()_+-=[];',./{}:<>?";

    public static string getRandmonStr(int len)
    {
        string ret = "";
        Random rd = new Random();
        for (int i = 0; i < len; i++)
        {
            ret += codeWord.Substring(rd.Next(0, 36), 1);
        }
        return ret;
    }

    public static void msg(string str, string tit = "")
    {
        if (tit == "") tit = dfv.lang.dat.Info;
        MessageBox.Show(str, tit, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void msgERR(string str, string tit = "")
    {
        if (tit == "") tit = dfv.lang.dat.Error;
        MessageBox.Show(str, tit, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    public static bool msgOK(string str, string tit = "")
    {
        if (tit == "") tit = dfv.lang.dat.Info;
        return MessageBox.Show(str, tit, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
    }

    public static bool msgYes(string str, string tit = "")
    {
        if (tit == "") tit = dfv.lang.dat.Info;
        return MessageBox.Show(str, tit, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    public static void msgWarn(string msg, string tit = "")
    {
        if (tit == "") tit = dfv.lang.dat.Warning;
        MessageBox.Show(msg, tit, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// open error message dialog
    /// </summary>
    public static int errMsg = 1;

    public static string getLogFile()
    {
        return AppDomain.CurrentDomain.BaseDirectory + "\\errlog.txt";
    }
    /// <summary>
    /// write log to ErrLog.txt
    /// </summary>
    /// <param name="con"></param>
    /// <returns></returns>
    public static bool log(string con, string path = "")
    {
        if (path == "")
        {
            path = getLogFile();
        }
        try
        {
            try
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Length > 10 * 1024 * 1024)
                {
                    fi.Delete();
                }
            }
            catch (Exception)
            {
            }
            File.AppendAllText(path
            , "------" + DateTime.Now + "------\r\n" + con + "\r\n\r\n"
            , Encoding.UTF8);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// get file extension(not include .)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string getFileExt(string name)
    {
        int ext = name.LastIndexOf(".");
        if (ext < 0)
            return "";
        string aLastName = name.Substring(ext + 1, (name.Length - ext - 1));
        return aLastName.ToLower();
    }

    /// <summary>
    /// remove file extension(include .)
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string removeFileExt(string name)
    {
        int ext = name.LastIndexOf(".");
        if (ext < 0)
            return name;
        return name.Substring(0, ext);
    }

    public static string toMultiLine(string str, int len)
    {
        var res = "";
        var count = 0;
        while (count + len <= str.Length)
        {
            res += str.Substring(count, len) + "\r\n";
            count += len;
        }
        if (count < str.Length)
        {
            res += str.Substring(count);
        }
        return res;
    }

    public static string saveFile(Action<SaveFileDialog> para = null)
    {

        SaveFileDialog openFileDialog = new SaveFileDialog();
        openFileDialog.Title = dfv.lang.dat.SaveFile;
        openFileDialog.Filter = dfv.lang.dat.AllFiles + "|*.*";
        openFileDialog.FileName = string.Empty;
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        para?.Invoke(openFileDialog);
        var result = openFileDialog.ShowDialog();
        if (result != DialogResult.OK)
        {
            return "";
        }
        return openFileDialog.FileName;
    }

    public static string[] openFile(Action<OpenFileDialog> para = null)
    {

        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = dfv.lang.dat.SelectFile;
        openFileDialog.Filter = dfv.lang.dat.AllFiles + "|*.*";
        openFileDialog.FileName = string.Empty;
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        para?.Invoke(openFileDialog);

        var result = openFileDialog.ShowDialog();
        if (result != DialogResult.OK)
        {
            return new string[] { };
        }
        return openFileDialog.FileNames;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool SetClipboard(string str)
    {
        try
        {
            Clipboard.SetDataObject(str, true);
        }
        catch (System.Exception ex)
        {
            try
            {
                Clipboard.Clear();
            }
            catch (Exception err)
            {
            }
            dfv.msgERR("copy error:" + ex.Message);
            return false;
        }
        return true;
    }

    public static string joinStr(string separate, params string[] strs)
    {
        var ret = "";
        foreach (var ss in strs)
        {
            if (ss != null && ss != "")
            {
                ret += ss + separate;
            }
        }
        if (ret.Length > 0)
            ret = ret.Substring(0, ret.Length - separate.Length);
        return ret;
    }

    /// <summary>
    /// get an unrepeat file name
    /// </summary>
    /// <param name="file">file origin</param>
    /// <param name="extName">new extension</param>
    /// <returns></returns>
    public static string getFile2(string file, string extName = "")
    {

        if (file == "")
            return "";
        int extI = file.LastIndexOf(".");
        var name = file;
        if (extI >= 0)
            name = file.Substring(0, extI);
        var ext = getFileExt(file);
        if (extName == "")
        {
            extName = ext;
        }

        var newName = name + "." + extName;
        for (int i = 2; i < 10000; i++)
        {
            if (File.Exists(newName))
            {
                newName = name + "_" + i + "." + extName;
            }
            else
            {
                return newName;
            }
        }
        return name + "_new." + extName;
    }

    public static string timeToStr(long mss)
    {
        long days = mss / (1000 * 60 * 60 * 24);
        long hours = (mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60);
        long minutes = (mss % (1000 * 60 * 60)) / (1000 * 60);
        long seconds = (mss % (1000 * 60)) / 1000;
        var ret = "";
        if (days > 0)
            ret += days + "d ";
        if (hours > 0)
            ret += hours + "h ";
        if (minutes > 0)
            ret += minutes + "m ";

        return ret + seconds + "s";
    }
    public static string numberFix2(long num)
    {
        if (num < 10)
            return "0" + num;
        return num + "";
    }



    public static string timeToStr2(long mss, bool trimMilli = true)
    {
        long days = mss / (1000 * 60 * 60 * 24);
        long hours = (mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60);
        long minutes = (mss % (1000 * 60 * 60)) / (1000 * 60);
        long seconds = (mss % (1000 * 60)) / 1000;

        var ret = "";
        if (hours > 0)
            ret += hours + ":";
        ret += numberFix2(minutes) + ":";

        ret += numberFix2(seconds) + "";

        if (!trimMilli)
        {
            long mill = (mss % (1000));
            ret += "." + numberFix2(mill);
        }

        return ret;
    }

    /// <summary>
    /// time string to millisecond
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long timeStrToLong(string time)
    {
        try
        {
            if (time.Length < 1)
                return 0;
            var sec = time.Split('.');
            if (sec.Length < 1)
                return 0;

            long millsec = 0;
            if (sec.Length == 2)
            {
                if (sec[1].Length > 3)
                    millsec = long.Parse(sec[1].Substring(0, 3));
                else if (sec[0].Length == 2)
                    millsec = long.Parse(sec[1]) * 10;
                else if (sec[0].Length == 1)
                    millsec = long.Parse(sec[1]) * 100;
            }

            var hour = sec[0].Split(':');
            if (hour.Length < 1)
                return millsec;

            if (hour.Length > 2)
            {
                millsec += long.Parse(hour[hour.Length - 3]) * 60 * 60 * 1000;
            }

            if (hour.Length > 1)
            {
                millsec += long.Parse(hour[hour.Length - 2]) * 60 * 1000;
            }

            if (hour.Length > 0)
            {
                millsec += long.Parse(hour[hour.Length - 1]) * 1000;
            }
            return millsec;
        }
        catch (Exception)
        {

            return 0;
        }


    }


    public static T reflectClone<T>(T fromObj, T toObj) where T : class
    {
        //FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //for (int i = 0; i < fields.Length; i++)
        //{
        //    FieldInfo field = fields[i];
        //    field.SetValue(toObj, field.GetValue(fromObj));
        //}

        PropertyInfo[] properties = fromObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];
            if (property.CanWrite && property.CanRead)
                property.SetValue(toObj, property.GetValue(fromObj));
        }
        return toObj;
    }

    ///////////////////////////////////////////

}
