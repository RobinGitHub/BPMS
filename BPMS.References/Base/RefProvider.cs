using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.References
{
    public class RefProvider
    {
        public static BPMSServiceRef BPMSServiceRefInstance = null;

        public static void Dispose()
        {
            if (BPMSServiceRefInstance != null)
                BPMSServiceRefInstance.Dispose();
        }

        public static void Init()
        {
            BPMSServiceRefInstance = new BPMSServiceRef();
        }
    }
}
