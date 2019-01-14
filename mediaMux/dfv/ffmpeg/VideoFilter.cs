using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace df
{
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



    /////////////////
}
