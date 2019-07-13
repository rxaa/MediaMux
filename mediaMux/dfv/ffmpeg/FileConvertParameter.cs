using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace df
{

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
                        else if (val.Equals("0")) { 
                            if(name2)
                                return "no-" + name + "=1";
                            else
                                return name + "=0";
                        }
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
