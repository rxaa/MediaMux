using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace df
{

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
}
