using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace df
{
    public class MediaFile
    {
        public List<MediaStream> streams { get; set; } = new List<MediaStream>();
        public MediaFileFormat format { get; set; } = new MediaFileFormat();
        public List<ChapterFormat> chapters { get; set; } = new List<ChapterFormat>();

    }

    public class MediaFileObj
    {
        public JObject format { get; set; } = new JObject();
        public List<ChapterFormat> chapters { get; set; } = new List<ChapterFormat>();
        public List<JObject> streams { get; set; } = new List<JObject>();
        public MediaError error { get; set; } = new MediaError();

        public MediaFile toMediaFile()
        {
            var mf = new MediaFile();
            streams.ForEach(s =>
            {
                var m = s.ToObject<MediaStream>();
                m.streamObj = s;
                m.tagsEdit = new Dictionary<string, string>(m.tags);
                m.disposition2 = m.disposition.Clone();
                mf.streams.Add(m);
            });
            mf.format = format.ToObject<MediaFileFormat>();
            mf.chapters = chapters;
            return mf;
        }

    }


    public class MediaError
    {
        public long code { get; set; } = 0;
        [JsonProperty(PropertyName = "string")]
        public string message { get; set; } = "";
    }

    public class ChapterFormat
    {
        public long id { get; set; } = 0;
        public long start { get; set; } = 0;
        public long end { get; set; } = 0;
        public Dictionary<string, string> tags { get; set; } = new Dictionary<string, string>();
    }

    public class MediaFileFormat
    {
        public string format_name { get; set; } = "";
        public string duration { get; set; } = "";
        public string size { get; set; } = "";
        public string bit_rate { get; set; } = "";
        public Dictionary<string, string> tags { get; set; } = new Dictionary<string, string>();

        [JsonIgnore]
        public long durationMilli { get; set; } = 0;
    }

    public class StreamDisposition
    {
        [JsonProperty(PropertyName = "default")]
        public int defaul { get; set; } = 0;
        public int dub { get; set; } = 0;
        public int original { get; set; } = 0;
        public int comment { get; set; } = 0;
        public int lyrics { get; set; } = 0;
        public int karaoke { get; set; } = 0;
        public int forced { get; set; } = 0;
        public int hearing_impaired { get; set; } = 0;
        public int visual_impaired { get; set; } = 0;
        public int clean_effects { get; set; } = 0;
        public int attached_pic { get; set; } = 0;
        public int timed_thumbnails { get; set; } = 0;

        public StreamDisposition Clone()
        {
            return dfv.reflectClone(this, new StreamDisposition());
        }
    }


    public class MediaStream
    {

        public int index { get; set; } = 0;
        /// <summary>
        /// video audio subtitle attachment
        /// </summary>
        public string codec_type { get; set; } = "";
        /// <summary>
        /// aac h264 ......
        /// </summary>
        public string codec_name { get; set; } = "";
        public string codec_long_name { get; set; } = "";
        public string codec_time_base { get; set; } = "";
        public string codec_tag_string { get; set; } = "";
        public string profile { get; set; } = "";
        public int width { get; set; } = 0;
        public int height { get; set; } = 0;
        public int coded_width { get; set; } = 0;
        public int coded_height { get; set; } = 0;
        public string sample_fmt { get; set; } = "";
        public string sample_rate { get; set; } = "";
        public string pix_fmt { get; set; } = "";
        public string bit_rate { get; set; } = "";
        public string r_frame_rate { get; set; } = "";
        public string avg_frame_rate { get; set; } = "";
        public int channels { get; set; } = 0;
        public string channel_layout { get; set; } = "";
        public string duration { get; set; } = "";
        public StreamDisposition disposition { get; set; } = new StreamDisposition();

        /// <summary>
        /// record edit result
        /// </summary>
        [JsonIgnore]
        public StreamDisposition disposition2 { get; set; } = new StreamDisposition();

        public Dictionary<string, string> tags { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// record edit result
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> tagsEdit { get; set; } = new Dictionary<string, string>();

        [JsonIgnore]
        public JObject streamObj { get; set; } = new JObject();
        [JsonIgnore]
        public ConvertMedia convert { get; set; } = new ConvertMedia();

        [JsonIgnore]
        public int fileIndex { get; set; } = 0;
        [JsonIgnore]
        public bool isAttachmentFile { get; set; } = false;
        [JsonIgnore]
        public bool selected { get; set; } = true;

        public static HashSet<string> formatPic = new HashSet<string>()
        {
            "jpg","mjpeg","png","gif","webp","bmp","psd",
        };

        public static HashSet<string> formatTxt = new HashSet<string>()
        {
            "ansi","txt",
        };

        public string getName()
        {
            var n = tags.GetStrVal("title");
            if (n == "")
                n = tags.GetStrVal("filename");
            return n;
        }

        public string getLang()
        {
            return tags.GetStrVal("language");
        }
        public void setLang(string str)
        {
            tags["language"] = str;
        }

        public string getBitRate()
        {
            if (bit_rate != "")
            {
                return (long)(long.Parse(bit_rate) / 1000) + "kb/s";
            }
            return "";
        }

        public bool isAudio()
        {
            return codec_type == "audio";
        }

        public bool isVideo()
        {
            return codec_type == "video";
        }

        public bool isPureVideo()
        {
            return codec_type == "video" && !isPic() && !formatTxt.Contains(codec_name);
        }

        public bool isPic()
        {
            return formatPic.Contains(codec_name);
        }

        public bool isSubtitle()
        {
            return codec_type == "subtitle";
        }

        public bool hasMovText(string ext)
        {
            return ext == "mp4" || ext == "mov";
        }

        public bool isMovText()
        {
            return codec_name == "mov_text";
        }

        public bool isImgSubtitle()
        {
            return codec_name == "hdmv_pgs_subtitle" || codec_name == "dvd_subtitle" || codec_name == "dvb_subtitle";
        }

        public string getText()
        {
            if (isAttachmentFile)
                return codec_long_name;

            var str = fileIndex + ":" + index + " " + AppLanguage.getLang(codec_type) + ": " + codec_name;
            if (codec_type == "video")
                str += " " + width + "*" + height;

            if (codec_type == "audio")
                str += " " + sample_rate + "hz";


            str += " " + getBitRate() + " " + getLang();

            if (disposition.defaul == 1)
                str += " " + dfv.lang.dat.default_;
            str += " " + getName();
            return str;
        }

        public string getDurationStr()
        {
            if (duration != "")
            {
                return duration;
            }
            var str = tags.GetStrVal("DURATION");
            if (str == "")
            {
                str = tags.GetStrVal("DURATION-eng");
            }
            return str;
        }

        public long getDuration()
        {
            return dfv.timeStrToLong(getDurationStr());
        }
    }

}
