using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace df
{

    public interface IObjectFile<out T> where T : class, new()
    {
        string fileMenu
        {
            get;
            set;
        }

        T dat
        {
            get;
        }

        void save();

        bool load();
    }

    public class ObjectFile<T> : IObjectFile<T> where T : class, new()
    {
        public string fileMenu
        {
            get;
            set;
        }

        public ObjectFile(string menu = "")
        {
            if (menu == "")
            {
                this.fileMenu = AppDomain.CurrentDomain.BaseDirectory + "\\" + typeof(T).Name + ".json";
            }
            else
            {
                this.fileMenu = menu;
            }
        }
        public T dat
        {
            get
            {
                load();
                return _dat;
            }
            set
            {
                _dat = value;
            }
        }

        public void save()
        {
            if (_dat != null)
            {
                var res = JsonConvert.SerializeObject(_dat, Formatting.Indented);

                try
                {
                    File.WriteAllText(fileMenu, res);
                }
                catch (DirectoryNotFoundException)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileMenu));
                    File.WriteAllText(fileMenu, res);
                }
            }
        }
        JsonSerializerSettings setting = new JsonSerializerSettings();
        public bool load()
        {
            if (_dat == null)
            {
                try
                {
                    var res = File.ReadAllText(fileMenu);
                    _dat = JsonConvert.DeserializeObject<T>(res);
                }
                catch (Exception ce)
                {
                    dfv.log(ce + "");
                    _dat = new T();
                }
            }
            return true;
        }
        private T _dat = null;
    }
}
