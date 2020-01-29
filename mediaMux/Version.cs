using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMux
{
    class Version
    {
        public string version { get; set; } = "";

        public string context { get; set; } = "";

        public int lang { get; set; } = 1;


        public bool needUpdate()
        {
            if (version == null || version == "")
                return false;
            var verNew = version.Split('.');
            var verOld = com.getVer().Split('.');

            for (int i = 0; i < verOld.Length; i++)
            {
                var nI = int.Parse(verNew[i]);
                var oI = int.Parse(verOld[i]);
                if (i >= verNew.Length)
                    return false;

                if (nI < oI)
                    return false;

                if (nI> oI)
                    return true;
            }

            return false;
        }
    }
}
