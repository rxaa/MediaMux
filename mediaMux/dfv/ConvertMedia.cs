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
        public static string[] extAudios = new string[] { "mka", "m4a", "mp3", "ogg", "flac" };
        public static string[] extVideos = new string[] { "mkv", "mp4", "mov", "flv", "avi" };

        public static string[] codeAudios = new string[] { "aac", "mp3", "ac3", "flac", "opus", "vorbis" };
        public static string[] codecVideos = new string[] { "", "h264", "h264_nvenc", "h264_qsv", "hevc", "hevc_nvenc", "hevc_qsv" };

        public static string[] codecVideosStr
        {
            get
            {
                return new string[] { "", "h264", "h264_nvenc (NVIDIA NVENC)", "h264_qsv (Intel Quick Sync Video)", "hevc", "hevc_nvenc (NVIDIA NVENC)", "hevc_qsv (Intel Quick Sync Video)" };
            }
        }


        public static string[] getPreset(string code)
        {
            if (code.Contains("nvenc"))
                return new string[] { "", "fast", "medium", "slow" };
            else if (code.Contains("qsv"))
                return new string[] { "", "veryfast", "faster", "fast", "medium",
        "slow","slower","veryslow" };
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
        [ConvertAttribute(name = "-preset", hasIndex = true)]
        [TypeConverterAttribute(typeof(CodeSpeedsConverter))]
        public string preset { get; set; } = "";


        [CategoryDf("video_")]
        [DisplayNameDf("video_bit_rate")]
        [DescriptionDf("video_bit_rate_descr")]
        public string video_bit_rate { get; set; } = "";

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

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class VideoFilter
    {
        [DisplayNameDf("setpts")]
        [DescriptionDf("setpts_descr")]
        [ConvertAttribute(name = "setpts")]
        [TypeConverterAttribute(typeof(PTSConverter))]
        public string setpts { get; set; } = "";

        [DisplayNameDf("crop")]
        [DescriptionDf("crop_descr")]
        [EditorAttribute(typeof(PropertyGridVideoCrop), typeof(System.Drawing.Design.UITypeEditor))]
        public FilterCrop crop { get; set; } = new FilterCrop();

        [DisplayNameDf("scale")]
        [DescriptionDf("scale_descr")]
        [EditorAttribute(typeof(PropertyGridVideoCrop), typeof(System.Drawing.Design.UITypeEditor))]
        public FilterScale scale { get; set; } = new FilterScale();


        [DisplayNameDf("horizon_flip")]
        [ConvertAttribute(name = "hflip", noValue = true)]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string horizon_flip { get; set; } = "";

        [DisplayNameDf("vertical_flip")]
        [ConvertAttribute(name = "vflip", noValue = true)]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string vertical_flip { get; set; } = "";


        [DisplayNameDf("deinterlace")]
        [DescriptionDf("deinterlace_descr")]
        [ConvertAttribute(name = "bwdif")]
        [TypeConverterAttribute(typeof(DeinterlaceConverter))]
        public string deinterlace { get; set; } = "";

        [DisplayNameDf("delogo")]
        [DescriptionDf("delogo_descr")]
        [EditorAttribute(typeof(PropertyGridVideoCrop), typeof(System.Drawing.Design.UITypeEditor))]
        public FilterCrop delogo { get; set; } = new FilterCrop();

        [DisplayNameDf("denoise")]
        [DescriptionDf("denoise_descr")]
        [TypeConverterAttribute(typeof(NumberConverter))]
        [ConvertAttribute(name = "hqdn3d")]
        public string denoise { get; set; } = "";

        [DisplayNameDf("gradfun")]
        [DescriptionDf("gradfun_descr")]
        [TypeConverterAttribute(typeof(NumberConverter))]
        [ConvertAttribute(name = "gradfun")]
        public string gradfun { get; set; } = "";


        [ConvertAttribute(name = "eq")]
        [DisplayNameDf("color_adjustment")]
        [DescriptionDf("color_adjustment_descr")]
        public FilterEq color_adjustment { get; set; } = new FilterEq();


        [DisplayNameDf("blur_sharp")]
        [DescriptionDf("blur_sharp_descr")]
        [ConvertAttribute(name = "unsharp")]
        public FilterUnsharp blur_sharp { get; set; } = new FilterUnsharp();


        [DisplayNameDf("pad")]
        [DescriptionDf("pad_descr")]
        public FilterPad pad { get; set; } = new FilterPad();



        [DisplayNameDf("rotate")]
        [DescriptionDf("rotate_descr")]
        public FilterRotate rotate { get; set; } = new FilterRotate();

        [DisplayNameDf("setsar")]
        [DescriptionDf("setsar_descr")]
        [TypeConverterAttribute(typeof(SarConverter))]
        public string setsar { get; set; } = "";

        //[DisplayNameDf("command_line")]
        //public string command_line { get; set; } = "";

        public string getCMD()
        {
            return FileConvertParameter.reflectFilter(this);
        }
    }

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class FilterRotate
    {
        [DisplayNameDf("angle")]
        [DescriptionDf("angle_descr")]
        [TypeConverterAttribute(typeof(RotateConverter))]
        public string angle { get; set; } = "";

        [DisplayNameDf("out_w")]
        [DescriptionDf("out_w_descr")]
        public string out_w { get; set; } = "";

        [DisplayNameDf("out_h")]
        [DescriptionDf("out_h_descr")]
        public string out_h { get; set; } = "";

        [DisplayNameDf("fillcolor")]
        [DescriptionDf("fillcolor_descr")]
        [EditorAttribute(typeof(PropertyGridColor), typeof(System.Drawing.Design.UITypeEditor))]
        public string fillcolor { get; set; } = "";

    }

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class FilterUnsharp
    {
        [DisplayNameDf("luma_amount")]
        [DescriptionDf("luma_amount_descr")]
        [PropertySlide(defaul = 0, max = 150, min = -150, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string luma_amount { get; set; } = "";

        [DisplayNameDf("luma_msize_x")]
        [DescriptionDf("luma_msize_x_descr")]
        [TypeConverterAttribute(typeof(LumaSizeConverter))]
        public string luma_msize_x { get; set; } = "";

        [DisplayNameDf("luma_msize_y")]
        [DescriptionDf("luma_msize_y_descr")]
        [TypeConverterAttribute(typeof(LumaSizeConverter))]
        public string luma_msize_y { get; set; } = "";
    }

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class FilterEq
    {
        [DisplayNameDf("contrast")]
        [DescriptionDf("contrast_descr")]
        //[TypeConverterAttribute(typeof(NumberConverter))]
        [PropertySlide(defaul = 100, max = 200, min = 0, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string contrast { get; set; } = "";

        [DisplayNameDf("brightness")]
        [DescriptionDf("brightness_descr")]
        [PropertySlide(defaul = 0, max = 100, min = -100, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string brightness { get; set; } = "";

        [DisplayNameDf("saturation")]
        [DescriptionDf("saturation_descr")]
        [PropertySlide(defaul = 100, max = 300, min = 0, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string saturation { get; set; } = "";

        [DisplayNameDf("gamma_red")]
        [DescriptionDf("gamma_red_descr")]
        [ConvertAttribute(name = "gamma_r")]
        [PropertySlide(defaul = 100, max = 500, min = 1, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string gamma_red { get; set; } = "";

        [DisplayNameDf("gamma_green")]
        [DescriptionDf("gamma_green_descr")]
        [ConvertAttribute(name = "gamma_g")]
        [PropertySlide(defaul = 100, max = 500, min = 1, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string gamma_green { get; set; } = "";

        [DisplayNameDf("gamma_blue")]
        [DescriptionDf("gamma_blue_descr")]
        [ConvertAttribute(name = "gamma_b")]
        [PropertySlide(defaul = 100, max = 500, min = 1, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string gamma_blue { get; set; } = "";

        [DisplayNameDf("gamma_weight")]
        [DescriptionDf("gamma_weight_descr")]
        [PropertySlide(defaul = 100, max = 100, min = 0, step = 1, toFloat = 100)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string gamma_weight { get; set; } = "";
    }


    [TypeConverterAttribute(typeof(SubClassJSONConverter))]
    public class FilterCrop
    {
        [DisplayNameDf("x")]
        [DescriptionDf("x_descr")]
        public string x { get; set; } = "";

        [DisplayNameDf("y")]
        [DescriptionDf("y_descr")]
        public string y { get; set; } = "";


        [DisplayNameDf("w")]
        [DescriptionDf("w_descr")]
        public string w { get; set; } = "";

        [DisplayNameDf("h")]
        [DescriptionDf("h_descr")]
        public string h { get; set; } = "";
    }


    [TypeConverterAttribute(typeof(SubClassJSONConverter))]
    public class FilterPad
    {

        [DisplayNameDf("w")]
        [DescriptionDf("w_descr")]
        public string w { get; set; } = "";

        [DisplayNameDf("h")]
        [DescriptionDf("h_descr")]
        public string h { get; set; } = "";

        [DisplayNameDf("x")]
        [DescriptionDf("x_descr")]
        public string x { get; set; } = "";

        [DisplayNameDf("y")]
        [DescriptionDf("y_descr")]
        public string y { get; set; } = "";

        [DisplayNameDf("color")]
        [DescriptionDf("color_descr")]
        [EditorAttribute(typeof(PropertyGridColor), typeof(System.Drawing.Design.UITypeEditor))]
        public string color { get; set; } = "";

    }

    [TypeConverterAttribute(typeof(SubClassJSONConverter))]
    public class FilterScale
    {
        [DescriptionDf("wh_descr")]
        public string w { get; set; } = "";

        [DescriptionDf("wh_descr")]
        public string h { get; set; } = "";

        [DisplayNameDf("flags")]
        [DescriptionDf("flags_descr")]
        [TypeConverter(typeof(ScaleFlagConverter))]
        [ConvertAttribute(name = "flags")]
        public string flags { get; set; } = "";
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




    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class x26xParams
    {
        [DescriptionDf("keyint_max_descr")]
        [ConvertAttribute(name = "keyint")]
        [PropertySlide(defaul = 250, max = 600, min = 1, step = 1, toFloat = 1)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string keyint_max { get; set; } = "";

        [DescriptionDf("keyint_min_descr")]
        [ConvertAttribute(name = "keyint_min", x265name = "min-keyint")]
        [PropertySlide(defaul = 0, max = 250, min = 0, step = 1, toFloat = 1)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string keyint_min { get; set; } = "";

        [DescriptionDf("scenecut_descr")]
        [ConvertAttribute(name = "scenecut")]
        [PropertySlide(defaul = 40, max = 100, min = 0, step = 1, toFloat = 1)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string scenecut { get; set; } = "";

        [DescriptionDf("bframes_descr")]
        [PropertySlide(defaul = 4, max = 16, min = 0, step = 1, toFloat = 1)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string bframes { get; set; } = "";

        [DescriptionDf("qpmax_descr")]
        public string qpmax { get; set; } = "";

        [DescriptionDf("qpmin_descr")]
        public string qpmin { get; set; } = "";

        [DescriptionDf("qcomp_descr")]
        public string qcomp { get; set; } = "";

        [DescriptionDf("qpstep_descr")]
        public string qpstep { get; set; } = "";

        [DescriptionDf("rc_lookahead_descr")]
        [ConvertAttribute(name = "rc_lookahead", x265name = "rc-lookahead")]
        public string rc_lookahead { get; set; } = "";

        [DisplayNameDf("ref")]
        [DescriptionDf("ref_descr")]
        [ConvertAttribute(name = "ref", x265name = "ref")]
        public string ref_ { get; set; } = "";

        [DescriptionDf("psy_rd_descr")]
        [ConvertAttribute(name = "psy_rd", x265name = "psy-rd")]
        public string psy_rd { get; set; } = "";

        [DescriptionDf("psy_rdoq_descr")]
        [ConvertAttribute(x265name = "psy-rdoq")]
        public string psy_rdoq { get; set; } = "";

        [DescriptionDf("deblock_descr")]
        public string deblock { get; set; } = "";

        [DescriptionDf("aq_mode_descr")]
        [ConvertAttribute(x265name = "aq-mode")]
        public string aq_mode { get; set; } = "";

        [DescriptionDf("aq_strength_descr")]
        [ConvertAttribute(x265name = "aq-strength")]
        public string aq_strength { get; set; } = "";

        [DescriptionDf("merange_descr")]
        public string merange { get; set; } = "";

    }

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class x265Params
    {
        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("pmode_descr")]
        [ConvertAttribute(name = "pmode", yesNoValue = true)]
        public string pmode { get; set; } = "";


        [PropertyList(list = new string[] { "", "0", "1", "2", "3", "4", "5", "6" }, exclusive = false)]
        [TypeConverter(typeof(ListConverter))]
        [DescriptionDf("rd_descr")]
        [ConvertAttribute(name = "rd")]
        public string rd { get; set; } = "";

        [PropertyList(list = new string[] { "", "64", "32", "16" }, exclusive = false)]
        [TypeConverter(typeof(ListConverter))]
        [DescriptionDf("ctu_descr")]
        [ConvertAttribute(name = "ctu")]
        public string ctu { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("sao_descr")]
        [ConvertAttribute(name = "sao", yesNoValue = true)]
        public string sao { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("weightp_descr")]
        [ConvertAttribute(name = "weightp", yesNoValue = true)]
        public string weightp { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("weightb_descr")]
        [ConvertAttribute(name = "weightb", yesNoValue = true)]
        public string weightb { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("strong_intra_smoothing_descr")]
        [ConvertAttribute(name = "strong-intra-smoothing", yesNoValue = true)]
        public string strong_intra_smoothing { get; set; } = "";

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("constrained_intra_descr")]
        [ConvertAttribute(name = " constrained-intra", yesNoValue = true)]
        public string constrained_intra { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("rect_descr")]
        [ConvertAttribute(name = "rect", yesNoValue = true)]
        public string rect { get; set; } = "";

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("amp_descr")]
        [ConvertAttribute(name = "amp", yesNoValue = true)]
        public string amp { get; set; } = "";

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("ssim_rd_descr")]
        [ConvertAttribute(name = "ssim-rd", yesNoValue = true)]
        public string ssim_rd { get; set; } = "";

    }


    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class ImageOverlay
    {
        [DisplayNameDf("position")]
        [DescriptionDf("position_descr")]
        [TypeConverter(typeof(PositionConverter))]
        public string position { get; set; } = "";

        [DisplayNameDf("x")]
        [DescriptionDf("x_descr")]
        public string x { get; set; } = "";

        [DisplayNameDf("y")]
        [DescriptionDf("y_descr")]
        public string y { get; set; } = "";

        [DisplayNameDf("start_time")]
        [DescriptionDf("duration_descr")]
        [TypeConverterAttribute(typeof(TimeConverter))]
        public string start_time { get; set; } = "";


        [DisplayNameDf("end_time")]
        [DescriptionDf("duration_descr")]
        [TypeConverterAttribute(typeof(TimeConverter))]
        public string end_time { get; set; } = "";


        [DisplayNameDf("shortest")]
        [DescriptionDf("shortest_descr")]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string shortest { get; set; } = "";
    }

    [TypeConverterAttribute(typeof(SubClassConverter))]
    public class AudioFilter
    {
        [DisplayNameDf("delay")]
        [DescriptionDf("delay_descr")]
        [ConvertAttribute(name = "adelay")]
        public string delay { get; set; } = "";

        [DisplayNameDf("tempo")]
        [DescriptionDf("tempo_descr")]
        [ConvertAttribute(name = "atempo")]
        [PropertySlide(defaul = 10, max = 100, min = 5, step = 1, toFloat = 10)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string tempo { get; set; } = "";

        [DisplayNameDf("volume")]
        [DescriptionDf("volume_descr")]
        [ConvertAttribute(name = "volume")]
        [PropertySlide(defaul = 10, max = 100, min = 1, step = 1, toFloat = 10)]
        [EditorAttribute(typeof(PropertyGridSlide), typeof(System.Drawing.Design.UITypeEditor))]
        public string volume { get; set; } = "";

        [DisplayNameDf("dynaudnorm")]
        [DescriptionDf("dynaudnorm_descr")]
        [ConvertAttribute(name = "dynaudnorm", escapeStr = false)]
        [TypeConverterAttribute(typeof(dynaudnormConverter))]
        public string dynaudnorm { get; set; } = "";

        public string getCMD()
        {
            return FileConvertParameter.reflectFilter(this);
        }

    }

    public class FileConvertParameter
    {
        [DisplayNameDf("mix_audio")]
        [DescriptionDf("mix_audio_descr")]
        [CategoryDf("audio_")]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string mix_audio { get; set; } = "";

        [DisplayNameDf("mix_shortest")]
        [DescriptionDf("mix_shortest_descr")]
        [CategoryDf("audio_")]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string mix_shortest { get; set; } = "";

        [DisplayNameDf("audio_filters")]
        [DescriptionDf("audio_filters_descr")]
        [CategoryDf("audio_")]
        public AudioFilter audio_filters { get; set; } = new AudioFilter();


        [DisplayNameDf("fade_in")]
        [DescriptionDf("duration_descr")]
        [TypeConverterAttribute(typeof(TimeConverter))]
        [CategoryDf("video_")]
        public string fade_in { get; set; } = "";

        [DisplayNameDf("fade_out")]
        [DescriptionDf("duration_descr")]
        [TypeConverterAttribute(typeof(TimeConverter))]
        [CategoryDf("video_")]
        public string fade_out { get; set; } = "";

        [DisplayNameDf("overlay")]
        [DescriptionDf("overlay_descr")]
        [CategoryDf("video_")]
        public ImageOverlay overlay { get; set; } = new ImageOverlay();


        [DisplayNameDf("filters")]
        [DescriptionDf("filters_descr")]
        [CategoryDf("video_")]
        public VideoFilter filters { get; set; } = new VideoFilter();

        [DisplayNameDf("start_time")]
        [DescriptionDf("duration_descr")]
        //[TypeConverterAttribute(typeof(TimeConverter))]
        [EditorAttribute(typeof(PropertyGridVideoTime), typeof(System.Drawing.Design.UITypeEditor))]
        [ConvertAttribute(name = "-ss"), CategoryDf("time")]
        public string start_time { get; set; } = "";

        [EditorAttribute(typeof(PropertyGridVideoTime), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayNameDf("end_time")]
        [DescriptionDf("duration_descr")]
        //[TypeConverterAttribute(typeof(TimeConverter))]
        [ConvertAttribute(name = "-to"), CategoryDf("time")]
        public string end_time { get; set; } = "";

        [EditorAttribute(typeof(PropertyGridVideoTime), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayNameDf("duration")]
        [DescriptionDf("duration_descr")]
        //[TypeConverterAttribute(typeof(TimeConverter))]
        [ConvertAttribute(name = "-t"), CategoryDf("time")]
        public string duration { get; set; } = "";


        [DisplayNameDf("eof_time")]
        [DescriptionDf("eof_time_descr")]
        [ConvertAttribute(name = "-sseof"), CategoryDf("time")]
        [TypeConverterAttribute(typeof(TimeConverter))]
        public string eof_time { get; set; } = "";

        [TypeConverterAttribute(typeof(YesNoConverter))]
        [DisplayNameDf("loop")]
        [DescriptionDf("loop_description")]
        [ConvertAttribute(name = "-loop")]
        public string loop { get; set; } = "";


        [TypeConverterAttribute(typeof(YesNoConverter))]
        [DisplayNameDf("concat")]
        [DescriptionDf("concat_descr")]
        public string concat { get; set; } = "";

        [Browsable(false)]
        public string fileName { get; set; } = "";

        public static string getFieldStr(PropertyInfo field, object obj, bool name2 = false)
        {
            var att = field.GetCustomAttribute<ConvertAttribute>();
            var val = field.GetValue(obj);
            var name = field.Name;

            if (val != null && val.ToString() != "")
            {
                if (att != null)
                {
                    if (att.name != "")
                        name = att.name;

                    if (name2 && att.x265name != "")
                        name = att.x265name;

                    if (att.yesNoValue)
                    {
                        if (val.Equals("1"))
                            return name + "=1";
                        else if (val.Equals("0"))
                            return "no-" + name + "=1";
                        else
                            return null;
                    }

                    if (att.noValue)
                        return name;

                    if (att.escapeStr == false)
                        return name + "=" + val.ToString();
                }

                return name + "=" + dfv.strEscape(val.ToString());
            }
            return null;
        }

        public static string getFiledName(PropertyInfo field)
        {
            var name = field.Name;
            var att = field.GetCustomAttribute<ConvertAttribute>();
            if (att != null)
            {
                if (att.name != "")
                    name = att.name;
            }
            return name;
        }

        public static string reflectFilter(object filters)
        {
            if (filters is VideoFilter)
            {
                var fi = filters as VideoFilter;
                if (fi.pad.h != "" && fi.pad.y == "")
                {
                    fi.pad.y = "(oh-ih)/2";
                }
            }

            return filters.GetType().GetProperties().JoinStr(",", fi =>
            {
                if (fi.PropertyType == typeof(string))
                {
                    return getFieldStr(fi, filters);
                }
                else
                {
                    var val = fi.GetValue(filters);
                    if (val == null)
                        return null;
                    var subStr = fi.PropertyType.GetProperties().JoinStr(":", sf => sf.PropertyType == typeof(string) ? getFieldStr(sf, val) : null);
                    if (subStr != "")
                        return getFiledName(fi) + "=" + subStr;
                    return null;

                }
            });
        }

    }

}
