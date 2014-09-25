using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BPMS.BLL
{
    public class BaseBLL : IDisposable
    {
        protected BllProvider BLLProvider { get; set; }

        public BaseBLL()
        {
            this.BLLProvider = new BllProvider();
        }

        public BaseBLL(BllProvider provider)
        {
            this.BLLProvider = provider;
        }

        ~BaseBLL()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (this.BLLProvider != null)
            {
                this.BLLProvider.Dispose();
                this.BLLProvider = null;
            }
        }
    }
}
