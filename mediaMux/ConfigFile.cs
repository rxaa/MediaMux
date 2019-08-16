using df;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMux
{
    public class ConfigFile
    {
        [CategoryDf("GUI")]
        [DisplayNameDf("language_file")]
        [DescriptionDf("language_file_descr")]
        [TypeConverterAttribute(typeof(languageFileConverter))]
        public string languageFile { get; set; } = "";

        [CategoryDf("GUI")]
        [DisplayNameDf("font")]
        [DescriptionDf("font_descr")]
        public Font font_ui { get; set; } = null;


        [CategoryDf("GUI")]
        [DisplayNameDf("check_update")]
        [DescriptionDf("check_update_descr")]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string check_update { get; set; } = "1";

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_font")]
        [DescriptionDf("subtitle_font_descr")]
        public Font subtitle_font { get; set; } = null;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_color")]
        [DescriptionDf("subtitle_color_descr")]
        public Color subtitle_color { get; set; } = Color.White;


        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_outline")]
        [DescriptionDf("subtitle_outline_descr")]
        [TypeConverterAttribute(typeof(FloatConverter))]
        public float subtitle_outline { get; set; } = 0f;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_outline_color")]
        [DescriptionDf("subtitle_outline_color_descr")]
        public Color subtitle_outline_color { get; set; } = Color.Black;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_shadow")]
        [DescriptionDf("subtitle_shadow_descr")]
        [TypeConverterAttribute(typeof(FloatConverter))]
        public float subtitle_shadow { get; set; } = 1;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_shadow_color")]
        [DescriptionDf("subtitle_shadow_color_descr")]
        public Color subtitle_shadow_color { get; set; } = Color.DodgerBlue;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_transparency")]
        [DescriptionDf("subtitle_transparency_descr")]
        [TypeConverterAttribute(typeof(PercentConverter))]
        public int subtitle_transparency { get; set; } = 0;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("subtitle_alignment")]
        [DescriptionDf("subtitle_alignment_descr")]
        [TypeConverterAttribute(typeof(FloatConverter))]
        public int subtitle_alignment { get; set; } = 0;

        [CategoryDf("Subtitle")]
        [DisplayNameDf("margin_vertical")]
        [DescriptionDf("margin_vertical_descr")]
        [TypeConverterAttribute(typeof(NumberConverter))]
        public string margin_vertical { get; set; } = "";

        [CategoryDf("Subtitle")]
        [DisplayNameDf("override_ass")]
        [DescriptionDf("override_ass_descr")]
        [TypeConverterAttribute(typeof(YesNoConverter))]
        public string override_ass { get; set; } = "";

        [Browsable(false)]
        public int winWidth { get; set; } = 0;

        [Browsable(false)]
        public int winHeight { get; set; } = 0;


        [Browsable(false)]
        public int processPriority { get; set; } = 3;

        public static ProcessPriorityClass[] priorityArr = new ProcessPriorityClass[] {
            ProcessPriorityClass.RealTime,
            ProcessPriorityClass.High,
            ProcessPriorityClass.AboveNormal,
            ProcessPriorityClass.Normal,
            ProcessPriorityClass.BelowNormal,
            ProcessPriorityClass.Idle,





        };

    }
}
