using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AppUpdaterContracts
{
    [DataContract]
    public class UpFileInfo
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public long FileLength { get; set; }
        [DataMember]
        public string Version { get; set; }
    }
}
