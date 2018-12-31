using df;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMux
{
    public class CodeListCfg
    {
        public Dictionary<string, ConvertMedia> list { get; set; } = new Dictionary<string, ConvertMedia>();
    }
}
