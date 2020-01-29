using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace df
{


    public class ConvertMedia
    {
        public static string[] bitRates = new string[] {
            "32",
            "64",
            "128",
            "192",
            "256",
            "320",
            "384",
            "448",
            "512",
            "640",
        };

        public static string[] FPSlist = new string[] {
            "5",
            "10",
            "15",
            "20",
            "23.976",
            "24",
            "25",
            "29.97",
            "30",
            "59.94",
            "60",
            "119.88",
            "120"
        };
        public static string[] extAudios = new string[] { "mka", "m4a", "mp3", "ogg", "wav", "flac" };
        public static string[] extVideos = new string[] { "mkv", "mp4", "mov", "webm", "flv","ts","m2ts", "avi" };

        public static string[] codeAudios = new string[] { "aac", "mp3", "ac3", "eac3", "flac", "opus", "vorbis", "pcm_s16le", "pcm_s32le" };
        public static string[] codecVideos = new string[] { "", "h264", "h264_nvenc", "h264_qsv", "hevc", "hevc_nvenc", "hevc_qsv", "vp9" };

        public static string[] codecVideosStr
        {
            get
            {
                return new string[] { "", "h264", "h264_nvenc (NVIDIA NVENC)", "h264_qsv (Intel Quick Sync Video)", "hevc", "hevc_nvenc (NVIDIA NVENC)", "hevc_qsv (Intel Quick Sync Video)", "vp9" };
            }
        }


        public static string[] getPreset(string code)
        {
            if (code.Contains("nvenc"))
                return new string[] { "", "fast", "medium", "slow" };
            else if (code.Contains("qsv"))
                return new string[] { "", "veryfast", "faster", "fast", "medium",
        "slow","slower","veryslow" };
            else if (code.Contains("vp9"))
                return new string[] { "", "realtime", "good", "best" };

            return new string[] {  "","ultrafast", "superfast", "veryfast", "faster", "fast", "medium",
        "slow","slower","veryslow","placebo"};
        }

        public static string[] getCRF()
        {
            return Enumerable.Range(0, 45).Select(it =>
            {
                if (it == 0)
                    return "";
                if (it == 1)
                    return "0 (" + dfv.lang.dat.best_quality + ")";

                if (it == 24)
                    return "23 (" + dfv.lang.dat.default_ + ")";

                if (it == 44)
                    return "43 (" + dfv.lang.dat.low_quality + ")";
                return (it - 1) + "";
            }).ToArray();
        }

        public static string[] codeSizes = new string[] { "4096:-1", "3840:-1", "2560:-1", "1920:-1", "1280:-1", "960:-1", "720:-1", "640:-1", "480:-1", "320:-1", };

        public static string[] codePixelFormats = new string[] { "yuv420p", "yuv420p10le", "yuv422p", "yuv422p10le", "yuv444p", "yuv444p10le" };


        [DisplayNameDf("ext")]
        [DescriptionDf("ext_descr")]
        [TypeConverterAttribute(typeof(ExtsConverter))]
        public string ext { get; set; } = "mkv";


        [CategoryDf("audio_")]
        [DisplayNameDf("audio_code")]
        [DescriptionDf("audio_code_descr")]
        [TypeConverterAttribute(typeof(AudioCodeConverter))]
        public string audio_code { get; set; } = "";

        [CategoryDf("audio_")]
        [DisplayNameDf("audio_bit_rate")]
        [DescriptionDf("audio_bit_rate_descr")]
        [TypeConverterAttribute(typeof(bitRatesConverter))]
        public string audio_bit_rate { get; set; } = "";

        [CategoryDf("audio_")]
        [DisplayNameDf("audio_quality")]
        [DescriptionDf("audio_quality_descr")]
        [TypeConverterAttribute(typeof(NumberConverter))]
        public string audio_quality { get; set; } = "";

        [CategoryDf("audio_")]
        [DisplayNameDf("audio_channels")]
        [DescriptionDf("audio_channels_descr")]
        [TypeConverterAttribute(typeof(audio_channelsConverter))]
        public string audio_channels { get; set; } = "";

        [CategoryDf("audio_")]
        [DisplayNameDf("audio_sample_rate")]
        [DescriptionDf("audio_sample_rate_descr")]
        [TypeConverterAttribute(typeof(audio_sample_rateConverter))]
        public string audio_sample_rate { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_code")]
        [DescriptionDf("video_code_descr")]
        [TypeConverterAttribute(typeof(CodeVideosConverter))]
        public int video_codec { get; set; } = 0;

        [Browsable(false)]
        public string video_code
        {
            get
            {
                return codecVideos[video_codec];
            }
        }

        [CategoryDf("video_")]
        [DisplayNameDf("video_crf")]
        [DescriptionDf("video_crf_descr")]
        [TypeConverterAttribute(typeof(crfConverter))]
        public int video_crfs { get; set; } = 0;

        [CategoryDf("video_")]
        [DisplayNameDf("preset")]
        [DescriptionDf("preset_descr")]
        //[ConvertAttribute(name = "-preset", hasIndex = true)]
        [TypeConverterAttribute(typeof(CodeSpeedsConverter))]
        public string preset { get; set; } = "";


        [CategoryDf("video_")]
        [DisplayNameDf("video_bit_rate")]
        [DescriptionDf("video_bit_rate_descr")]
        public string video_bit_rate { get; set; } = "";


        [CategoryDf("video_")]
        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DisplayNameDf("two_pass")]
        [DescriptionDf("two_pass_descr")]
        [ConvertAttribute(yesNoValue = true)]
        public string video_2pass { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_max_bit_rate")]
        [DescriptionDf("video_max_bit_rate_descr")]
        public string video_max_bit_rate { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_buffer_size")]
        [DescriptionDf("video_buffer_size_descr")]
        public string video_buffer_size { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_display_size")]
        [DescriptionDf("video_display_size_descr")]
        [TypeConverterAttribute(typeof(CodeSizesConverter))]
        /// <summary>
        /// 1280x720 or w1280
        /// </summary>
        public string video_display_size { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_fps")]
        [DescriptionDf("video_fps_descr")]
        [ConvertAttribute(name = "-r", hasIndex = true)]
        [TypeConverterAttribute(typeof(FPSlistConverter))]
        public string video_fps { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_pixel_format")]
        [DescriptionDf("video_pixel_format_descr")]
        [ConvertAttribute(name = "-pix_fmt", hasIndex = true)]
        [TypeConverterAttribute(typeof(codePixelFormatsConverter))]
        public string video_pixel_format { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("video_tune")]
        [DescriptionDf("video_tune_descr")]
        [ConvertAttribute(name = "-tune", hasIndex = true)]
        [TypeConverterAttribute(typeof(codeTunesConverter))]
        public string video_tune { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("display_aspect_ratio")]
        [DescriptionDf("display_aspect_ratio_descr")]
        [ConvertAttribute(name = "-aspect", hasIndex = true)]
        [TypeConverterAttribute(typeof(display_aspect_ratioConverter))]
        public string display_aspect_ratio { get; set; } = "";


        [CategoryDf("video_")]
        [DisplayNameDf("profile")]
        [DescriptionDf("profile_descr")]
        [ConvertAttribute(name = "-profile", hasIndex = true)]
        [TypeConverterAttribute(typeof(ProfileConverter))]
        public string profile { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("level")]
        [DescriptionDf("level_descr")]
        [ConvertAttribute(name = "-level", hasIndex = true)]
        [TypeConverterAttribute(typeof(LevelConverter))]
        public string level { get; set; } = "";

        [CategoryDf("video_")]
        [DisplayNameDf("x26x_params")]
        [DescriptionDf("x26x_params_descr")]
        public x26xParams x26x_params { get; set; } = new x26xParams();

        [CategoryDf("video_")]
        [DisplayNameDf("x265_params")]
        [DescriptionDf("x265_params_descr")]
        public x265Params x265_params { get; set; } = new x265Params();


        [DisplayNameDf("command_line")]
        [DescriptionDf("command_line_descr")]
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))]
        public string command_line { get; set; } = "";

        [DisplayNameDf("video_filters")]
        [DescriptionDf("video_filters_descr")]
        public VideoFilter video_filters { get; set; } = new VideoFilter();

        [DisplayNameDf("audio_filters")]
        [DescriptionDf("audio_filters_descr")]
        public AudioFilter audio_filters { get; set; } = new AudioFilter();

    }


    public class ConvertAttribute : Attribute
    {
        public string name { get; set; } = "";
        public bool hasIndex { get; set; } = false;
        public bool noValue { get; set; } = false;
        public string[] lists { get; set; } = new string[] { };
        public string x265name { get; set; } = "";

        public bool yesNoValue { get; set; } = false;

        public bool escapeStr { get; set; } = true;
    }






    ///////////////////
}
