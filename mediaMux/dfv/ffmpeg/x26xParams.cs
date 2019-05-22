using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace df
{
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
        [ConvertAttribute(name = "constrained-intra", yesNoValue = true)]
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
        [DescriptionDf("early_skip")]
        [ConvertAttribute(name = "early-skip", yesNoValue = true)]
        public string early_skip { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("rskip_descr")]
        [ConvertAttribute(name = "rskip", yesNoValue = true)]
        public string rskip { get; set; } = "";


        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("splitrd_skip_descr")]
        [ConvertAttribute(name = "splitrd-skip", yesNoValue = true)]
        public string splitrd_skip { get; set; } = "";

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("fast_intra")]
        [ConvertAttribute(name = "fast-intra", yesNoValue = true)]
        public string fast_intra { get; set; } = "";

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("b_intra_descr")]
        [ConvertAttribute(name = " b-intra", yesNoValue = true)]
        public string b_intra { get; set; } = "";
       

        [TypeConverter(typeof(YesNoDefaultConverter))]
        [DescriptionDf("ssim_rd_descr")]
        [ConvertAttribute(name = "ssim-rd", yesNoValue = true)]
        public string ssim_rd { get; set; } = "";

    }

}
