using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace df
{
    public class AudioCodeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "" }.Concat(ConvertMedia.codeAudios).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }


    public class bitRatesConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "" }.Concat(ConvertMedia.bitRates).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }



    public class audio_channelsConverter : ComboBoxItemTypeConvert
    {
        public override Dictionary<string, object> GetConvertMap()
        {
            var dict = new Dictionary<string, object>() {
                {"","" },
                {"1 (mono)","1" },
                {"2 (stereo)","2" },
                {"6 (5.1)","6" },
                {"8 (7.1)","8" },
            };

            return dict;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class audio_sample_rateConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "8000", "12000", "16000", "22050", "24000", "32000", "44100", "48000", "64000", "88200", "96000" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }



    public class CodeVideosConverter : ComboBoxItemTypeConvert
    {
        public override Dictionary<string, object> GetConvertMap()
        {
            var dict = new Dictionary<string, object>();
            ConvertMedia.codecVideosStr.ForEach((s, i) =>
            {
                dict.Add(s, i);
            });
            return dict;
        }
    }

    public class crfConverter : ComboBoxItemTypeConvert
    {
        public override Dictionary<string, object> GetConvertMap()
        {
            var dict = new Dictionary<string, object>() { };
            ConvertMedia.getCRF().ForEach((s, i) =>
            {
                dict.Add(s, i);
            });
            return dict;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

    }

    public class dynaudnormConverter : ComboBoxItemTypeConvert
    {
        public override Dictionary<string, object> GetConvertMap()
        {
            var dict = new Dictionary<string, object>() {
                { dfv.lang.dat.Yes , "f=200:g=11:p=0.95:m=10.0:r=0.0:n=1:c=0:b=1:s=0.0"},
                { dfv.lang.dat.No , ""},
            };
            return dict;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class FPSlistConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "" }.Concat(ConvertMedia.FPSlist).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }


    public class CodeSpeedsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var codec = "";
            var obj = context.Instance;
            if (obj is ConvertMedia)
            {
                codec = (obj as ConvertMedia).video_code;
            }
            return new StandardValuesCollection(ConvertMedia.getPreset(codec));
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }


    public class display_aspect_ratioConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "4:3", "16:9", "1.85:1", "2.35:1" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class ProfileConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "baseline", "main", "high", "high10", "high422", "high444" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class LevelConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "3.0", "3.1", "4.0", "4.1", "4.2", "5", "5.0", "5.1" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class CodeSizesConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "" }.Concat(ConvertMedia.codeSizes).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class codeTunesConverter : StringConverter
    {
        public static string[] codeTunes = new string[] { "fastdecode", "zerolatency", "psnr", "ssim" };
        public static string[] codeTunesx264 = new string[] { "film", "animation", "grain", "stillimage", };


        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var obj = context.Instance;
            if (obj is ConvertMedia)
            {
                if ((obj as ConvertMedia).video_code.Contains("h264"))
                {
                    return new StandardValuesCollection(new string[] { "" }.Concat(codeTunesx264).Concat(codeTunes).ToList());
                }
            }
            return new StandardValuesCollection(new string[] { "" }.Concat(codeTunes).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class codePixelFormatsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "" }.Concat(ConvertMedia.codePixelFormats).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class ExtsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(ConvertMedia.extVideos.Concat(ConvertMedia.extAudios).ToList());
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class DeinterlaceConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "0", "1", "0:-1:1" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class RotateConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "45*PI/180", "90*PI/180", "180*PI/180", "270*PI/180" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }


    public class ScaleFlagConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "fast_bilinear", "bilinear", "bicubic", "experimental", "neighbor", "area", "bicublin", "gauss", "sinc", "lanczos", "spline" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
    public class PositionConverter : ComboBoxItemTypeConvert
    {
        public override Dictionary<string, object> GetConvertMap()
        {
            return new Dictionary<string, object>() {
                { "","" },
                { dfv.lang.dat.Left_top,"0" },
                 { dfv.lang.dat.Right_top,"1" },
                  { dfv.lang.dat.Right_bottom,"2" },
                   { dfv.lang.dat.Left_bottom,"3" },
            };
        }
    }

    public class TimeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "0.5", "1", "1.5", "5", "10", "30", "1:0", "2:0", "1:0:0" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }


    public class LumaSizeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "3", "5", "7", "9", "11", "13", "15", "17", "19", "21", "23" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }


    public class SarConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "1:1" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

    public class PTSConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "", "0.1*PTS", "0.5*PTS", "0.6*PTS", "0.8*PTS", "1*PTS", "1.1*PTS", "1.5*PTS", "2*PTS" });
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

   
}
